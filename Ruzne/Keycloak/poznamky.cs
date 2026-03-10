using var client = new HttpClient();
    
// Parametry pro Keycloak (Service Account)
var content = new FormUrlEncodedContent(new[]
{
    new KeyValuePair<string, string>("grant_type", "client_credentials"),
    new KeyValuePair<string, string>("client_id", "vás-client-id"),
    new KeyValuePair<string, string>("client_secret", "váš-client-secret")
});

// URL: https://{server}/realms/{realm}/protocol/openid-connect/token
var response = await client.PostAsync("https://keycloak-server/realms/vás-realm/protocol/openid-connect/token", content);
response.EnsureSuccessStatusCode();

var json = await response.Content.ReadAsStringAsync();
var authData = JsonSerializer.Deserialize<JsonElement>(json);

string token = authData.GetProperty("access_token").GetString();


var apiClient = new HttpClient();
apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);