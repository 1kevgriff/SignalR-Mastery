Write-Output "Remove .clef files"
Get-ChildItem . -recurse -include *.clef | remove-item

Write-Output "Iterating over NPM packages"
foreach ( $file in Get-ChildItem -recurse -include package.json | Where-Object {!($_.Directory -like "*node_modules*")} )
{
    Write-Output $file
    Push-Location $file.Directory
    try {
        ncu -u
        npm install
    }
    finally {
        Pop-Location
    }
}
