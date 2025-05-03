using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using APBD07.Models;

namespace APBD07.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ClientsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Endpoint POST /api/clients
    /// Tworzy nowego klienta.
    /// </summary>
    [HttpPost]
    public IActionResult AddClient(Client client)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var command = new SqlCommand("""
            INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
            OUTPUT INSERTED.IdClient
            VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
        """, connection);

        command.Parameters.AddWithValue("@FirstName", client.FirstName);
        command.Parameters.AddWithValue("@LastName", client.LastName);
        command.Parameters.AddWithValue("@Email", client.Email);
        command.Parameters.AddWithValue("@Telephone", client.Telephone);
        command.Parameters.AddWithValue("@Pesel", client.Pesel);

        var id = (int)command.ExecuteScalar();
        return Created($"api/clients/{id}", new { id });
    }

    /// <summary>
    /// Endpoint PUT /api/clients/{id}/trips/{tripId}
    /// Rejestruje klienta na wycieczkę.
    /// </summary>
    [HttpPut("{id}/trips/{tripId}")]
    public IActionResult RegisterClientToTrip(int id, int tripId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Client WHERE IdClient = @Id", connection);
        checkCmd.Parameters.AddWithValue("@Id", id);
        if ((int)checkCmd.ExecuteScalar() == 0)
            return NotFound("Client not found");

        checkCmd.CommandText = "SELECT COUNT(*) FROM Trip WHERE IdTrip = @TripId";
        checkCmd.Parameters.Clear();
        checkCmd.Parameters.AddWithValue("@TripId", tripId);
        if ((int)checkCmd.ExecuteScalar() == 0)
            return NotFound("Trip not found");

        checkCmd.CommandText = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @Id AND IdTrip = @TripId";
        checkCmd.Parameters.AddWithValue("@Id", id);
        if ((int)checkCmd.ExecuteScalar() > 0)
            return Conflict("Client already registered");

        using var insertCmd = new SqlCommand("""
            INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt)
            VALUES (@Id, @TripId, GETDATE());
        """, connection);
        insertCmd.Parameters.AddWithValue("@Id", id);
        insertCmd.Parameters.AddWithValue("@TripId", tripId);
        insertCmd.ExecuteNonQuery();

        return Ok("Client registered successfully");
    }

    /// <summary>
    /// Endpoint DELETE /api/clients/{id}/trips/{tripId}
    /// Usuwa rejestrację klienta z wycieczki.
    /// </summary>
    [HttpDelete("{id}/trips/{tripId}")]
    public IActionResult UnregisterClientFromTrip(int id, int tripId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var cmd = new SqlCommand("SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @Id AND IdTrip = @TripId", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@TripId", tripId);
        if ((int)cmd.ExecuteScalar() == 0)
            return NotFound("Registration not found");

        cmd.CommandText = "DELETE FROM Client_Trip WHERE IdClient = @Id AND IdTrip = @TripId";
        cmd.ExecuteNonQuery();

        return Ok("Client unregistered from trip");
    }

    /// <summary>
    /// Endpoint DELETE /api/clients/{id}
    /// Usuwa klienta, jeśli nie jest zapisany na żadną wycieczkę.
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult DeleteClient(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Client WHERE IdClient = @Id", connection);
        checkCmd.Parameters.AddWithValue("@Id", id);
        if ((int)checkCmd.ExecuteScalar() == 0)
            return NotFound("Client not found");

        checkCmd.CommandText = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @Id";
        if ((int)checkCmd.ExecuteScalar() > 0)
            return BadRequest("Client is assigned to trips");

        using var deleteCmd = new SqlCommand("DELETE FROM Client WHERE IdClient = @Id", connection);
        deleteCmd.Parameters.AddWithValue("@Id", id);
        deleteCmd.ExecuteNonQuery();

        return Ok("Client deleted");
    }
}
