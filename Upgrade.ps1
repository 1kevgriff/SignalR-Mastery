# Get-ChildItem . -recurse -include *.csproj | upgrade-assistant --skip-backup --non-interactive upgrade $_.Name

foreach ( $file in Get-ChildItem -recurse -include *.csproj)
{
    upgrade-assistant upgrade --skip-backup --non-interactive $file
}

Get-ChildItem . -recurse -include *.orig | remove-item
