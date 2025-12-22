class FeedItem {
    [string]$Title
    [string]$Link
    [datetime]$PubDate
    [string]$Description
    [string]$Author
    [string[]]$Tags
    [string]$OutputDir

    FeedItem([string]$title, [string]$link, [datetime]$pubDate, [string]$description, [string]$author, [string[]]$tags, [string]$outputDir) {
        $this.Title = $title
        $this.Link = $link
        $this.PubDate = $pubDate
        $this.Description = $description
        $this.Author = $author
        $this.Tags = $tags
        $this.OutputDir = $outputDir
    }
}

class Feed {
    [string]$FeedName
    [string]$OutputDir
    [string]$URL
    [string]$FeedLevelAuthor
    [FeedItem[]]$Items

    Feed([string]$feedName, [string]$outputDir, [string]$url) {
        $this.FeedName = $feedName
        $this.OutputDir = $outputDir
        $this.URL = $url

        $this.LoadXmlFromUrl()
    }

    Feed([string]$dataFile) {
        $jsonFeed = Get-Content $dataFile | ConvertFrom-Json

        $this.Items = $jsonFeed.Items | ForEach-Object {
            [FeedItem]::new(
                $_.Title,
                $_.Link,
                $_.PubDate,
                $_.Description,
                $_.Author,
                $_.Tags,
                $_.OutputDir
            )
        }
        $this.FeedName = $jsonFeed.FeedName
        $this.OutputDir = $jsonFeed.OutputDir
        $this.URL = $jsonFeed.URL
        $this.FeedLevelAuthor = $jsonFeed.FeedLevelAuthor
    }

