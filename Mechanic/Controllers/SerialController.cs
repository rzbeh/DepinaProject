using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MechanicProject.DTOs.UserViewModels;
using MechanicProject.Services.Interface;
using Microsoft.AspNetCore.Hosting;

namespace MechanicProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialController : ControllerBase
    {
        private readonly ISerialRepository _serialRepository;
        private readonly IWebHostEnvironment _iwebhostenviroment;

        public SerialController(ISerialRepository serialRepository , IWebHostEnvironment iwebhostenviroment)
        {
            _serialRepository = serialRepository;
            _iwebhostenviroment = iwebhostenviroment;
        }


        // Endpoint to check and update phone number
        [HttpPost("check-serial")]
        public IActionResult CheckSerial([FromBody] SerialRequestDto request)
        {
            if (!_serialRepository.IsSerialExist(request.Serial))
            {
                return NotFound("Serial number does not exist.");
            }

            var result = _serialRepository.CheckAndUpdatePhoneNumber(request.Serial, request.PhoneNumber);

            if (result.Contains("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getUserInfo/{serial}")]
        public async Task<IActionResult> GetUserInfo(int serial)
        {
            var userInfo = await _serialRepository.GetUserInfo(serial);

            if (userInfo == null)
            {
                return NotFound("Serial number not found.");
            }

            return Ok(userInfo);
        }

        [HttpPost("submitUserInfo/{serial}")]
        public async Task<IActionResult> SubmitUserInfo(int serial, [FromBody] UserInfoDto userInfoDto)
        {
            var result = await _serialRepository.SubmitUserInfo(serial, userInfoDto);

            if (!result)
            {
                return NotFound("Serial number not found.");
            }

            return Ok("User information updated successfully.");
        }

        [HttpPost("calculateOperation")]
        public IActionResult CalculateOperation([FromBody] CalculationRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = _serialRepository.CalculateOperationLevel(requestDto.PreviousKm, requestDto.CurrentKm);

            return Ok(result);
        }

        [HttpGet("qr-code-serial-status/{serial}")]
        public async Task<IActionResult> CheckSerialStatus(int serial)
        {
            // Check if the serial exists
            if (!_serialRepository.IsSerialExist(serial))
            {
                return NotFound("Serial number does not exist.");
            }

            // Call the repository method to get the status of the serial
            var status = await _serialRepository.CheckSerialStatus(serial);

            // Return a 404 NotFound if the status is null (i.e., serial number does not exist)
            if (status == null)
            {
                return NotFound("Serial number does not exist.");
            }

            // Return the product status (available or taken)
            return Ok(status);
        }







    }
}
