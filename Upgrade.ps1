# Get-ChildItem . -recurse -include *.csproj | upgrade-assistant --skip-backup --non-interactive upgrade $_.Name

Write-Output "Iterating over all projects in folder"
foreach ( $file in Get-ChildItem -recurse -include *.csproj)
{
    Write-Output "Upgrading project $file"
    upgrade-assistant upgrade --skip-backup --non-interactive $file
}

Write-Output "Remove .clef files"
Get-ChildItem . -recurse -include *.clef | remove-item

Write-Output "Iterating over NPM packages"
foreach ( $file in Get-ChildItem -recurse -include package.json | Where-Object {!($_.Directory -like "*node_modules*")} )
{
    Write-Output $file
    Set-Location $file.Directory -PassThru
    ncu -u
    npm install
}
