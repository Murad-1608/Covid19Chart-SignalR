using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class CovidsController(CovidService covidService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await covidService.SaveCovid(covid);

            IQueryable<Covid> covidList = covidService.GetList();

            return Ok(covid);
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
                        CovidDate = DateTime.Now.AddDays(2),
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
