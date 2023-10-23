using System.Diagnostics;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using WebshopCheckoutAPI.Models;

namespace WebshopCheckoutAPI.Controllers;

public class HomeController : Controller
{
    private static readonly HttpClient client = new HttpClient();
    [HttpGet]
    [Route("/api/address")]
    public async Task<IActionResult> Address([FromQuery] string searchTerm)
    {
        var GeoAPIKey = Environment.GetEnvironmentVariable("GEO_CODE_API_KEY");
        var resp = await client.GetAsync("https://api.geoapify.com/v1/geocode/autocomplete?text=" + searchTerm + "&format=json&apiKey=" + GeoAPIKey);
        var responseBody = await resp.Content.ReadAsStringAsync();
        return Content(responseBody);
    }

    [HttpGet]
    [Route("/api/convert")]
    public async Task<IActionResult> CurrencyConvert([FromQuery] string countryCode)
    {
        var FreeAPIKey = Environment.GetEnvironmentVariable("FREE_CODE_API_KEY");
        var resp = await client.GetAsync("https://api.freecurrencyapi.com/v1/latest?base_currency=DKK&currencies=" +
                                         countryCode + "&apikey=" + FreeAPIKey);
        var responseBody = await resp.Content.ReadAsStringAsync();
        return Content(responseBody);
    }
}
