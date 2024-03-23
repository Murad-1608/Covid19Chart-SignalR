using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
            await hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();

            using (var command = appDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarix,[1],[2],[3],[4],[5] from " +
                    "(select [City],[Count],Cast([CovidDate] as date) as tarix from Covids) as covidT " +
                    "PIVOT " +
                    "(sum(Count) for City in([1],[2],[3],[4],[5])) as PTable " +
                    "order by tarix asc";

                command.CommandType = System.Data.CommandType.Text;

                appDbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart covidChart = new();
                        covidChart.CovidDate = reader.GetDateTime(0).ToLongDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                covidChart.Counts.Add(0);
                            }
                            else
                            {
                                covidChart.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(covidChart);

                    }
                }
                appDbContext.Database.CloseConnection();

                return covidCharts;
            }
        }

    }
}
