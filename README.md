# ReadTweetsFromTwitterList

- Azure function to read tweets from Twitter List.
- API Key and API Secret needs to be obtained from Twitter Developers Account.
- Add `local.settings.json` to the root of the AzureFunction project.
- After Deployment to azure, update the same configs there too.
- You can use sample Twitter List ID : 1572510829198856192

## Format of local.settings.json

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "API_KEY": "###",
    "API_SECRET": "###",
    "TWITTER_LIST_ID": "###"
  }
}
```
