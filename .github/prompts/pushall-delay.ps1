param(
    [Parameter(Mandatory = $true)]
    [string]$Warning,
    
    [Parameter(Mandatory = $false)]
    [string]$Message = "",
    
    [Parameter(Mandatory = $false)]
    [string]$MessageFile,
    
    [Parameter(Mandatory = $false)]
    [int]$Delay = 10
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Add message from file if provided
if ($MessageFile) {
    if (Test-Path $MessageFile) {
        $FileMessage = Get-Content $MessageFile -Raw
        if($Message -ne "") {
            $Message += "`n`n"
        }
        $Message += $FileMessage
    } else {
        Write-Error "Message file '$MessageFile' not found"
        exit 1
    }
}

# Global variable to track abort status
$script:Aborted = $false

# Function to check for specific keypress
function Test-Keypress {
    if ([Console]::KeyAvailable) {
        $key = [Console]::ReadKey($true)
        if ($key.Key -eq 'Spacebar') {
            return 'abort'
        } elseif ($key.Key -eq 'Enter') {
            return 'continue'
        } else {
            return 'other'
        }
    }
    return 'none'
}

try {
    # Clear the terminal
    Clear-Host
    
    # Function to get safe terminal dimensions with fallbacks
    function Get-SafeTerminalSize {
        try {
            $width = [Math]::Max(60, [Console]::WindowWidth - 2)  # Leave margin
            $height = [Math]::Max(10, [Console]::WindowHeight - 2)  # Leave margin
            return @{ Width = $width; Height = $height }
        }
        catch {
            # Fallback to safe defaults if console access fails
            return @{ Width = 80; Height = 24 }
        }
    }
    
    # Function to detect if terminal size changed
    function Test-TerminalSizeChanged {
        param($previousSize)
        try {
            $currentSize = Get-SafeTerminalSize
            return ($currentSize.Width -ne $previousSize.Width -or $currentSize.Height -ne $previousSize.Height)
        }
        catch {
            return $false
        }
    }
    
    # Function to create progress bar with consistent sizing
    function New-ProgressBar {
        param(
            [int]$Progress = 0,
            [switch]$Complete
        )
        
        $currentTerminalSize = Get-SafeTerminalSize
        # Cap progress bar width to reasonable maximum (100 chars) to prevent excessive width on large screens
        $progressBarWidth = [Math]::Min(100, [Math]::Max(20, [Math]::Floor($currentTerminalSize.Width * 0.6)))
        
        if ($Complete) {
            return "█" * $progressBarWidth
        } else {
            $filledLength = [Math]::Floor(($Progress / 100) * $progressBarWidth)
            $emptyLength = $progressBarWidth - $filledLength
            return "█" * $filledLength + "░" * $emptyLength
        }
    }
    
    # Get initial terminal size
    $terminalSize = Get-SafeTerminalSize
    $lastTerminalSize = $terminalSize.Clone()
    
    # Function to calculate layout based on current terminal size
    function Calculate-Layout {
        param($currentTerminalSize, $message)
        
        $messageLines = $message -split "`r?`n"
        
        # Find the maximum line length
        $maxLineLength = 0
        foreach ($line in $messageLines) {
            if ($line.Length -gt $maxLineLength) {
                $maxLineLength = $line.Length
            }
        }
        
        # Use full terminal width with just a small margin for borders
        $minBoxWidth = 60  # Minimum for very small terminals
        $boxWidth = [Math]::Max($minBoxWidth, $currentTerminalSize.Width - 4)  # Use full width minus margin for borders
        
        return @{
            MessageLines = $messageLines
            BoxWidth = $boxWidth
            MaxLineLength = $maxLineLength
        }
    }
    
    # Function to render the full display
    function Render-Display {
        param($layout, $message, $delay)
        
        # Display styled header
        Write-Host ""
        $headerBorder = "═" * $layout.BoxWidth
        $headerText = "⚠️  ACTION REQUIRED  ⚠️"
        $headerPadding = [Math]::Max(0, [Math]::Floor(($layout.BoxWidth - $headerText.Length) / 2))
        $headerLine = (" " * $headerPadding) + $headerText + (" " * ($layout.BoxWidth - $headerPadding - $headerText.Length))
        
        Write-Host $headerBorder -ForegroundColor Cyan
        Write-Host $headerLine -ForegroundColor Yellow
        Write-Host $headerBorder -ForegroundColor Cyan
        Write-Host ""
        
        # Display the countdown message
        Write-Host "You have " -NoNewline -ForegroundColor White
        Write-Host "$delay seconds" -NoNewline -ForegroundColor Red
        Write-Host " to abort this process. $Warning" -ForegroundColor White
        Write-Host ""
        
        if($layout.MessageLines -and $layout.MessageLines.Count -gt 0) {
            # Display the main message in a dynamic box
            # Generate borders
            $topBorder = "┌" + ("─" * ($layout.BoxWidth - 2)) + "┐"
            $bottomBorder = "└" + ("─" * ($layout.BoxWidth - 2)) + "┘"
            
            Write-Host $topBorder -ForegroundColor Green
            
            foreach ($line in $layout.MessageLines) {
                # Handle long lines that exceed box width
                if ($line.Length -gt ($layout.BoxWidth - 4)) {
                    # Word wrap long lines
                    $words = $line -split ' '
                    $currentLine = ''
                    
                    foreach ($word in $words) {
                        $testLine = if ($currentLine) { $currentLine + ' ' + $word } else { $word }
                        if ($testLine.Length -le ($layout.BoxWidth - 4)) {
                            $currentLine = $testLine
                        } else {
                            # Output the current line if it has content
                            if ($currentLine) {
                                $contentLength = $currentLine.Length
                                $padding = ($layout.BoxWidth - 4 - $contentLength)
                                Write-Host "│ " -NoNewline -ForegroundColor Green
                                Write-Host $currentLine -NoNewline -ForegroundColor Yellow
                                Write-Host (" " * $padding) -NoNewline
                                Write-Host " │" -ForegroundColor Green
                            }
                            $currentLine = $word
                        }
                    }
                    
                    # Output the final line
                    if ($currentLine) {
                        $contentLength = $currentLine.Length
                        $padding = ($layout.BoxWidth - 4 - $contentLength)
                        Write-Host "│ " -NoNewline -ForegroundColor Green
                        Write-Host $currentLine -NoNewline -ForegroundColor Yellow
                        Write-Host (" " * $padding) -NoNewline
                        Write-Host " │" -ForegroundColor Green
                    }
                } else {
                    # Normal line that fits in the box - ensure proper padding
                    $contentLength = $line.Length
                    $padding = ($layout.BoxWidth - 4 - $contentLength)
                    Write-Host "│ " -NoNewline -ForegroundColor Green
                    Write-Host $line -NoNewline -ForegroundColor Yellow
                    Write-Host (" " * $padding) -NoNewline
                    Write-Host " │" -ForegroundColor Green
                }
            }
            
            Write-Host $bottomBorder -ForegroundColor Green
            Write-Host ""
        }
        
        # Display abort instructions
        Write-Host "💡 Press " -NoNewline -ForegroundColor Cyan
        Write-Host "SPACE" -NoNewline -ForegroundColor Magenta
        Write-Host " to abort the workflow, or " -NoNewline -ForegroundColor Cyan
        Write-Host "ENTER" -NoNewline -ForegroundColor Green
        Write-Host " to continue immediately" -ForegroundColor Cyan
        Write-Host ""
        Write-Host ""
    }
    
    # Calculate initial layout
    $layout = Calculate-Layout $terminalSize $Message
    
    # Render initial display
    Render-Display $layout $Message $Delay
    
    # Countdown timer with progress bar
    $startTime = Get-Date
    $endTime = $startTime.AddSeconds($Delay)
    
    while ((Get-Date) -lt $endTime -and -not $script:Aborted) {
        # Check if terminal was resized and recalculate entire layout
        $currentTerminalSize = Get-SafeTerminalSize
        if (Test-TerminalSizeChanged $lastTerminalSize) {
            # Terminal was resized - recalculate everything and redraw
            $terminalSize = $currentTerminalSize
            $lastTerminalSize = $currentTerminalSize
            $layout = Calculate-Layout $currentTerminalSize $Message
            
            # Clear the screen and redraw everything with new layout
            Clear-Host
            Render-Display $layout $Message $Delay
        }
        
        $elapsedSeconds = ((Get-Date) - $startTime).TotalSeconds
        $remainingSeconds = [Math]::Max(0, [Math]::Ceiling(($endTime - (Get-Date)).TotalSeconds))
        $progress = [Math]::Min(100, [Math]::Floor(($elapsedSeconds / $Delay) * 100))
        
        # Always use the beautiful progress bar - just position it safely at the bottom
        $currentTerminalSize = Get-SafeTerminalSize
        
        # Create progress bar using helper function
        $progressBar = New-ProgressBar -Progress $progress
        
        # Create the timer text components
        $timerText = "⏱️  {0:00}s" -f $remainingSeconds
        $progressText = "{0:00}%" -f $progress
        $instructionText = "SPACE=abort, ENTER=continue"
        
        # Build the complete line and check if it fits
        $completeLine = "$timerText  [$progressBar] $progressText - $instructionText"
        
        # If the complete line is too long, truncate the instruction text
        if ($completeLine.Length -gt $currentTerminalSize.Width) {
            $instructionText = "SPACE/ENTER"
            $completeLine = "$timerText  [$progressBar] $progressText - $instructionText"
        }
        
        # Calculate centering position
        $leftPadding = [Math]::Max(0, [Math]::Floor(($currentTerminalSize.Width - $completeLine.Length) / 2))
        
        # Position the progress bar at the bottom of the terminal safely
        try {
            $bottomPosition = [Math]::Max(0, [Console]::WindowHeight - 2)  # Leave 1 line below
            [Console]::SetCursorPosition(0, $bottomPosition)
            
            # Clear the bottom line
            Write-Host (" " * [Console]::WindowWidth) -NoNewline
            [Console]::SetCursorPosition($leftPadding, $bottomPosition)
            
            # Write the beautiful centered progress bar at the bottom
            Write-Host $timerText -NoNewline -ForegroundColor White
            Write-Host "  [" -NoNewline -ForegroundColor White
            Write-Host $progressBar -NoNewline -ForegroundColor Green
            Write-Host "] " -NoNewline -ForegroundColor White
            Write-Host $progressText -NoNewline -ForegroundColor Cyan
            Write-Host " - " -NoNewline -ForegroundColor White
            Write-Host $instructionText -NoNewline -ForegroundColor Gray
        }
        catch {
            # If positioning fails, fall back to simple append
            Write-Host "`r⏱️ ${remainingSeconds}s remaining (${progress}%) - SPACE=abort, ENTER=continue" -NoNewline -ForegroundColor White
        }
        
        # Check for keypress multiple times during each update cycle for responsiveness
        for ($i = 0; $i -lt 10; $i++) {
            $keyResult = Test-Keypress
            if ($keyResult -eq 'abort') {
                $script:Aborted = $true
                break
            } elseif ($keyResult -eq 'continue') {
                # Set flag to proceed immediately without showing as aborted
                $script:Aborted = $false
                $endTime = Get-Date  # Force exit from outer loop
                break
            }
            # Only sleep if we haven't exceeded the time limit
            if ((Get-Date) -lt $endTime) {
                Start-Sleep -Milliseconds 10  # Much shorter sleep intervals
            } else {
                break
            }
        
        if ($script:Aborted -or (Get-Date) -ge $endTime) { break }
    }
    
    # Show final 100% completion if not aborted
    if (-not $script:Aborted) {
        try {
            $bottomPosition = [Math]::Max(0, [Console]::WindowHeight - 2)  # Leave 1 line below
            [Console]::SetCursorPosition(0, $bottomPosition)
            Write-Host (" " * [Console]::WindowWidth) -NoNewline
            
            $currentTerminalSize = Get-SafeTerminalSize
            
            # Create completion display using helper function
            $progressBar = New-ProgressBar -Complete
            $completeLine = "⏱️  00s  [$progressBar] 100% - Complete!"
            $leftPadding = [Math]::Max(0, [Math]::Floor(($currentTerminalSize.Width - $completeLine.Length) / 2))
            
            [Console]::SetCursorPosition($leftPadding, $bottomPosition)
            Write-Host "⏱️  " -NoNewline -ForegroundColor White
            Write-Host "00" -NoNewline -ForegroundColor Red
            Write-Host "s  [" -NoNewline -ForegroundColor White
            Write-Host $progressBar -NoNewline -ForegroundColor Green
            Write-Host "] " -NoNewline -ForegroundColor White
            Write-Host "100%" -NoNewline -ForegroundColor Cyan
            Write-Host " - Complete!" -ForegroundColor Green
        }
        catch {
            Write-Host "`r⏱️ 00s remaining (100%) - Complete!" -ForegroundColor Green
        }
        Start-Sleep -Milliseconds 500
    }
    
    Write-Host ""
    
    if ($script:Aborted) {
        # Clear the bottom line and show abort message
        try {
            $bottomPosition = [Math]::Max(0, [Console]::WindowHeight - 2)  # Leave 1 line below
            [Console]::SetCursorPosition(0, $bottomPosition)
            Write-Host (" " * [Console]::WindowWidth) -NoNewline
            [Console]::SetCursorPosition(0, $bottomPosition)
        } catch { }
        
        Write-Host ""
        
        $footerBorder = "═" * $layout.BoxWidth
        $abortText = "🛑 ABORTED 🛑"
        $abortPadding = [Math]::Max(0, [Math]::Floor(($layout.BoxWidth - $abortText.Length) / 2))
        $abortLine = (" " * $abortPadding) + $abortText + (" " * ($layout.BoxWidth - $abortPadding - $abortText.Length))
        
        Write-Host $footerBorder -ForegroundColor Red
        Write-Host $abortLine -ForegroundColor Red
        Write-Host $footerBorder -ForegroundColor Red
        Write-Host ""
        Write-Host "❌ Workflow aborted by user. Exiting with code 1." -ForegroundColor Red
        Write-Host ""
        exit 1
    } else {
        # Clear the bottom line before showing completion
        try {
            $bottomPosition = [Math]::Max(0, [Console]::WindowHeight - 2)  # Leave 1 line below
            [Console]::SetCursorPosition(0, $bottomPosition)
            Write-Host (" " * [Console]::WindowWidth) -NoNewline
        } catch { }
        
        Write-Host ""
        
        $footerBorder = "═" * $layout.BoxWidth
        $proceedText = "✅ PROCEEDING ✅"
        $proceedPadding = [Math]::Max(0, [Math]::Floor(($layout.BoxWidth - $proceedText.Length) / 2))
        $proceedLine = (" " * $proceedPadding) + $proceedText + (" " * ($layout.BoxWidth - $proceedPadding - $proceedText.Length))
        
        Write-Host $footerBorder -ForegroundColor Green
        Write-Host $proceedLine -ForegroundColor Green
        Write-Host $footerBorder -ForegroundColor Green
        Write-Host ""
        Write-Host "🚀 No abort detected. Proceeding with the workflow. Exiting with code 0." -ForegroundColor Green
        Write-Host ""
        exit 0
    }
}
catch {
    Write-Host ""
    Write-Host "❌ An error occurred: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
