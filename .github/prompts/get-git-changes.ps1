#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Comprehensive git changes analysis for workflow automation

.DESCRIPTION
    Analyzes local git working directory and staging area changes, as well as performs
    network operations to check remote branch status, fetch remote origin URLs, and compare
    local vs remote SHA hashes. Saves structured data to JSON file and creates individual
    .diff files for each changed file.

.PARAMETER OutputDir
    Directory to save the git changes output (default: .tmp/git-changes-analysis)

.PARAMETER CompareWithMain
    Switch to intelligently compare current branch changes against remote main branch.
    Automatically determines the comparison strategy:
    - If current branch exists on remote: compares remote branch vs remote main (fetches both)
    - If current branch is local-only: compares local branch vs remote main (fetches remote main)
    - If on main branch: analyzes only uncommitted changes
    This provides comprehensive analysis of all changes relative to remote main regardless of branch state.
#>

param(
    [string]$OutputDir = ".tmp/git-changes-analysis",
    [switch]$CompareWithMain
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Exit code constants
$EXIT_CODE_SUCCESS = 0
$EXIT_CODE_GIT_FAILURE = 2

try {
    # Remove old analysis directory if it exists
    if (Test-Path $OutputDir) {
        Remove-Item $OutputDir -Recurse -Force | Out-Null
    }

    Write-Host "Analyzing git changes..." -ForegroundColor Cyan
    
    # Critical validation: Check if git repository is in a valid state
    Write-Host "🔍 Validating git repository state..." -ForegroundColor Cyan
    
    # Check if we're in a git repository
    $gitDir = git rev-parse --git-dir 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $gitDir) {
        throw "Not in a git repository or git directory is corrupted"
    }
    
    # Check for git index lock (indicates interrupted git operation)
    $gitRootDir = git rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -eq 0 -and $gitRootDir) {
        $indexLock = Join-Path $gitRootDir ".git/index.lock"
        if (Test-Path $indexLock) {
            throw "Git index is locked (.git/index.lock exists). Another git operation may be in progress or was interrupted."
        }
    }
    
    # Check for merge conflicts
    $mergeHead = Join-Path $gitDir "MERGE_HEAD"
    if (Test-Path $mergeHead) {
        throw "Repository is in merge state. Please complete or abort the merge before continuing."
    }
    
    # Check for rebase in progress
    $rebaseApply = Join-Path $gitDir "rebase-apply"
    $rebaseMerge = Join-Path $gitDir "rebase-merge"
    if ((Test-Path $rebaseApply) -or (Test-Path $rebaseMerge)) {
        throw "Repository is in rebase state. Please complete or abort the rebase before continuing."
    }
    
    Write-Host "✅ Git repository state is valid" -ForegroundColor Green
   
   
    # Create fresh output directory
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
    $OutputPath = Join-Path $OutputDir "git-changes-analysis.json"
    
    # Get current branch
    $currentBranch = git branch --show-current 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $currentBranch) {
        throw "Not in a git repository or unable to determine current branch (git exit code: $LASTEXITCODE)"
    }
    
    # Check remote branch status using git ls-remote (minimal network operation)
    $remoteBranch = @{
        exists                = $false
        hasUpdates            = $false
        localAhead            = $false
        diverged              = $false
        localBasedOnNewerMain = $false
        localSha              = $null
        remoteSha             = $null
        origin                = $null
        branchName            = $currentBranch
    }
    
    $remoteMain = @{
        exists     = $true  # main always exists in remote for this repo
        hasUpdates = $false
        localSha   = $null
        remoteSha  = $null
        origin     = $null
        branchName = "main"
    }
    
    try {
        # Get the remote origin URL
        $remoteOrigin = git remote get-url origin 2>$null
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Could not get remote origin URL (git exit code: $LASTEXITCODE)"
            $remoteOrigin = $null
        }
        
        if ($remoteOrigin) {
            $remoteBranch.origin = $remoteOrigin
            $remoteMain.origin = $remoteOrigin
            
            # Get local commit SHA for current branch
            $localSha = git rev-parse HEAD 2>$null
            if ($LASTEXITCODE -ne 0 -or -not $localSha) {
                throw "Could not get local commit SHA (git exit code: $LASTEXITCODE)"
            }
            $remoteBranch.localSha = $localSha
            
            # Check remote branch using ls-remote (doesn't pull anything)
            $lsRemoteOutput = git ls-remote origin "refs/heads/$currentBranch" 2>$null
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Could not check remote branch status via ls-remote (git exit code: $LASTEXITCODE)"
            }
            elseif ($lsRemoteOutput -and $lsRemoteOutput.Trim()) {
                # Remote branch exists
                $remoteBranch.exists = $true
                $remoteSha = ($lsRemoteOutput -split '\s+')[0]
                $remoteBranch.remoteSha = $remoteSha
                
                # Determine the relationship between local and remote branches
                if ($remoteSha -ne $localSha) {
                    # Check if remote is ancestor of local (local is ahead) or vice versa
                    $isRemoteAncestor = git merge-base --is-ancestor $remoteSha $localSha 2>$null
                    $remoteIsAncestor = ($LASTEXITCODE -eq 0)
                    
                    if ($remoteIsAncestor) {
                        # Remote is ancestor of local - local is ahead, no updates needed
                        $remoteBranch.hasUpdates = $false
                        $remoteBranch.localAhead = $true
                    }
                    else {
                        # Check if local is ancestor of remote (remote is ahead)
                        $isLocalAncestor = git merge-base --is-ancestor $localSha $remoteSha 2>$null
                        $localIsAncestor = ($LASTEXITCODE -eq 0)
                        
                        if ($localIsAncestor) {
                            # Local is ancestor of remote - remote has updates we need
                            $remoteBranch.hasUpdates = $true
                            $remoteBranch.localAhead = $false
                        }
                        else {
                            # Branches have diverged - check if this might be due to a rebase
                            $remoteBranch.hasUpdates = $true
                            $remoteBranch.diverged = $true
                            $remoteBranch.localAhead = $false
                            
                            # Additional analysis: check if local branch is based on more recent main
                            $localMergeBaseWithMain = git merge-base HEAD origin/main 2>$null
                            $remoteMergeBaseWithMain = git merge-base $remoteSha origin/main 2>$null
                            
                            if ($LASTEXITCODE -eq 0 -and $localMergeBaseWithMain -and $remoteMergeBaseWithMain) {
                                # Compare how recent each branch's base is relative to main
                                $localBaseIsNewer = git merge-base --is-ancestor $remoteMergeBaseWithMain $localMergeBaseWithMain 2>$null
                                if ($LASTEXITCODE -eq 0) {
                                    $remoteBranch.localBasedOnNewerMain = $true
                                }
                            }
                        }
                    }
                }
                else {
                    # SHAs are identical - branches are in sync
                    $remoteBranch.hasUpdates = $false
                    $remoteBranch.localAhead = $false
                }
            }
            
            # Check remote main branch status and fetch for accurate comparison
            $lsRemoteMainOutput = git ls-remote origin "refs/heads/main" 2>$null
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Could not check remote main branch status via ls-remote (git exit code: $LASTEXITCODE)"
            }
            elseif ($lsRemoteMainOutput -and $lsRemoteMainOutput.Trim()) {
                $remoteMainSha = ($lsRemoteMainOutput -split '\s+')[0]
                $remoteMain.remoteSha = $remoteMainSha
                
                # Get our local main branch SHA
                $localMainSha = git rev-parse main 2>$null
                if ($LASTEXITCODE -eq 0 -and $localMainSha) {
                    $remoteMain.localSha = $localMainSha
                }
                
                # Fetch main to ensure we have up-to-date remote reference for accurate comparison
                Write-Host "🔄 Fetching remote main for accurate branch status analysis..." -ForegroundColor Cyan
                git fetch origin main --quiet 2>$null
                if ($LASTEXITCODE -ne 0) {
                    Write-Warning "Failed to fetch main branch from remote for comparison (git exit code: $LASTEXITCODE)"
                }
                
                # Check if current branch needs to rebase against main
                # Find the merge-base between current branch and remote main
                $mergeBase = git merge-base HEAD origin/main 2>$null
                if ($LASTEXITCODE -eq 0 -and $mergeBase) {
                    # Check if remote main has commits beyond the merge-base
                    $commitsAhead = git rev-list --count "$mergeBase..origin/main" 2>$null
                    if ($LASTEXITCODE -eq 0 -and [int]$commitsAhead -gt 0) {
                        $remoteMain.hasUpdates = $true
                    }
                }
                else {
                    # If we can't find merge-base or are on main, check direct comparison
                    if ($currentBranch -eq "main") {
                        # On main branch: check if remote main is ahead of local main
                        if ($remoteMainSha -ne $localMainSha) {
                            $remoteMain.hasUpdates = $true
                        }
                    }
                    else {
                        # Can't determine merge-base, assume we need updates
                        $remoteMain.hasUpdates = $true
                    }
                }
            }
        }
    }
    catch {
        Write-Warning "Could not check remote branch status: $($_.Exception.Message)"
    }
    
    $compareLocalWithMain = $false
    $compareRemoteWithMain = $false
    
    if($compareWithMain) {
        $compareLocalWithMain = $true
        $compareRemoteWithMain = $false

        if($remoteBranch.exists) {
            $compareLocalWithMain = $false
            $compareRemoteWithMain = $true
        }
    }

    # Determine analysis mode
    if ($compareRemoteWithMain) {
        Write-Host "🌐 Comparing current remote branch with main..." -ForegroundColor Magenta
        
        # Check network connectivity before attempting remote operations
        if (-not $remoteBranch.origin) {
            throw "No remote origin configured - cannot perform remote comparison"
        }
        
        # Test network connectivity to remote
        try {
            Write-Host "🔌 Testing network connectivity to remote..." -ForegroundColor Cyan
            git ls-remote --exit-code origin HEAD 2>$null | Out-Null
            if ($LASTEXITCODE -ne 0) {
                throw "Cannot connect to remote origin. Check network connectivity and credentials."
            }
            Write-Host "✅ Network connectivity to remote confirmed" -ForegroundColor Green
        }
        catch {
            throw "Network connectivity test failed: $($_.Exception.Message)"
        }
        
        # For main comparison, we need to compare current remote branch vs remote main
        if (-not $remoteBranch.exists) {
            Write-Host "⚠️  Current branch has no remote - cannot compare with main" -ForegroundColor Yellow
            $statusLines = @()  # No changes to analyze
        }
        elseif ($currentBranch -eq "main") {
            Write-Host "ℹ️  Currently on main branch - no comparison needed" -ForegroundColor Blue
            $statusLines = @()  # No changes to analyze
        }
        else {
            # Get diff between current remote branch and remote main
            Write-Host "📊 Fetching changes between current remote branch and main..." -ForegroundColor Cyan
            
            # We need to fetch to get the remote refs for comparison
            try {
                Write-Host "🔄 Fetching remote branches for comparison..." -ForegroundColor Cyan
                git fetch origin $currentBranch --quiet 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to fetch current branch from remote (git exit code: $LASTEXITCODE)"
                }
                
                git fetch origin main --quiet 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to fetch main branch from remote (git exit code: $LASTEXITCODE)"
                }
                
                $currentRemoteRef = "origin/$currentBranch"
                $mainRemoteRef = "origin/main"
                
                # Verify the remote refs exist after fetch
                git show-ref --verify --quiet "refs/remotes/$currentRemoteRef" 2>$null | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "Remote branch $currentRemoteRef does not exist after fetch"
                }
                
                git show-ref --verify --quiet "refs/remotes/$mainRemoteRef" 2>$null | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "Remote main branch $mainRemoteRef does not exist after fetch"
                }
                
                # Get list of changed files between remote branch and remote main
                $branchToMainChanges = git diff --name-status $mainRemoteRef $currentRemoteRef 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to get diff between $mainRemoteRef and $currentRemoteRef (git exit code: $LASTEXITCODE)"
                }
                $statusLines = $branchToMainChanges
            }
            catch {
                Write-Error "Could not fetch remote branches for comparison: $($_.Exception.Message)"
                throw "Git fetch operation failed - cannot continue safely with remote comparison"
            }
        }
    }
    elseif ($compareLocalWithMain) {
        Write-Host "🔍 Comparing local uncommitted changes against main branch..." -ForegroundColor Magenta
        
        if ($currentBranch -eq "main") {
            Write-Host "ℹ️  Currently on main branch - analyzing uncommitted changes only" -ForegroundColor Blue
            # For main branch, just analyze local uncommitted changes
            $statusLines = git status --porcelain 2>$null
            if ($LASTEXITCODE -ne 0) {
                throw "Failed to get git status (git exit code: $LASTEXITCODE)"
            }
        }
        else {
            Write-Host "📊 Comparing current local branch with remote main..." -ForegroundColor Cyan
            
            # Compare current local branch (including uncommitted changes) against remote main
            try {
                # First ensure we have remote main
                Write-Host "🔄 Fetching remote main for comparison..." -ForegroundColor Cyan
                git fetch origin main --quiet 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to fetch main branch from remote (git exit code: $LASTEXITCODE)"
                }
                
                # Verify remote main exists after fetch
                git show-ref --verify --quiet "refs/remotes/origin/main" 2>$null | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "Remote main branch does not exist after fetch"
                }
                
                # Get changes between current local branch and remote main (committed changes)
                $branchToMainChanges = git diff --name-status origin/main HEAD 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to get diff between origin/main and HEAD (git exit code: $LASTEXITCODE)"
                }
                
                # Then get uncommitted changes 
                $localChanges = git status --porcelain 2>$null
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to get git status for local changes (git exit code: $LASTEXITCODE)"
                }
                
                # Combine both sets of changes for comprehensive analysis
                $allChanges = @()
                if ($branchToMainChanges) {
                    $allChanges += $branchToMainChanges
                }
                if ($localChanges) {
                    # Convert local changes format to diff format for consistency
                    foreach ($localChange in $localChanges) {
                        if ([string]::IsNullOrEmpty($localChange)) { continue }
                        
                        if ($localChange.Length -ge 3) {
                            $status = $localChange.Substring(0, 2)
                            $file = $localChange.Substring(3)
                            
                            # Convert git status codes to git diff codes
                            $diffStatus = switch -Regex ($status) {
                                '^\?\?' { 'A' }  # Untracked -> Added
                                '^A' { 'A' }     # Added -> Added
                                '^.A' { 'A' }    # Added -> Added
                                '^[MD]' { 'M' }  # Modified/Deleted -> Modified
                                '^.[MD]' { 'M' } # Modified/Deleted -> Modified
                                '^D' { 'D' }     # Deleted -> Deleted
                                '^.D' { 'D' }    # Deleted -> Deleted
                                '^R' { 'R' }     # Renamed -> Renamed
                                default { 'M' }  # Default to Modified
                            }
                            
                            $allChanges += "$diffStatus`t$file"
                        }
                    }
                }
                
                $statusLines = $allChanges | Sort-Object -Unique
            }
            catch {
                Write-Error "Could not analyze local branch changes against remote main: $($_.Exception.Message)"
                throw "Git diff operation failed - cannot continue safely with local branch analysis"
            }
        }
    }
    else {
        Write-Host "📁 Analyzing local changes..." -ForegroundColor Cyan
        # Get git status for local changes
        $statusLines = git status --porcelain 2>$null
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to get git status for local changes (git exit code: $LASTEXITCODE)"
        }
    }
    
    # Initialize tracking variables
    $summary = @{
        totalFiles = 0
        new        = 0
        modified   = 0
        deleted    = 0
        renamed    = 0
        hasChanges = $false
    }
    
    $files = @{
        new      = @()
        modified = @()
        deleted  = @()
        renamed  = @()
    }
    
    $fileDetails = @()
    $allChangedFiles = @()
    
    # Parse each status line
    foreach ($line in $statusLines) {
        if ([string]::IsNullOrEmpty($line)) {
            continue
        }
        
        $status = ""
        $file = ""
        
        # Handle different git command output formats
        if ($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") {
            # git diff --name-status format: "M\tfilename" or "A\tfilename"
            if ($line.Contains("`t")) {
                $parts = $line -split "`t", 2
                $status = $parts[0].Trim()
                $file = $parts[1].Trim()
            }
        }
        else {
            # git status --porcelain format: "XY filename"
            if ($line.Length -ge 3) {
                $status = $line.Substring(0, 2)
                $file = $line.Substring(3)
            }
        }
        
        if ([string]::IsNullOrEmpty($file)) {
            continue
        }
        
        $summary.totalFiles++
        $summary.hasChanges = $true
        
        # Determine change type based on git status codes
        $changeType = if (($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") -or 
            ($compareLocalWithMain -and $currentBranch -ne "main")) {
            # Handle git diff --name-status single character codes (for CompareRemoteWithMain or CompareLocalWithMain)
            switch ($status) {
                'A' { 
                    $summary.new++
                    $files.new += $file
                    "New"
                }
                'M' { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified"
                }
                'D' { 
                    $summary.deleted++
                    $files.deleted += $file
                    "Deleted"
                }
                'R' { 
                    $summary.renamed++
                    $files.renamed += $file
                    "Renamed"
                }
                'T' { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified (Type Change)"
                }
                default { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified"
                }
            }
        }
        else {
            # Handle git status --porcelain two character codes
            switch -Regex ($status) {
                '^\?\?' { 
                    $summary.new++
                    $files.new += $file
                    "New"
                }
                '^A' { 
                    $summary.new++
                    $files.new += $file
                    "New"
                }
                '^.A' { 
                    $summary.new++
                    $files.new += $file
                    "New"
                }
                '^[MD]' { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified"
                }
                '^.[MD]' { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified"
                }
                '^D' { 
                    $summary.deleted++
                    $files.deleted += $file
                    "Deleted"
                }
                '^.D' { 
                    $summary.deleted++
                    $files.deleted += $file
                    "Deleted"
                }
                '^R' { 
                    $summary.renamed++
                    $files.renamed += $file
                    "Renamed"
                }
                default { 
                    $summary.modified++
                    $files.modified += $file
                    "Modified"
                }
            }
        }

        $fileDetails += @{
            file       = $file
            status     = $status
            changeType = $changeType
        }
            
        $allChangedFiles += $file
    }

    # Generate diff files for each changed file
    $diffFiles = @()

    foreach ($file in $allChangedFiles) {
        # Create safe filename for diff
        $safeFileName = $file -replace '[/\\:*?"<>|]', '-'
        $diffFileName = "$safeFileName.diff"
        $diffPath = Join-Path $OutputDir $diffFileName
    
        try {
            if (($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") -or 
                ($compareLocalWithMain -and $currentBranch -ne "main")) {
                # For main comparison or local branch comparison, show diff against main
                if ($compareRemoteWithMain) {
                    # For CompareRemoteWithMain, show diff between current remote branch and remote main
                    $currentRemoteRef = "origin/$currentBranch"
                    $mainRemoteRef = "origin/main"
                    $diffContent = git diff $mainRemoteRef $currentRemoteRef -- $file 2>$null
                    if ($LASTEXITCODE -ne 0) {
                        Write-Warning "Failed to generate diff for $file against remote main (git exit code: $LASTEXITCODE)"
                        continue
                    }
                }
                else {
                    # For CompareLocalWithMain, show diff between current state (including uncommitted) and remote main
                    $diffContent = git diff origin/main -- $file 2>$null
                    if ($LASTEXITCODE -ne 0) {
                        Write-Warning "Failed to generate diff for $file against remote main (git exit code: $LASTEXITCODE)"
                        continue
                    }
                }
            }
            else {
                # For local analysis, use existing logic
                # Check if file is tracked
                $isTracked = $null -ne (git ls-files $file 2>$null)
                if ($LASTEXITCODE -ne 0) {
                    Write-Warning "Failed to check if $file is tracked (git exit code: $LASTEXITCODE)"
                    continue
                }
            
                $diffContent = if ($isTracked) {
                    # For tracked files, show diff against HEAD
                    $diffOutput = git diff HEAD -- $file 2>$null
                    if ($LASTEXITCODE -ne 0) {
                        Write-Warning "Failed to generate diff for tracked file $file (git exit code: $LASTEXITCODE)"
                        $null
                    }
                    else {
                        $diffOutput
                    }
                }
                else {
                    # For untracked files, show as entirely new
                    if (Test-Path $file) {
                        $fileContent = Get-Content $file -Raw -ErrorAction SilentlyContinue
                        if ($fileContent) {
                            $lines = $fileContent -split "`n"
                            $header = "--- /dev/null`n+++ b/$file`n@@ -0,0 +1,$($lines.Length) @@"
                            $content = $lines | ForEach-Object { "+$_" }
                            "$header`n$($content -join "`n")"
                        }
                    }
                }
            }
        
            if ($diffContent) {
                $diffContent | Out-File -FilePath $diffPath -Encoding utf8 -Force
                $diffFiles += @{
                    originalFile = $file
                    diffFile     = $diffFileName
                    diffPath     = $diffPath
                }
            }
        }
        catch {
            Write-Error "Failed to generate diff for $file`: $($_.Exception.Message)"
            # Don't throw here as this is per-file processing, but log the error
        }
    }

    # Create consolidated file objects grouped by change type
    $consolidatedFiles = @{
        new      = @()
        modified = @()
        deleted  = @()
        renamed  = @()
    }
    
    foreach ($fileDetail in $fileDetails) {
        $file = $fileDetail.file
        $changeType = $fileDetail.changeType
        $diffInfo = $diffFiles | Where-Object { $_.originalFile -eq $file } | Select-Object -First 1
        
        $fileObject = @{
            file = $file
        }
        
        # Add diff information if available
        if ($diffInfo) {
            $fileObject.diffPath = $diffInfo.diffPath
        }
        
        # Add file to appropriate change type category
        switch ($changeType) {
            "New" { 
                $consolidatedFiles.new += $fileObject
            }
            "Modified" { 
                $consolidatedFiles.modified += $fileObject
            }
            "Modified (Type Change)" { 
                $consolidatedFiles.modified += $fileObject
            }
            "Deleted" { 
                $consolidatedFiles.deleted += $fileObject
            }
            "Renamed" { 
                $consolidatedFiles.renamed += $fileObject
            }
        }
    }

    # Enhanced summary with core statistics
    $enhancedSummary = @{
        totalFiles = $summary.totalFiles
        new        = $summary.new
        modified   = $summary.modified
        deleted    = $summary.deleted
        renamed    = $summary.renamed
        hasChanges = $summary.hasChanges
    }

    # Create consolidated JSON structure with reduced duplication
    $gitData = @{
        timestamp = (Get-Date -Format "yyyy-MM-ddTHH:mm:ssZ")
        analysis  = @{
            type        = if ($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") { 
                "branch-to-main" 
            }
            elseif ($compareLocalWithMain -and $currentBranch -ne "main") { 
                "local-branch-to-main" 
            }
            else { 
                "local" 
            }
            mode        = if ($compareRemoteWithMain) { 
                "branch-main-comparison" 
            }
            elseif ($compareLocalWithMain) { 
                "local-branch-main-comparison" 
            }
            else { 
                "local-changes" 
            }
            description = if ($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") { 
                "Analysis of changes between current remote branch and remote main branch" 
            }
            elseif ($compareLocalWithMain -and $currentBranch -ne "main") {
                "Analysis of local uncommitted changes in context of main branch differences"
            }
            else { 
                "Analysis of local changes and repository status" 
            }
        }
        summary   = $enhancedSummary
        branch    = @{
            current = $currentBranch
            remote  = @{
                exists                = $remoteBranch.exists
                hasUpdates            = $remoteBranch.hasUpdates
                localAhead            = $remoteBranch.localAhead
                diverged              = $remoteBranch.diverged
                localBasedOnNewerMain = $remoteBranch.localBasedOnNewerMain
                localSha              = $remoteBranch.localSha
                remoteSha             = $remoteBranch.remoteSha
                origin                = $remoteBranch.origin
                branchName            = $remoteBranch.branchName
            }
            main    = @{
                exists     = $remoteMain.exists
                hasUpdates = $remoteMain.hasUpdates
                localSha   = $remoteMain.localSha
                remoteSha  = $remoteMain.remoteSha
                origin     = $remoteMain.origin
                branchName = $remoteMain.branchName
            }
        }
        files     = $consolidatedFiles
    }

    # Save to JSON
    $jsonOutput = $gitData | ConvertTo-Json -Depth 10 -Compress:$false
    $jsonOutput | Out-File -FilePath $OutputPath -Encoding UTF8 -Force

    Start-Sleep 1

    # Display summary
    Write-Host "✅ Git analysis saved to: $OutputPath" -ForegroundColor Green

    $analysisType = if ($compareRemoteWithMain -and $remoteBranch.exists -and $currentBranch -ne "main") { 
        "Branch vs Main Changes" 
    }
    elseif ($compareLocalWithMain -and $currentBranch -ne "main") { 
        "Local Branch vs Main Changes" 
    }
    else { 
        "Local Changes" 
    }
    Write-Host "📊 $analysisType Summary:" -ForegroundColor Yellow
    Write-Host "   • Branch: $($gitData.branch.current)" -ForegroundColor Cyan
    Write-Host "   • Analysis mode: $($gitData.analysis.mode)" -ForegroundColor Cyan
    Write-Host "   • Total files changed: $($gitData.summary.totalFiles)" -ForegroundColor Cyan
    Write-Host "   • New: $($gitData.summary.new)" -ForegroundColor Cyan
    Write-Host "   • Modified: $($gitData.summary.modified)" -ForegroundColor Cyan
    Write-Host "   • Deleted: $($gitData.summary.deleted)" -ForegroundColor Cyan
    Write-Host "   • Renamed: $($gitData.summary.renamed)" -ForegroundColor Cyan

    # Display remote branch status
    Write-Host "🌐 Remote Branch Status:" -ForegroundColor Yellow
    if ($gitData.branch.remote.origin) {
        Write-Host "   • Origin: $($gitData.branch.remote.origin)" -ForegroundColor Cyan
        if ($gitData.branch.remote.exists) {
            Write-Host "   • Remote branch exists: ✅" -ForegroundColor Green
            if ($gitData.branch.remote.hasUpdates) {
                if ($gitData.branch.remote.diverged) {
                    if ($gitData.branch.remote.localBasedOnNewerMain) {
                        Write-Host "   • Branch status: ⚠️  DIVERGED - local rebased on newer main" -ForegroundColor Yellow
                        Write-Host "   • Local SHA:  $($gitData.branch.remote.localSha)" -ForegroundColor Cyan
                        Write-Host "   • Remote SHA: $($gitData.branch.remote.remoteSha)" -ForegroundColor Cyan
                        Write-Host "   • Action recommended: Push with --force-with-lease (local is more current)" -ForegroundColor Green
                    }
                    else {
                        Write-Host "   • Branch status: ⚠️  DIVERGED - both local and remote have unique commits" -ForegroundColor Red
                        Write-Host "   • Local SHA:  $($gitData.branch.remote.localSha)" -ForegroundColor Cyan
                        Write-Host "   • Remote SHA: $($gitData.branch.remote.remoteSha)" -ForegroundColor Cyan
                        Write-Host "   • Action needed: Fetch and merge or rebase to resolve divergence" -ForegroundColor Yellow
                    }
                }
                else {
                    Write-Host "   • Remote has updates: ⚠️  YES - remote is ahead" -ForegroundColor Red
                    Write-Host "   • Local SHA:  $($gitData.branch.remote.localSha)" -ForegroundColor Cyan
                    Write-Host "   • Remote SHA: $($gitData.branch.remote.remoteSha)" -ForegroundColor Cyan
                    Write-Host "   • Action needed: Pull or fetch to get latest changes" -ForegroundColor Yellow
                }
            }
            elseif ($gitData.branch.remote.localAhead) {
                Write-Host "   • Local is ahead: ✅ Ready to push" -ForegroundColor Green
                Write-Host "   • Local SHA:  $($gitData.branch.remote.localSha)" -ForegroundColor Cyan
                Write-Host "   • Remote SHA: $($gitData.branch.remote.remoteSha)" -ForegroundColor Cyan
                Write-Host "   • Action available: Push to update remote branch" -ForegroundColor Green
            }
            else {
                Write-Host "   • Remote is in sync: ✅" -ForegroundColor Green
            }
        }
        else {
            Write-Host "   • Remote branch exists: ❌ (new branch)" -ForegroundColor Yellow
        }
    
        # Display remote main branch status
        if ($gitData.branch.main.remoteSha) {
            Write-Host "   • Remote main branch:" -ForegroundColor Cyan
            if ($gitData.branch.main.hasUpdates) {
                Write-Host "     ⚠️  Main has moved ahead - rebase recommended" -ForegroundColor Red
                if ($gitData.branch.main.localSha) {
                    Write-Host "     • Local main SHA:  $($gitData.branch.main.localSha)" -ForegroundColor Cyan
                }
                Write-Host "     • Remote main SHA: $($gitData.branch.main.remoteSha)" -ForegroundColor Cyan
            }
            else {
                Write-Host "     ✅ Main is up to date" -ForegroundColor Green
            }
        }
    }
    else {
        Write-Host "   • No remote origin configured" -ForegroundColor Gray
    }

    # Show change analysis if there are changes
    $hasFiles = ($gitData.files.new.Count -gt 0) -or 
                ($gitData.files.modified.Count -gt 0) -or 
                ($gitData.files.deleted.Count -gt 0) -or 
                ($gitData.files.renamed.Count -gt 0)
    
    if ($hasFiles) {
        # Analyze changes by directory for all file types
        $changesByDirectory = @{}
        
        # Process each change type
        foreach ($changeType in @('new', 'modified', 'deleted', 'renamed')) {
            foreach ($fileObj in $gitData.files.$changeType) {
                $dir = Split-Path $fileObj.file -Parent
                if ([string]::IsNullOrEmpty($dir)) { $dir = "root" }
                if (-not $changesByDirectory.ContainsKey($dir)) {
                    $changesByDirectory[$dir] = @()
                }
                $changesByDirectory[$dir] += @{
                    file = $fileObj.file
                    changeType = $changeType
                }
            }
        }
    
        Write-Host "🔍 Changes by type:" -ForegroundColor Yellow
        if ($gitData.files.new.Count -gt 0) {
            $count = $gitData.files.new.Count
            Write-Host "   • New: $count file$(if ($count -ne 1) { 's' })" -ForegroundColor Cyan
        }
        if ($gitData.files.modified.Count -gt 0) {
            $count = $gitData.files.modified.Count
            Write-Host "   • Modified: $count file$(if ($count -ne 1) { 's' })" -ForegroundColor Cyan
        }
        if ($gitData.files.deleted.Count -gt 0) {
            $count = $gitData.files.deleted.Count
            Write-Host "   • Deleted: $count file$(if ($count -ne 1) { 's' })" -ForegroundColor Cyan
        }
        if ($gitData.files.renamed.Count -gt 0) {
            $count = $gitData.files.renamed.Count
            Write-Host "   • Renamed: $count file$(if ($count -ne 1) { 's' })" -ForegroundColor Cyan
        }
    
        Write-Host "📁 Changes by directory:" -ForegroundColor Yellow
        foreach ($dir in ($changesByDirectory.Keys | Sort-Object)) {
            $count = $changesByDirectory[$dir].Count
            Write-Host "   • $dir`: $count file$(if ($count -ne 1) { 's' })" -ForegroundColor Cyan
        }
    }
    else {
        Write-Host "   • No local changes detected" -ForegroundColor Green
    }

    exit $EXIT_CODE_SUCCESS
}
catch {
    Write-Host "❌ Critical error occurred during git analysis" -ForegroundColor Red
    Write-Host "🚨 This indicates a serious git repository or network issue" -ForegroundColor Red
    Write-Host "" -ForegroundColor Red
    Write-Host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    Write-Host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Stack Trace:" -ForegroundColor Red
    Write-Host $_.Exception.StackTrace -ForegroundColor Red
    Write-Host "Script Stack Trace:" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    Write-Host "" -ForegroundColor Red
    Write-Host "⚠️  DO NOT CONTINUE with pushall or other git operations until this is resolved!" -ForegroundColor Red
    Write-Host "🔧 Recommended actions:" -ForegroundColor Yellow
    Write-Host "   1. Check git repository integrity: git fsck" -ForegroundColor Yellow
    Write-Host "   2. Verify network connectivity if using remote operations" -ForegroundColor Yellow
    Write-Host "   3. Ensure no other git operations are running" -ForegroundColor Yellow
    Write-Host "   4. Check for merge conflicts or incomplete operations" -ForegroundColor Yellow
    
    # Set a specific exit code to indicate git operation failure
    exit $EXIT_CODE_GIT_FAILURE  # Exit code indicates git operation failure
}
