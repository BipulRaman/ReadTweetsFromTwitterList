# ReadTweetsFromTwitterList

Azure function to read tweets from Twitter List.
API Key and API Secret needs to be obtained from Twitter Developers Account.

## Format of local.settings.json

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "API_KEY": "###",
    "API_SECRET": "###"
  }
}
```
