rd /s /q .\TestResults
rd /s /q .\CodeCoverage

dotnet test .\CostsDiary.Api.UnitTests\CostsDiary.Api.UnitTests.csproj --no-build --results-directory:".\TestResults" --collect:"XPlat Code Coverage"

REM if you get an error at next step run the following command one time
REM dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator -reports:.\TestResults\**\coverage.cobertura.xml -targetdir:.\CodeCoverage -reporttypes:HtmlInline_AzurePipelines;Cobertura

start chrome .\CodeCoverage\index.htm

pause