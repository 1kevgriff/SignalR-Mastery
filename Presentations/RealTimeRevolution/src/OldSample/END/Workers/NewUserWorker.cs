
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class NewUserWorker : BackgroundService
{
    private readonly IHttpClientFactory _factory;
    private ILogger _logger;
    private readonly IHubContext<SyncHub> hubContext;

    public NewUserWorker(IHttpClientFactory factory, ILogger<NewUserWorker> logger, IHubContext<SyncHub> hubContext)
    {
        this._factory = factory;
        _logger = logger;
        this.hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(10000);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var r = await GetUser();

                await hubContext.Clients.All.SendAsync("NewUser", r.Results.First());

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
        catch (Exception ex){
            _logger.LogError(ex, "Error");
        }

        _logger.LogInformation("Shutting down worker...");
    }

    private async Task<NewUser> GetUser()
    {
        var url = "https://randomuser.me/api/";
        _logger.LogInformation("Getting new user");

        using (var client = _factory.CreateClient())
        {
            var response = await client.GetStringAsync(url);

            NewUser newUser = JsonSerializer.Deserialize<NewUser>(response);
            return newUser;
        }
    }
}
public class Name
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("first")]
    public string First { get; set; }

    [JsonPropertyName("last")]
    public string Last { get; set; }
}

public class Street
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Coordinates
{
    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }
}

public class Timezone
{
    [JsonPropertyName("offset")]
    public string Offset { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Location
{
    [JsonPropertyName("street")]
    public Street Street { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("coordinates")]
    public Coordinates Coordinates { get; set; }

    [JsonPropertyName("timezone")]
    public Timezone Timezone { get; set; }
}

public class Login
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("salt")]
    public string Salt { get; set; }

    [JsonPropertyName("md5")]
    public string Md5 { get; set; }

    [JsonPropertyName("sha1")]
    public string Sha1 { get; set; }

    [JsonPropertyName("sha256")]
    public string Sha256 { get; set; }
}

public class Dob
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }
}

public class Registered
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }
}

public class Id
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class Picture
{
    [JsonPropertyName("large")]
    public string Large { get; set; }

    [JsonPropertyName("medium")]
    public string Medium { get; set; }

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; }
}

public class Result
{
    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("name")]
    public Name Name { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("login")]
    public Login Login { get; set; }

    [JsonPropertyName("dob")]
    public Dob Dob { get; set; }

    [JsonPropertyName("registered")]
    public Registered Registered { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("cell")]
    public string Cell { get; set; }

    [JsonPropertyName("id")]
    public Id Id { get; set; }

    [JsonPropertyName("picture")]
    public Picture Picture { get; set; }

    [JsonPropertyName("nat")]
    public string Nat { get; set; }
}

public class Info
{
    [JsonPropertyName("seed")]
    public string Seed { get; set; }

    [JsonPropertyName("results")]
    public int Results { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }
}

public class NewUser
{
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; }

    [JsonPropertyName("info")]
    public Info Info { get; set; }
}

