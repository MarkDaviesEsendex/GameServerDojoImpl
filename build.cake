#tool "nuget:?package=NUnit.ConsoleRunner"

var ouputDirectory = "../../output";
var solutionFile = "./**/*.sln";

Task("Clean")
    .Does(() =>
{
    var directoriesToClean = GetDirectories("./output");
    CleanDirectories(directoriesToClean);

    directoriesToClean = GetDirectories("./src/**/bin/");
    CleanDirectories(directoriesToClean);
});

Task("NugetRestore")
    .Does(() =>
{
    var solutions = GetFiles(solutionFile);
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        NuGetRestore(solution);
    }
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    NUnit3("./src/**/bin/**/*.Tests.dll");
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("NugetRestore")
    .Does(() =>
{
    var solutions = GetFiles(solutionFile);
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        MSBuild(solution, configurator => 
                configurator.SetConfiguration("Release")
                    .SetVerbosity(Verbosity.Minimal)
                    .WithProperty("OutDir", ouputDirectory));
    }
});

RunTarget("Test");