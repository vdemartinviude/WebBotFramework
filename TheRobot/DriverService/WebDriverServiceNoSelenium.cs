using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.DriverService;

public enum KindOfBrowser
{
    Chrome,
    Edge,
    Firefox
}

public class WebDriverServiceNoSelenium : IWebDriverServiceNoSeleniun
{
    private readonly KindOfBrowser _browser;
    private readonly IConfiguration _configuration;
    private Process _process;
    private readonly HttpClient _httpClient;

    public WebDriverServiceNoSelenium(KindOfBrowser browser, IConfiguration configuration, HttpClient httpClient)
    {
        _browser = browser;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public void Dispose()
    {
        _process.CloseMainWindow();
    }

    public async Task Start()
    {
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = _configuration.GetRequiredSection("NoSeleniumConfiguration").GetValue<string>("EdgeWebDriverPath"),
            WindowStyle = ProcessWindowStyle.Maximized,
            Arguments = "--port=3698",
            UseShellExecute = true,
        };

        _process = Process.Start(info);
        await Task.Delay(5000);
    }

    public async Task NewSession(CancellationToken token)
    {
        Uri baseuri = new("http://localhost:3698");
        _httpClient.BaseAddress = baseuri;
        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsJsonAsync("session", new NoSeleniumCapabilities { capabilities = new() }, token);
    }
}