How to run:

- Install Azure Storage Emulator
- Modify local.settings.json file, add `"AzureWebJobsStorage": "UseDevelopmentStorage=true"`
- Press F5 to run the service

Update local.settings.json to:
```{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  }
}
```

How to populate the database:

- Check `Scripts` folder. It contains scripts that will prepopulate the database.
- The easiest way is to run `run.ps1` which will prepulate with games, members, wishlist selections, voting sessions and voting session entries.
- If you want to prepulate only one specific table, run each script individually.
