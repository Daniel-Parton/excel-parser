$srcPath = Join-Path -Path $PSScriptRoot -ChildPath "..\src"
$testsToRun = Get-ChildItem -Path $srcPath -Recurse -File -Filter *.Test.csproj

foreach ($testToRun in $testsToRun) {
  dotnet test $testToRun.FullName --no-restore -c "Release"
  if ($LASTEXITCODE -ne 0) { $failure = $true }
}

if ($testFailure -eq $true) { throw "broken tests" }
