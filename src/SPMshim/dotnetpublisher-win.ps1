dotnet publish .\SPMshim.csproj -o win-x64 -r win-x64  --self-contained true -o bin\RenameTheShimToYourPackageName /p:PublishSingleFile=true /p:IncludeNativeLibariesForSelfExtract=true
