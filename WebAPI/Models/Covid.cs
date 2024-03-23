namespace WebAPI.Models
{
    public enum ECity
    {
        Baku = 1,
        Istanbul = 2,
        London = 3,
        Berlin = 4,
        Hamilton = 5
    }
    public class Covid
    {
        public int Id { get; set; }
        public ECity City { get; set; }
        public int Count { get; set; }
        public DateTime CovidDate { get; set; }
    }
}
