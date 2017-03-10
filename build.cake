#tool "nuget:?package=NUnit.ConsoleRunner"

var ouputDirectory = "ouput";

Task("Clean")
    .Does(() =>
{
    if(DirectoryExists(ouputDirectory))
    {
        DeleteDirectory(ouputDirectory, recursive:true);
    }
    CreateDirectory(ouputDirectory);
});

Task("Test")
    .Does(() => 
{
    NUnit3("Tests.dll");
});

Task("Build")
    .Does(() =>
{
    MSBuild("Solution", new MSBuildSettings()
        .WithProperty("OutDir", ouputDirectory)
        .WithProperty("DeployOnBuild", "true")
        .WithProperty("WebPublishMethod", "Package")
        .WithProperty("PackageAsSingleFile", "true")
        .WithProperty("SkipInvalidConfigurations", "true"));
})

RunTarget("Test");