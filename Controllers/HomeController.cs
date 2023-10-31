using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

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
    [HttpPost]
    [Route("/api/order")]
    public void Email(double orderPrice, string currency)
    {
        var message = new MimeMessage ();
        message.From.Add(new MailboxAddress("WebshopCheckout", Environment.GetEnvironmentVariable("FROM_EMAIL")));
        message.To.Add(new MailboxAddress("To whom it may concern", Environment.GetEnvironmentVariable("TO_EMAIL")));
        message.Subject = "I'd like to reach you about your cars extended warranty";
        message.Body = new TextPart("plain")
        {
            Text = @"Total order price"+ orderPrice + " " + currency
        };
        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com",465 ,true);

            client.Authenticate(Environment.GetEnvironmentVariable("FROM_EMAIL"), "FROM_PASS");
            client.Disconnect(true);
        }

    }
}
