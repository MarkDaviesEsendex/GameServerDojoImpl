
$sourceDirectory = ".\src"
$nugetPath = ".\NuGet\NuGet.exe"
$solutionName = "Solution.sln"

& $nugetPath @("restore", "$sourceDirectory\$solutionName", "-ConfigFile", ".\NuGet\NuGet.config")

Invoke-Psake -properties @{
    "solution" = $solutionName;
    "projectName" = "Project";
}