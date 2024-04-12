#!/bin/bash

# Find all .csproj files and update the target framework to .NET 8
find . -name "*.csproj" -exec sed -i 's/net6.0/net8.0/g' {} \;

echo "Upgrade to .NET 8 completed."
