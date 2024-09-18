using MechanicProject.DTOs.UserViewModels;
using MechanicProject.Models;

namespace MechanicProject.Services.Interface
{
    public interface ISerialRepository
    {

        bool IsSerialExist(int serial);
        string CheckAndUpdatePhoneNumber(int serial, string phoneNumber);
        Task<UserInfoDto> GetUserInfo(int serial);
        Task<bool> SubmitUserInfo(int serial, UserInfoDto userInfoDto);
        Task<Sm?> GetRecordBySerialAsync(int serial);
        OperationStatusDto CalculateOperationLevel(int previousKm, int currentKm);
        Task<QRCODESerialRequestDto> CheckSerialStatus(int serial);

    }
}
