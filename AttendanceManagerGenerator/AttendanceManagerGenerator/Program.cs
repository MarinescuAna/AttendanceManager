using AttendanceManagerGenerator.Handlers;
using AttendanceManagerGenerator.Modules;
using System.Net.Http.Headers;
using System.Text.Json;

using StreamReader stream = File.OpenText("configuration.json");
var configurationString = stream.ReadToEnd();
var config = JsonSerializer.Deserialize<Configuration>(configurationString) ?? throw new Exception("Can't read the configuration file!");
stream.Close();
stream.Dispose();

using var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:44325/");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

var currentUserToken = await new LoginHandler(client).LoginAsync(config.CurrentUser.Email, config.CurrentUser.Password);

if (currentUserToken != null)
{
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", currentUserToken.AccessToken);
    Console.WriteLine("Authenticated!");
}
else
{
    Console.WriteLine("Authentication faild!");
    return;
}

await new ReportHandler(client).InsertReportAsync(config.Report);
Console.WriteLine("DONE!");