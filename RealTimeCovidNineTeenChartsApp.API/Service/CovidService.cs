using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeCovidNineTeenChartsApp.API.Context;
using RealTimeCovidNineTeenChartsApp.API.Hubs;
using RealTimeCovidNineTeenChartsApp.API.Models;

namespace RealTimeCovidNineTeenChartsApp.API.Service;

public class CovidService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<CovidHub> _hubContext;
    
    public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public IQueryable<Covid> GetAll()
    {
        return _context.Covids.AsQueryable();
    }

    public async Task SaveAsync(Covid entity)
    {
        await _context.Covids.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("ReceiveCovidList", await GetCovidChartList());
    }

    public async Task<List<CovidChart>> GetCovidChartList()
    {
        List<CovidChart> covidCharts = new();
        await using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT tarih, [1], [2], [3], [4], [5] FROM (SELECT [City], [Count], CAST([CovidDate] as date) as tarih FROM Covids) AS CovidTable PIVOT (SUM(Count) FOR City IN([1], [2], [3], [4], [5])) AS PTable ORDER BY tarih ASC";
        command.CommandType = CommandType.Text;
        await _context.Database.OpenConnectionAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            CovidChart cc = new();
            cc.CovidDate = reader.GetDateTime(0).ToString();
            Enumerable.Range(1,5).ToList().ForEach((i) =>
            {
                if (DBNull.Value.Equals(reader[i]))
                {
                    cc.Counts.Add(0);
                }
                else
                {
                    cc.Counts.Add(reader.GetInt32(i));
                }
            });
            covidCharts.Add(cc);
        }

        await _context.Database.CloseConnectionAsync();

        return covidCharts;
    }
}