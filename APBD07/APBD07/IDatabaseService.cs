using APBD07.Models;

namespace APBD07.Services;

public interface IDatabaseService
{
    IEnumerable<Trip> GetTrips();
    IEnumerable<Client> GetClients();
}