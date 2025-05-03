using APBD07.Models;

namespace APBD07.Services;

public class DatabaseService : IDatabaseService
{
    private readonly DatabaseContext _context;

    public DatabaseService(DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<Trip> GetTrips()
    {
        return _context.Trips.ToList();
    }

    public IEnumerable<Client> GetClients()
    {
        return _context.Clients.ToList();
    }
}