    LoadXmlFromUrl() {
        $xmlContent = Get-ContentFromUrl -Url $this.URL

        $xmlDoc = New-Object System.Xml.XmlDocument
        $xmlDoc.LoadXml($xmlContent)

        $this.SetFeedLevelAuthor($xmlDoc)

        $this.Items = @()
        
        $rawItems = @($this.GetRawItems($xmlDoc))
        foreach ($rawItem in $rawItems) {
            $titleNode = $this.GetElementByName($rawItem, 'title')
            if (-not $titleNode) {
                throw "No title found in feed item."
            }
            $title = $this.GetXmlElementValue($titleNode)
            if (-not $title -or [string]::IsNullOrWhiteSpace($title)) { 
                Write-Host "Skipping item: No title found" -ForegroundColor Yellow
                continue 
            }
            
            # Check if title has meaningful content (alphanumeric characters)
            $alphanumericContent = $title -replace '[^a-zA-Z0-9]', ''
            if ([string]::IsNullOrEmpty($alphanumericContent)) {
                Write-Host "Skipping item: Title contains no meaningful content: '$title'" -ForegroundColor Yellow
                continue
            }

            $link = $null
            $linkNode = $this.GetElementByName($rawItem, 'link')
            if ($linkNode) {
                $link = $this.GetXmlElementValue($linkNode, 'href')
                if (-not $link) {
                    $link = $this.GetXmlElementValue($linkNode)
                }
            }
            if (-not $link) {
                $link = $this.GetXmlElementByName($rawItem, 'url')
            }
            
            # YouTube-specific: Look for alternate link with rel="alternate"
            if (-not $link) {
                foreach ($childNode in $rawItem.ChildNodes) {
                    if ($childNode.LocalName -eq 'link' -and $childNode.GetAttribute('rel') -eq 'alternate') {
                        $link = $this.GetXmlElementValue($childNode, 'href')
                        if ($link) {
                            break
                        }
                    }
                }
            }
            
            if (-not $link) {
                Write-Host "Skipping item '$title': No link found" -ForegroundColor Yellow
                continue
            }

            $pubDateRaw = $null
            $dateFields = @('pubDate', 'published', 'updated', 'date')

            foreach ($field in $dateFields) {
                $dateNode = $this.GetElementByName($rawItem, $field)

                if ($dateNode) {
                    $pubDateRaw = $this.GetXmlElementValue($dateNode)
                    break
                }
            }

            if (-not $pubDateRaw) {
                Write-Host "Skipping item '$title': No publication date found" -ForegroundColor Yellow
                continue
            }

            try {
                # Try lenient parsing first - ignore day of week validation
                try {
                    $pubDate = [DateTime]::Parse($pubDateRaw, [System.Globalization.CultureInfo]::InvariantCulture, [System.Globalization.DateTimeStyles]::AllowWhiteSpaces)
                }
                catch {
                    # Fallback: try removing day of week if present
                    $cleanedDate = $pubDateRaw -replace '^[A-Za-z]+,\s*', ''
                    $pubDate = [DateTime]::Parse($cleanedDate, [System.Globalization.CultureInfo]::InvariantCulture, [System.Globalization.DateTimeStyles]::AllowWhiteSpaces)
                }
            }
            catch {
                Write-Host "Skipping item '$title': Invalid publication date format '$pubDateRaw' - $($_.Exception.Message)" -ForegroundColor Yellow
                continue
            }

            $description = $null
            $descFields = @('description', 'summary', 'content', 'encoded')

            foreach ($field in $descFields) {
                $descNode = $this.GetElementByName($rawItem, $field)

                if ($descNode) {
                    $description = $this.GetXmlElementValue($descNode)
                    break
                }
            }

            # YouTube-specific: Try media:description if standard fields don't work
            if (-not $description) {
                foreach ($childNode in $rawItem.ChildNodes) {
                    if ($childNode.LocalName -eq 'group' -and $childNode.NamespaceURI -eq 'http://search.yahoo.com/mrss/') {
                        foreach ($mediaChild in $childNode.ChildNodes) {
                            if ($mediaChild.LocalName -eq 'description' -and $mediaChild.NamespaceURI -eq 'http://search.yahoo.com/mrss/') {
                                $description = $this.GetXmlElementValue($mediaChild)
                                break
                            }
                        }
                        if ($description) {
                            break
                        }
                    }
                }
            }

            if ($description) {
                # Try to clean it up already
                $markdownDescription = Get-MarkdownFromHtml -HtmlContent $description
                if ($null -ne $markdownDescription) {
                    $description = $markdownDescription
                }

                # Remove reddit garbage, everything after "\n\nsubmitted by" and "<br><br> submitted by"
                $description = $description -replace '\n+submitted by.*$', ''
                $description = $description -replace '<br>+\s*submitted by.*$', ''
                $description = $description -replace '\| submitted by.*$', ''
                $description = $description -replace 'submitted by.*$', ''
                $description = $description.Trim()
            } else {
                $description = "";
            }

            $author = $null
            $authorFields = @('creator', 'author')

            foreach ($field in $authorFields) {
                $authorNode = $this.GetElementByName($rawItem, $field)

                if ($authorNode) {
                    $nameNode = $this.GetElementByName($authorNode, 'name')

                    if ($nameNode) {
                        $author = $this.GetXmlElementValue($nameNode)
                    }
                    else {
                        $author = $this.GetXmlElementValue($authorNode)
                    }
                    break
                }
            }

            if (-not $author) {
                foreach ($childNode in $rawItem.ChildNodes) {
                    if ($childNode.LocalName -eq 'creator' -and $childNode.NamespaceURI -eq 'http://purl.org/dc/elements/1.1/') {
                        $author = $this.GetXmlElementValue($childNode)
                        break
                    }
                }
            }

            if (-not $author) {
                foreach ($childNode in $rawItem.ChildNodes) {
                    if ($childNode.LocalName -eq 'author') {
                        foreach ($authorChild in $childNode.ChildNodes) {
                            if ($authorChild.LocalName -eq 'name') {
                                $author = $this.GetXmlElementValue($authorChild)
                                break
                            }
                        }
                        if ($author) {
                            break
                        }
                    }
                }
            }

            if (-not $author) {
                $author = $this.FeedLevelAuthor
            }

            if (-not $author) {
                $author = $this.FeedName
            }

            if (-not $author) {
                $author = "Unknown"
            }

            #if author starts with /u/ remove it
            if ($author -and $author.StartsWith('/u/')) {
                $author = $author.Substring(3)
            }

            $tags = @()

            foreach ($childNode in $rawItem.ChildNodes) {
                if ($childNode.LocalName -eq 'category') {
                    $catValue = $this.GetXmlElementValue($childNode, 'term')

                    if (-not $catValue) {
                        $catValue = $this.GetXmlElementValue($childNode)
                    }

                    if ($catValue) {
                        $tags += $catValue.Trim()
                    }
                }
            }

            # YouTube-specific
            if ($this.IsYouTubeFeed()) {
                $youtubeTags = @()

                if ($link) {
                    $youtubeVideoId = Get-YouTubeVideoId -Url $link
                    if ($youtubeVideoId) {
                        $youtubeTags = $this.GetYouTubeTags($youtubeVideoId)
                    }
                }

                # Some Youtube channels appear to provide a HUGE list of the same tags with every video..
                if (-not $youtubeTags -or $youtubeTags.Count -gt 15) {
                    $youtubeTags = @()
                    
                    if ($description -and $description.Contains('#')) {
                        $hashtagPattern = '#(\w+)'
                        $matches = [regex]::Matches($description, $hashtagPattern)

                        foreach ($match in $matches) {
                            $hashtag = $match.Groups[1].Value
                            # Skip HTML color codes (6 hex characters) and other short codes
                            if ($hashtag -and $hashtag.Length -gt 1 -and -not ($hashtag -match '^[A-Fa-f0-9]{6}$') -and -not ($hashtag -match '^[A-Fa-f0-9]{3}$')) {
                                $youtubeTags += $hashtag
                            }
                        }
                    }
                }

                $tags += $youtubeTags
            }

            if ($tags) {
                $expandedTags = @()
                foreach ($tag in $tags) {
                    if ($tag -match '[;,]') {
                        # Split on both semicolons and commas, trim whitespace, and filter out empty entries
                        $splitTags = $tag -split '[;,]' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' }
                        $expandedTags += $splitTags
                    }
                    else {
                        $expandedTags += $tag
                    }
                }
                $tags = $expandedTags | Sort-Object -Unique
            }

            $item = [FeedItem]::new($title, $link, $pubDate, $description, $author, $tags, $this.OutputDir)
            $this.Items += $item
        }
    }

