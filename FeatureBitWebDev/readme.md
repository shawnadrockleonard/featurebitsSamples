## commands to start
dotnet fbit add -n DummyOn -o true
dotnet fbit add -n DummyOff -o false

dotnet fbit list


dotnet fbit generate


dotnet fbit add --excluded-environments "LocalDev" -n DummyNoDev -o true

dotnet fbit generate --force