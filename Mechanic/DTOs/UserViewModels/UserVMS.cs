namespace MechanicProject.DTOs.UserViewModels
{
    public class SerialRequestDto
    {
        public int Serial { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

    }
    
    
    public class UserInfoDto
    {
        public int Serial { get; set; }
        public int? KM { get; set; }
        public string? calcu { get; set; }
        public string? Sellernum { get; set; }
        public int? EngineId { get; set; }
        public string? Codes { get; set; }
        public string? CarType { get; set; }
        public string? Type { get; set; }
        public string? msg { get; set; }

    }


    public class OperationStatusDto
    {
        public int KilometersDriven { get; set; }
        public bool IsReplacementNeeded { get; set; }
    }


    public class CalculationRequestDto
    {
        public int PreviousKm { get; set; }
        public int CurrentKm { get; set; }
    }

      public class QRCODESerialRequestDto
      {
     public int Serial { get; set; }
     public string? Codes { get; set; }
     public string? CarType { get; set; }
     public string? Type { get; set; }
     public string? msg { get; set; }
       
      }

    public class PhoneNumberRequest
    {
        public string PhoneNumber { get; set; }  // User's phone number
    }


}
