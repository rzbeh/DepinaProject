using MechanicProject.DTOs.UserViewModels;
using MechanicProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using ServiceReference1;

namespace Mechanic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;  // Assuming you are using EF Core for database access
        private readonly string _username = "09120708177";   // Replace with your actual username
        private readonly string _password = "4thvahdati@FB";   // Replace with your actual password
        private readonly string _from = "20001390";             // Replace with your sender ID
        private readonly int _bodyId = 185798;

        public SmsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST api/sms/send-serial
        [HttpPost("send-serial")]
        public async Task<IActionResult> SendSerial([FromBody] PhoneNumberRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            // Check if the phone number is associated with a serial number in the database
            var user = await _dbContext.Sms
                .Where(u => u.User == request.PhoneNumber)
                 .OrderByDescending(u => u.Time)
                .Select(u => new { u.Serial })  // Assuming SerialNumber is an int
                .FirstOrDefaultAsync();

            if (user == null || user.Serial == 0)
            {
                return NotFound("No serial number found for this phone number.");
            }

            try
            {
                // Create the SOAP client instance (generated from the WSDL)
                using (var soapClient = new SendSoapClient(SendSoapClient.EndpointConfiguration.SendSoap))
                {
                    var text = $"Your serial number is: {user.Serial}";

                    // Call the SMS service to send the serial number to the user's phone
                    var result = await soapClient.SendByBaseNumber2Async(_username, _password, text, request.PhoneNumber, _bodyId);


                    if (!string.IsNullOrEmpty(result)) // Assuming a positive result means success
                    {
                        return Ok("Serial number sent via SMS successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "Failed to send SMS.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}