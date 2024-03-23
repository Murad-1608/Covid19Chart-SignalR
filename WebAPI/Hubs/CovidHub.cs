using Microsoft.AspNetCore.SignalR;
using WebAPI.Models;

namespace WebAPI.Hubs
{
    public class CovidHub : Hub
    {
        private readonly CovidService covidService;
        public CovidHub(CovidService covidService)
        {
            this.covidService = covidService;
        }
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", covidService.GetCovidChartList());
        }
    }
}
