rm -r -f ./published
dotnet publish ./src/DbDump/DbDump.csproj -c Release -o ./published -p:PublishTrimmed=true -r win-x64