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

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    NUnit3("./src/**/bin/**/*.Tests.dll");
});

Task("Publish")
    .IsDependentOn("Test")
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
                    .WithProperty("DeployOnBuild", "true")
                    .WithProperty("WebPublishMethod", "Package")
                    .WithProperty("PackageAsSingleFile", "true")
                    .WithProperty("SkipInvalidConfigurations", "true")
                    .WithProperty("OutDir", ouputDirectory));
    }
});

Task("Deploy")
    .IsDependentOn("Publish")
    .Does(() => 
{
    var parameterFile = MakeAbsolute(File("./output/_PublishedWebsites/Server.Web_Package/Server.Web.SetParameters.xml"));
    var command = MakeAbsolute(File("./output/_PublishedWebsites/Server.Web_Package/Server.Web.deploy.cmd")).ToString();
    
    XmlPoke(parameterFile, "//parameters/setParameter[@name='IIS Web Application Name']/@value", "BuildAutomation/MarkD");
    using(var process = StartAndReturnProcess(command, new ProcessSettings{ Arguments = "/Y -allowUntrusted:true /M:csWebServerD01.collstream.local /U:webdeploy@collstream.local /P:6I542oz0X5" }))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code: {0}", process.GetExitCode());
    }
});

RunTarget("Deploy");