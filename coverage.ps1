dotnet restore
dotnet build

Set-Location tools

# Instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --workdir ../ --assemblies test/**/bin/**/*.dll --sources src/**/*.cs 

# Reset hits count in case minicover was run for this project
dotnet minicover reset --workdir ../

Set-Location ..

Get-ChildItem test/**/*.csproj | ForEach-Object { dotnet test --no-build $_.FullName }
#for project in test/**/*.csproj; do dotnet test --no-build $project; done

Set-Location tools

# Uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument --workdir ../

# Create html reports inside folder coverage-html
dotnet minicover htmlreport --workdir ../ --threshold 90 --output ./test/coverage/html

# Print console report
# This command returns failure if the coverage is lower than the threshold
dotnet minicover report --workdir ../ --threshold 90

Set-Location ..
Remove-Item ./coverage.json
Remove-Item ./coverage.hits