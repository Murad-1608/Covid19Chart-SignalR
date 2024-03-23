using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Models
{
    public class CovidService
    {
        private readonly AppDbContext appDbContext;
        private readonly IHubContext<CovidHub> hubContext;

        public CovidService(AppDbContext appDbContext, IHubContext<CovidHub> hubContext)
        {
            this.appDbContext = appDbContext;
            this.hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return appDbContext.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await appDbContext.Covids.AddAsync(covid);
            await appDbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("ReceiveCovidList","data");
        }

    }
}
