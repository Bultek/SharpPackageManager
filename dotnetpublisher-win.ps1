dotnet publish $args[0] -o win-x64 -r win-x64  --self-contained true -o $args[1] /p:PublishSingleFile=true /p:IncludeNativeLibariesForSelfExtract=true
