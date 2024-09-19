using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BackupDatabaseFunconality.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase
    {
        [HttpPost("backup")]
        public IActionResult BackupDatabase(string databaseName, string backupFolder)
        {
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(backupFolder))
            {
                return BadRequest("Database name and backup folder are required.");
            }

            try
            {
                string backupFileName = $"{databaseName}_{DateTime.Now:yyyy-MM-dd}.bak";
                string backupFilePath = System.IO.Path.Combine(backupFolder, backupFileName);
                string connectionString = $"Server=DESKTOP-;Database={databaseName};User Id=your_username;Password=your_password;MultipleActiveResultSets=true;TrustServerCertificate=true;";

                using (var connection = new SqlConnection($"Server=DESKTOP-LNACHR8;Database={databaseName};;MultipleActiveResultSets=true;TrustServerCertificate=true;Integrated Security=True;"))
                {
                    connection.Open();
                    var command = new SqlCommand($"BACKUP DATABASE [{databaseName}] TO DISK = @backupFile", connection);
                    command.Parameters.AddWithValue("@backupFile", backupFilePath);
                    command.ExecuteNonQuery();
                }

                return Ok($"Backup created successfully at {backupFilePath}");
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, $"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
