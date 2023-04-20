function Write-Log {
    param (
        [string]$Message,
        [string]$LogFilePath = (Join-Path -Path $PSScriptRoot -ChildPath "TerminateEC2Instance.log")
    )

    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logEntry = "$timestamp - $Message"

    try {
        Add-Content -Path $LogFilePath -Value $logEntry
    }
    catch {
        Write-Host "Failed to write to log file: $($_.Exception.Message)"
    }
}

# Start of the script
Write-Log "Starting TerminateEC2Instance.ps1 script"

# Your script logic goes here
# ...

# End of the script
Write-Log "TerminateEC2Instance.ps1 script completed"
