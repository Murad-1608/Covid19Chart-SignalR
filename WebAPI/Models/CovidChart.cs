namespace WebAPI.Models
{
    public class CovidChart
    {
        public CovidChart()
        {
            Counts = new List<int>();
        }

        public List<int> Counts { get; set; }
        public string CovidDate { get; set; }

    }
}
