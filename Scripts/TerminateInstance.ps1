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

$instance_id = (Invoke-WebRequest -Uri 'http://169.254.169.254/latest/meta-data/instance-id').Content
Write-Log "Instance ID: $instance_id"

Write-Host "Warning: The instance will be terminated. Press any key to cancel the termination."

if (-not $host.UI.RawUI.KeyAvailable) {
    try {
        $result = aws ec2 terminate-instances --instance-ids $instance_id
        Write-Log "Terminate instance result: $result"
    } catch {
        Write-Log "Error terminating instance: $_"
    }
} else {
    $null = $host.UI.RawUI.ReadKey("IncludeKeyUp,NoEcho")
}

# End of the script
Write-Log "TerminateEC2Instance.ps1 script completed"
