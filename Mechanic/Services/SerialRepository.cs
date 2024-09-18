using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using MechanicProject.DTOs.UserViewModels;
using MechanicProject.Services.Interface;
using MechanicProject.Models;
using Microsoft.EntityFrameworkCore;



public class SerialRepository : ISerialRepository
{
    private const int ReplacementThreshold = 60000; // Example threshold in kilometers
    private readonly AppDbContext _context;
   

    public SerialRepository(AppDbContext context)
    {
        _context = context;
    }

    public OperationStatusDto CalculateOperationLevel(int previousKm, int currentKm)
    {
        int kilometersDriven = currentKm - previousKm;

        return new OperationStatusDto
        {
            KilometersDriven = kilometersDriven,
            IsReplacementNeeded = kilometersDriven >= ReplacementThreshold
        };
    }

    public string CheckAndUpdatePhoneNumber(int serial, string phoneNumber)
    {
        var record = _context.Sms.FirstOrDefault(s => s.Serial == serial);

        if (record == null)
        {
            return "Serial number does not exist.";
        }

        if (!string.IsNullOrEmpty(record.User))
        {
            if (record.User == phoneNumber)
            {
                return "OK: Phone number matches.";
            }
            else
            {
                return "Error: This serial number is already registered with a different phone number.";
            }
        }
        else
        {
            record.User = phoneNumber; // Add the phone number
            _context.SaveChanges(); // Save the changes to the database
            return "Phone number has been added.";
        }
    }

    public async Task<QRCODESerialRequestDto> CheckSerialStatus(int serial)
    {
        var smRecord = await GetRecordBySerialAsync(serial);

        if (smRecord == null)
        {
            return null; // Serial number does not exist
        }

        var response = new QRCODESerialRequestDto
        {
            Codes = smRecord.Codes,
            Type = smRecord.Type,
            CarType = smRecord.CarType,

        };

        response.msg = string.IsNullOrEmpty(smRecord.User)
            ? "این محصول موجود هست"
            : "این محصول خریداری شده";

        return response;
    }

   

    public async Task<Sm?> GetRecordBySerialAsync(int serial)
    {
        return await _context.Sms.FirstOrDefaultAsync(s => s.Serial == serial);
    }

    public async Task<UserInfoDto> GetUserInfo(int serial)
    {
        var smRecord = await  GetRecordBySerialAsync(serial);

        if (smRecord == null)
        {
            return null;
        }

        return new UserInfoDto
        {   
            EngineId = smRecord.EngineId,
            KM = smRecord.Km,   
            Sellernum = smRecord.SellerNum,
            Codes = smRecord.Codes,
            Type = smRecord.Type,
            CarType = smRecord.CarType
            
        };
    }

   

    public bool IsSerialExist(int serial)
    {
        return _context.Sms.Any(s => s.Serial == serial);
    }


    public async Task<bool> SubmitUserInfo(int serial, UserInfoDto userInfoDto)
    {
        var smRecord = await GetRecordBySerialAsync(serial); ;

        if (smRecord == null)
        {
            return false;
        }

        smRecord.EngineId = userInfoDto.EngineId;
        smRecord.Km = userInfoDto.KM;
        smRecord.SellerNum = userInfoDto.Sellernum;

        await _context.SaveChangesAsync();

        return true;
    }
}

