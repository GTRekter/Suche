# Suche
Application which show how to execute searches on both Azure Cognitive Search and AWS ElasticSearch Service

# Configurations
File **appsettings.json**

```
"Auth0": {
    "Domain": "AUTH0_API_DOMAIN",
    "ApiIdentifier": "AUTH0_API_IDENTIFIER"
 },
"Azure": {
    "SearchServiceName": "AZURE_COGNITIVESERACH_API_NAME",
    "SearchServiceAdminApiKey": "AZURE_COGNITIVESERACH_API_KEY",
    "SearchIndexName": "AZURE_INDEX_NAME"
},
"Amazon": {
    "AccessKeyId": "AWS_IAMUSER_ACCESSKEY",
    "SecretAccessKey": "AWS_IAMUSER_SECRETACCESSKEY",
    "Region": "AWS_ELASTICSEARCH_SERVICE_REGION",
    "DomainName": "AWS_ELASTICSEARCH_DOMAIN"
    // Obsolete
    //"SearchServiceEndpoint": "AWS_ELASTIC_SEARCH_SERVICE_ENDPOINT"
}
```
## Performances
Performances executing the same search query on the same **35,543 documents**.

Property | Azure Cognitive Search | AWS Elasticsearch
------------ | ------------ | -------------
First time byte | ## | 329.96400000000006
Response time | ## | 330.10200000000003


## Example to get an Auth0 Access Token
```
HttpResponseMessage response = null;
TokenResponse token = null;
var tokenResponse = GetAccessToken();
Console.WriteLine($"Method: {tokenResponse.RequestMessage.Method}");
Console.WriteLine($"Request: {tokenResponse.RequestMessage.RequestUri.ToString()}");
Console.WriteLine($"Status code: {tokenResponse.StatusCode}");
Console.WriteLine($"Reason: {tokenResponse.ReasonPhrase}");
if (tokenResponse.IsSuccessStatusCode)
{
    string content = tokenResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    token = JsonConvert.DeserializeObject<TokenResponse>(content);
    WriteColoredLine($"AccessToken: {token.Access_token} | Type {token.Token_type}", ConsoleColor.Green);
}
Console.WriteLine("-----------------------------------------");
private static HttpResponseMessage GetAccessToken()
{
    var tokenRequest = new
    {             
        client_id = "APPLICATION_CLIENT_ID",
        client_secret = "APPLICATION_CLIENTSECRET",
        audience = "APPLICATION_AUDIANCE",
        grant_type = "client_credentials"
    };
    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("APPLICATION_URL");
    return client.PostAsync("oauth/token", httpContent).GetAwaiter().GetResult();
}
```
## Example to Azure search
```
string searchText = "Microsoft";
response = SearchCategories(searchText, token);
Console.WriteLine($"Method: {response.RequestMessage.Method}");
Console.WriteLine($"Request: {response.RequestMessage.RequestUri.ToString()}");
Console.WriteLine($"Status code: {response.StatusCode}");
Console.WriteLine($"Reason: {response.ReasonPhrase}");
if (response.IsSuccessStatusCode)
{
    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(content);
    foreach (var category in categories)
    {
        WriteColoredLine(category.ToString(), ConsoleColor.Green);
    }
}
private static HttpResponseMessage SearchCategories(string searchText, TokenResponse token)
{
    var json = JsonConvert.SerializeObject(new SearchCategoryRequest() { Text = searchText });
    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.Token_type, token.Access_token);
    return client.PostAsync("categories/search", httpContent).GetAwaiter().GetResult();
}
```
