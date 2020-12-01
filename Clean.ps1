Get-ChildItem . -recurse -include bin | remove-item -recurse
Get-ChildItem . -recurse -include obj | remove-item -recurse
Get-ChildItem . -recurse -include node_modules | remove-item -recurse