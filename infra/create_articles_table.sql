-- Create articles table for Tech Hub
-- This script creates a table to store all frontmatter properties from all collections

-- Create articles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='articles' AND xtype='U')
BEGIN
    CREATE TABLE articles (
        id BIGINT IDENTITY(1,1) PRIMARY KEY NONCLUSTERED, -- Non-clustered for Azure Search change tracking
        
        -- Frontmatter properties from all collections
        title NVARCHAR(500) NOT NULL,
        description NVARCHAR(MAX),
        author NVARCHAR(200),
        canonical_url NVARCHAR(1000),
        feed_name NVARCHAR(200) NULL, -- Nullable since not all articles have feeds
        feed_url NVARCHAR(1000) NULL, -- Nullable since not all articles have feeds
        date DATETIME2 NOT NULL,
        permalink NVARCHAR(1000),
        categories NVARCHAR(MAX), -- JSON array as string, e.g., '["AI", "GitHub Copilot"]'
        tags NVARCHAR(MAX), -- JSON array as string, e.g., '["AI", "Code Comments", "Developer Tools"]'
        tags_normalized NVARCHAR(MAX), -- JSON array as string, e.g., '["ai", "code comments", "developer tools"]'
        
        -- Collection distinguishes between news, videos, community, posts, etc.
        collection NVARCHAR(100) NOT NULL, -- e.g., 'news', 'videos', 'community', 'posts'
        content NVARCHAR(MAX), -- Full article content (markdown or processed)
        
        -- Azure Search compatibility fields
        IsDeleted BIT NOT NULL DEFAULT 0, -- Soft delete support for Azure Search
        rowversion_timestamp ROWVERSION, -- Change tracking support for Azure Search
        
        -- Audit fields
        created_at DATETIME2 DEFAULT GETUTCDATE(),
        updated_at DATETIME2 DEFAULT GETUTCDATE()
    );
    
    PRINT 'Table articles created successfully';
END
ELSE
BEGIN
    PRINT 'Table articles already exists';
END

-- Create performance indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_articles_date' AND object_id = OBJECT_ID('articles'))
BEGIN
    CREATE INDEX IX_articles_date ON articles (date DESC);
    PRINT 'Index IX_articles_date created';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_articles_collection' AND object_id = OBJECT_ID('articles'))
BEGIN
    CREATE INDEX IX_articles_collection ON articles (collection);
    PRINT 'Index IX_articles_collection created';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_articles_IsDeleted' AND object_id = OBJECT_ID('articles'))
BEGIN
    CREATE INDEX IX_articles_IsDeleted ON articles (IsDeleted);
    PRINT 'Index IX_articles_IsDeleted created';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_articles_created_at' AND object_id = OBJECT_ID('articles'))
BEGIN
    CREATE INDEX IX_articles_created_at ON articles (created_at DESC);
    PRINT 'Index IX_articles_created_at created';
END

-- Create composite indexes for common query patterns
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_articles_date_collection' AND object_id = OBJECT_ID('articles'))
BEGIN
    CREATE INDEX IX_articles_date_collection ON articles (collection, date DESC);
    PRINT 'Index IX_articles_date_collection created';
END

-- Note: For full-text search on tags and categories, you would need to enable full-text search:
-- CREATE FULLTEXT CATALOG ft_catalog AS DEFAULT;
-- CREATE FULLTEXT INDEX ON articles (title, description, content, tags, categories) KEY INDEX PK__articles__id;

