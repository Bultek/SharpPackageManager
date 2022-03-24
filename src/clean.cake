var target = Argument("target", "Clean");
Task("Clean")
    .Does(() =>
{
    CleanDirectory($"./SharpPackageManager/bin/Debug/net6.0");
    CleanDirectory($"./SharpPackageManager/bin/Release/net6.0");
    CleanDirectory($"./SharpPackageManager/obj");
});
RunTarget(target);