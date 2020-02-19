$MONGODIR = "../mongo/data"

if (!(Test-Path -Path $MONGODIR )) {
    New-Item -ItemType directory -Path $MONGODIR
}

$MONGOCOMMAND = "-noexit mongod --dbpath " + $MONGODIR 

Start-Process -FilePath "powershell" -ArgumentList $MONGOCOMMAND 

Start-Process -FilePath "powershell" -ArgumentList "-noexit EventStore.ClusterNode.exe --db ../eventstore/db --log ../eventstore/logs"