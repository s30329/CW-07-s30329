namespace APBD07.Models;

public class Trip
{
    public int TripId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxPeople { get; set; }
    public List<Country> Countries { get; set; } = new();
}