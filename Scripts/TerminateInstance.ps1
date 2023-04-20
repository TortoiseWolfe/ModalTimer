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

Write-Host "Warning: The instance will be terminated. Press any key to cancel the termination."

$warning_duration_seconds = 10

function Show-Countdown($duration) {
    $start_time = Get-Date
    do {
        $elapsed_time = (Get-Date) - $start_time
        $remaining_time = $duration - $elapsed_time.TotalSeconds
        Write-Host "`rTerminating in $($remaining_time.ToString("N0")) seconds...`t" -NoNewline
        Start-Sleep -Seconds 1
    } while ($elapsed_time.TotalSeconds -lt $duration)
}

Show-Countdown $warning_duration_seconds

if (-not $host.UI.RawUI.KeyAvailable) {
    aws ec2 terminate-instances --instance-ids $instance_id
} else {
    $null = $host.UI.RawUI.ReadKey("IncludeKeyUp,NoEcho")
}


# End of the script
Write-Log "TerminateEC2Instance.ps1 script completed"
