using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController(CovidService covidService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await covidService.SaveCovid(covid);

            List<CovidChart> covidList = covidService.GetCovidChartList();

            return Ok(covidList);
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new();

            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid
                    {
                        City = item,
                        CovidDate = DateTime.Now.AddDays(x),
                        Count = random.Next(100, 1000)
                    };
                    covidService.SaveCovid(newCovid).Wait();
                    Thread.Sleep(1000);
                }
            });

            return Ok("Datalar yuklendi");
        }
    }
}
