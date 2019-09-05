Clear-Host

Write-Host "Running Tests and collecting coverage stats"
dotnet test /p:AltCover=true /p:AltCoverThreshold=90 /p:AltCoverAssemblyExcludeFilter="NUnit|Microsoft|xunit|AltCover|Tests" /p:AltCoverTypeFilter="Program" /p:AltCoverForce=true /p:AltCoverLocalSource=true

Write-Host "Generating Coverage Report"
$reportGenVersion = "4.2.19";
$reportGen = "$env:UserProfile\.nuget\packages\reportgenerator\$reportGenVersion\tools\netcoreapp2.1\ReportGenerator.dll";
#Start-Process dotnet -ArgumentList "$reportGen","-reports:./test/**/coverage.xml","-targetdir:.\test\artifacts","-reporttypes:MHTML;Badges" -NoNewWindow;
dotnet $reportGen -reports:./test/**/coverage.xml -targetdir:.\test\artifacts -reporttypes:"MHTML;Badges"

Write-Host "Launching Coverage Report"
Invoke-Item -Path .\test\artifacts\summary.mht