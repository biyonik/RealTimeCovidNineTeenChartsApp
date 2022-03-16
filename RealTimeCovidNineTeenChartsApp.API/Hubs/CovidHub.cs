using Microsoft.AspNetCore.SignalR;
using RealTimeCovidNineTeenChartsApp.API.Service;

namespace RealTimeCovidNineTeenChartsApp.API.Hubs;

public class CovidHub: Hub
{
    private readonly CovidService _covidService;

    public CovidHub(CovidService covidService)
    {
        _covidService = covidService;
    }
    
    public async Task GetCovidListAsync()
    {
        await Clients.All.SendAsync("ReceiveCovidList", await _covidService.GetCovidChartList());
    }
}