    SetFeedLevelAuthor([System.Xml.XmlDocument]$xmlDoc) {
        $this.FeedLevelAuthor = $null
        $channelNode = $xmlDoc.SelectSingleNode('//channel')

        if ($channelNode) {
            $managingEditor = $this.GetElementByName($channelNode, 'managingEditor')
            if ($managingEditor) {
                $this.FeedLevelAuthor = $this.GetXmlElementValue($managingEditor)
            }
            if (-not $this.FeedLevelAuthor) {
                $webMaster = $this.GetElementByName($channelNode, 'webMaster')
                if ($webMaster) {
                    $this.FeedLevelAuthor = $this.GetXmlElementValue($webMaster)
                }
            }
            # If no explicit author found, use channel title
            if (-not $this.FeedLevelAuthor) {
                $titleNode = $this.GetElementByName($channelNode, 'title')
                if ($titleNode) {
                    $this.FeedLevelAuthor = $this.GetXmlElementValue($titleNode)
                }
            }
        }

        if (-not $this.FeedLevelAuthor) {
            $feedNode = $xmlDoc.SelectSingleNode('//feed')
            if (-not $feedNode) {
                $feedNode = $xmlDoc.SelectSingleNode('//*[local-name()="feed"]')
            }
            if ($feedNode -and $feedNode.ChildNodes) {
                foreach ($childNode in $feedNode.ChildNodes) {
                    if ($childNode.LocalName -eq 'author') {
                        foreach ($authorChild in $childNode.ChildNodes) {
                            if ($authorChild.LocalName -eq 'name') {
                                $this.FeedLevelAuthor = $this.GetXmlElementValue($authorChild)
                                break
                            }
                        }
                        if ($this.FeedLevelAuthor) {
                            break
                        }
                    }
                }
                # If no explicit author found, use feed title
                if (-not $this.FeedLevelAuthor) {
                    foreach ($childNode in $feedNode.ChildNodes) {
                        if ($childNode.LocalName -eq 'title') {
                            $this.FeedLevelAuthor = $this.GetXmlElementValue($childNode)
                            break
                        }
                    }
                }
            }
        }
    }

