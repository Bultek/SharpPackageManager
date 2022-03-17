var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Clean")
    .Does(() =>
{
    CleanDirectory($"./SharpPackageManager/bin/{configuration}/net6.0");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("SharpPackageManager.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});
RunTarget(target);