-- Create a view for easier querying with JSON parsing
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_articles_parsed')
BEGIN
    EXEC('
    CREATE VIEW vw_articles_parsed AS
    SELECT 
        id,
        title,
        description,
        author,
        canonical_url,
        feed_name,
        feed_url,
        date,
        permalink,
        collection,
        content,
        IsDeleted,
        rowversion_timestamp,
        created_at,
        updated_at,
        -- Parse JSON arrays for easier querying
        JSON_VALUE(categories, ''$[0]'') as primary_category,
        JSON_VALUE(tags, ''$[0]'') as primary_tag,
        -- Count of tags and categories
        (SELECT COUNT(*) FROM OPENJSON(categories)) as category_count,
        (SELECT COUNT(*) FROM OPENJSON(tags)) as tag_count,
        -- Derived viewing_mode based on collection
        CASE collection
            WHEN ''videos'' THEN ''internal''
            WHEN ''magazines'' THEN ''internal''
            ELSE ''external''
        END as viewing_mode
    FROM articles
    WHERE IsDeleted = 0; -- Only show non-deleted articles in view
    ');
    PRINT 'View vw_articles_parsed created successfully';
END

-- Create stored procedure for inserting articles
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_InsertArticle')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_InsertArticle
        @title NVARCHAR(500),
        @description NVARCHAR(MAX) = NULL,
        @author NVARCHAR(200) = NULL,
        @canonical_url NVARCHAR(1000) = NULL,
        @feed_name NVARCHAR(200) = NULL,
        @feed_url NVARCHAR(1000) = NULL,
        @date DATETIME2,
        @permalink NVARCHAR(1000) = NULL,
        @categories NVARCHAR(MAX) = NULL,
        @tags NVARCHAR(MAX) = NULL,
        @tags_normalized NVARCHAR(MAX) = NULL,
        @collection NVARCHAR(100),
        @content NVARCHAR(MAX) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        -- Check if article already exists by canonical_url and collection (including soft-deleted)
        IF EXISTS (SELECT 1 FROM articles WHERE canonical_url = @canonical_url AND collection = @collection AND @canonical_url IS NOT NULL)
        BEGIN
            -- Update existing article (including restoring soft-deleted ones)
            UPDATE articles 
            SET 
                title = @title,
                description = @description,
                author = @author,
                feed_name = @feed_name,
                feed_url = @feed_url,
                date = @date,
                permalink = @permalink,
                categories = @categories,
                tags = @tags,
                tags_normalized = @tags_normalized,
                content = @content,
                IsDeleted = 0, -- Restore if it was soft-deleted
                updated_at = GETUTCDATE()
            WHERE canonical_url = @canonical_url AND collection = @collection;
            
            SELECT id as inserted_id FROM articles WHERE canonical_url = @canonical_url AND collection = @collection;
        END
        ELSE
        BEGIN
            -- Insert new article
            INSERT INTO articles (
                title, description, author, canonical_url, feed_name, feed_url, 
                date, permalink, categories, tags, tags_normalized, collection, content
            )
            VALUES (
                @title, @description, @author, @canonical_url, @feed_name, @feed_url,
                @date, @permalink, @categories, @tags, @tags_normalized, @collection, @content
            );
            
            SELECT SCOPE_IDENTITY() as inserted_id;
        END
    END
    ');
    PRINT 'Stored procedure sp_InsertArticle created successfully';
END

-- Create stored procedure for searching articles
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_SearchArticles')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_SearchArticles
        @search_term NVARCHAR(500) = NULL,
        @category NVARCHAR(100) = NULL,
        @tag NVARCHAR(100) = NULL,
        @author NVARCHAR(200) = NULL,
        @collection NVARCHAR(100) = NULL,
        @start_date DATETIME2 = NULL,
        @end_date DATETIME2 = NULL,
        @page_size INT = 50,
        @page_number INT = 1
    AS
    BEGIN
        SET NOCOUNT ON;
        
        DECLARE @offset INT = (@page_number - 1) * @page_size;
        
        SELECT 
            id,
            title,
            description,
            author,
            date,
            canonical_url,
            collection,
            categories,
            tags,
            created_at
        FROM articles
        WHERE 
            (@search_term IS NULL OR 
             title LIKE ''%'' + @search_term + ''%'' OR 
             description LIKE ''%'' + @search_term + ''%'')
        AND (@category IS NULL OR categories LIKE ''%'' + @category + ''%'')
        AND (@tag IS NULL OR tags LIKE ''%'' + @tag + ''%'')
        AND (@author IS NULL OR author LIKE ''%'' + @author + ''%'')
        AND (@collection IS NULL OR collection = @collection)
        AND (@start_date IS NULL OR date >= @start_date)
        AND (@end_date IS NULL OR date <= @end_date)
        ORDER BY date DESC
        OFFSET @offset ROWS
        FETCH NEXT @page_size ROWS ONLY;
        
        -- Return total count
        SELECT COUNT(*) as total_count
        FROM articles
        WHERE 
            (@search_term IS NULL OR 
             title LIKE ''%'' + @search_term + ''%'' OR 
             description LIKE ''%'' + @search_term + ''%'')
        AND (@category IS NULL OR categories LIKE ''%'' + @category + ''%'')
        AND (@tag IS NULL OR tags LIKE ''%'' + @tag + ''%'')
        AND (@author IS NULL OR author LIKE ''%'' + @author + ''%'')
        AND (@collection IS NULL OR collection = @collection)
        AND (@start_date IS NULL OR date >= @start_date)
        AND (@end_date IS NULL OR date <= @end_date);
    END
    ');
    PRINT 'Stored procedure sp_SearchArticles created successfully';
END

-- Create stored procedure for soft deleting articles (Azure Search compatibility)
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_SoftDeleteArticle')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_SoftDeleteArticle
        @id BIGINT = NULL,
        @canonical_url NVARCHAR(1000) = NULL,
        @collection NVARCHAR(100) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        IF @id IS NOT NULL
        BEGIN
            UPDATE articles 
            SET IsDeleted = 1, updated_at = GETUTCDATE()
            WHERE id = @id;
        END
        ELSE IF @canonical_url IS NOT NULL AND @collection IS NOT NULL
        BEGIN
            UPDATE articles 
            SET IsDeleted = 1, updated_at = GETUTCDATE()
            WHERE canonical_url = @canonical_url AND collection = @collection;
        END
        ELSE
        BEGIN
            RAISERROR(''Either @id or both @canonical_url and @collection must be provided'', 16, 1);
            RETURN;
        END
        
        SELECT @@ROWCOUNT as rows_affected;
    END
    ');
    PRINT 'Stored procedure sp_SoftDeleteArticle created successfully';
END

-- Update the search stored procedure to filter out soft-deleted items
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_SearchArticles')
BEGIN
    DROP PROCEDURE sp_SearchArticles;
    EXEC('
    CREATE PROCEDURE sp_SearchArticles
        @search_term NVARCHAR(500) = NULL,
        @category NVARCHAR(100) = NULL,
        @tag NVARCHAR(100) = NULL,
        @author NVARCHAR(200) = NULL,
        @collection NVARCHAR(100) = NULL,
        @start_date DATETIME2 = NULL,
        @end_date DATETIME2 = NULL,
        @page_size INT = 20,
        @page_number INT = 1
    AS
    BEGIN
        SET NOCOUNT ON;
        
        DECLARE @offset INT = (@page_number - 1) * @page_size;
        
        SELECT 
            id,
            title,
            description,
            author,
            date,
            canonical_url,
            collection,
            categories,
            tags,
            created_at
        FROM articles
        WHERE 
            IsDeleted = 0 -- Exclude soft-deleted items
        AND (@search_term IS NULL OR 
             title LIKE ''%'' + @search_term + ''%'' OR 
             description LIKE ''%'' + @search_term + ''%'')
        AND (@category IS NULL OR categories LIKE ''%'' + @category + ''%'')
        AND (@tag IS NULL OR tags LIKE ''%'' + @tag + ''%'')
        AND (@author IS NULL OR author LIKE ''%'' + @author + ''%'')
        AND (@collection IS NULL OR collection = @collection)
        AND (@start_date IS NULL OR date >= @start_date)
        AND (@end_date IS NULL OR date <= @end_date)
        ORDER BY date DESC
        OFFSET @offset ROWS
        FETCH NEXT @page_size ROWS ONLY;
        
        -- Return total count (excluding soft-deleted)
        SELECT COUNT(*) as total_count
        FROM articles
        WHERE 
            IsDeleted = 0 -- Exclude soft-deleted items
        AND (@search_term IS NULL OR 
             title LIKE ''%'' + @search_term + ''%'' OR 
             description LIKE ''%'' + @search_term + ''%'')
        AND (@category IS NULL OR categories LIKE ''%'' + @category + ''%'')
        AND (@tag IS NULL OR tags LIKE ''%'' + @tag + ''%'')
        AND (@author IS NULL OR author LIKE ''%'' + @author + ''%'')
        AND (@collection IS NULL OR collection = @collection)
        AND (@start_date IS NULL OR date >= @start_date)
        AND (@end_date IS NULL OR date <= @end_date);
    END
    ');
    PRINT 'Stored procedure sp_SearchArticles updated to exclude soft-deleted items';
END

PRINT 'Articles database schema setup completed successfully!';
PRINT 'Available objects:';
PRINT '  - Table: articles (with indexes on date, collection, tags, categories, IsDeleted)';
PRINT '  - View: vw_articles_parsed (with derived viewing_mode, excludes soft-deleted)';
PRINT '  - Stored Procedure: sp_InsertArticle';
PRINT '  - Stored Procedure: sp_SearchArticles (excludes soft-deleted items)';
PRINT '  - Stored Procedure: sp_SoftDeleteArticle';

-- Azure Search Integration Notes:
PRINT '';
PRINT 'AZURE SEARCH INTEGRATION SETUP:';
PRINT '1. The table includes a ROWVERSION column (rowversion_timestamp) for change tracking';
PRINT '2. The primary key is NONCLUSTERED as required by Azure Search';
PRINT '3. Soft delete support via IsDeleted column is implemented';
PRINT '4. To enable change tracking on the database, run:';
PRINT '   ALTER DATABASE [YourDatabaseName] SET CHANGE_TRACKING = ON (CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON)';
PRINT '5. To enable change tracking on the table, run:';
PRINT '   ALTER TABLE articles ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)';
PRINT '6. For Azure Search indexer data source configuration:';
PRINT '   - Use SqlIntegratedChangeTrackingPolicy for optimal performance';
PRINT '   - Or use HighWaterMarkChangeDetectionPolicy with highWaterMarkColumnName: "rowversion_timestamp"';
PRINT '   - Enable convertHighWaterMarkToRowVersion: true in indexer parameters for better performance';
PRINT '7. Configure soft delete policy in Azure Search data source with:';
PRINT '   - softDeleteColumnName: "IsDeleted"';
PRINT '   - softDeleteMarkerValue: "1"';
PRINT '8. Recommended fields for Azure Search index:';
PRINT '   - Key field: id (required)';
PRINT '   - Searchable: title, description, content, author';
PRINT '   - Filterable: collection, IsDeleted, date';
PRINT '   - Facetable: collection, author, primary_category (from view)';
PRINT '   - Sortable: date, title';
