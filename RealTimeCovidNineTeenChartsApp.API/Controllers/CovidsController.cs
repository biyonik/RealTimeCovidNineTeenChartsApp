using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeCovidNineTeenChartsApp.API.Models;
using RealTimeCovidNineTeenChartsApp.API.Service;

namespace RealTimeCovidNineTeenChartsApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CovidsController : ControllerBase
{
    private readonly CovidService _covidService;

    public CovidsController(CovidService covidService)
    {
        _covidService = covidService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveCovid(Covid covid)
    {
        await _covidService.SaveAsync(covid);
        var serializedData = JsonSerializer.Serialize(await _covidService.GetCovidChartList());
        var deserializedData = JsonSerializer.Deserialize<List<CovidChart>>(serializedData);
        return Ok(deserializedData);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> InitializeCovid()
    {
        Random random = new();
        Enumerable.Range(1, 10).ToList().ForEach((day) =>
        {
            foreach (City item in Enum.GetValues(typeof(City)))
            {
                Covid newCovid = new()
                {
                    Id = Guid.NewGuid(),
                    City = item,
                    Count = random.Next(100, 1000000),
                    CovidDate = DateTime.Now.AddDays(day)
                };
                _covidService.SaveAsync(newCovid).Wait();
                Thread.Sleep(1000);
            }
        });
        return Ok("Covid-19 verileri veritabanına kaydedildi");
    }
}