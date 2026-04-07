# Disable progress bars for performance
$ProgressPreference = 'SilentlyContinue'

Mock Start-Sleep { }
Mock Invoke-WebRequest { throw "Should not make HTTP calls" }
Mock Invoke-RestMethod { throw "Should not make HTTP calls" }
