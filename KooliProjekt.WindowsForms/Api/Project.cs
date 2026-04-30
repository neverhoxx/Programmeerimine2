namespace KooliProjekt.WindowsForms
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Budget { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