    [string] GetXmlElementValue([System.Xml.XmlNode]$Element) {
        return $this.GetXmlElementValue($Element, $null)
    }

    [string] GetXmlElementValue([System.Xml.XmlNode]$Element, [string]$AttributeName = $null) {
        if ($null -eq $Element) { return $null }
        if ($AttributeName) { return $Element.GetAttribute($AttributeName) }
        $value = $Element.InnerText
        if ($value) {
            $value = [System.Web.HttpUtility]::HtmlDecode($value)
            $value = $value.Trim()
        }
        return $value
    }

    [System.Xml.XmlNode] GetElementByName([System.Xml.XmlNode]$ParentNode, [string]$ElementName) {
        $element = $ParentNode.SelectSingleNode($ElementName)
        if ($element) { return $element }
        foreach ($child in $ParentNode.ChildNodes) {
            if ($child.LocalName -eq $ElementName) { return $child }
        }
        return $null
    }

    [System.Collections.IEnumerable] GetRawItems([System.Xml.XmlDocument]$XmlDoc) {
        $rawItems = @()

        $rootNamespace = $XmlDoc.DocumentElement.NamespaceURI
        if ($rootNamespace -eq "http://www.w3.org/2005/Atom") {
            $nsmgr = New-Object System.Xml.XmlNamespaceManager($XmlDoc.NameTable)
            $nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom")
            $rawItems = $XmlDoc.SelectNodes('//atom:entry', $nsmgr)
            
            # If no items found with namespace, try without namespace (YouTube compatibility)
            if ($rawItems.Count -eq 0) {
                $rawItems = $XmlDoc.SelectNodes('//entry')
            }
        }
        elseif ($rootNamespace -and $rootNamespace -ne "") {
            $nsmgr = New-Object System.Xml.XmlNamespaceManager($XmlDoc.NameTable)
            $nsmgr.AddNamespace("ns", $rootNamespace)
            $rawItems = $XmlDoc.SelectNodes('//ns:item', $nsmgr)
            if ($rawItems.Count -eq 0) {
                $rawItems = $XmlDoc.SelectNodes('//ns:entry', $nsmgr)
            }
        }
        else {
            $rssItems = $XmlDoc.SelectNodes('//rss/channel/item')
            if ($rssItems.Count -gt 0) { 
                $rawItems = $rssItems 
            }
            elseif ($XmlDoc.SelectNodes('//channel/item').Count -gt 0) {
                $rawItems = $XmlDoc.SelectNodes('//channel/item')
            }
            elseif ($XmlDoc.SelectNodes('//feed/entry').Count -gt 0) {
                $rawItems = $XmlDoc.SelectNodes('//feed/entry')
            }
            elseif ($XmlDoc.SelectNodes('//item').Count -gt 0) {
                $rawItems = $XmlDoc.SelectNodes('//item')
            }
            elseif ($XmlDoc.SelectNodes('//entry').Count -gt 0) {
                $rawItems = $XmlDoc.SelectNodes('//entry')
            }
        }
        return $rawItems
    }

    [bool] IsYouTubeFeed() {
        return $this.URL -match "youtube\.com"
    }

    [string[]] GetYouTubeTags([string]$videoId) {
        if (-not $videoId) { return @() }
        
        try {
            $apiUrl = "https://ytapi.apps.mattw.io/v3/videos?part=snippet&id=$videoId"
            $response = Invoke-RestMethod -Uri $apiUrl -Method Get -TimeoutSec 10
            
            if ($response.items -and $response.items.Count -gt 0) {
                $snippet = $response.items[0].snippet
                if ($snippet.PSObject.Properties.Name -ccontains "tags") {
                    return $snippet.tags
                }
            }
        }
        catch {
            Write-Warning "Failed to fetch YouTube tags for video ID $videoId`: $($_.Exception.Message)"
        }
        
        return @()
    }
}
