using System;
using System.Collections.Generic;

namespace MechanicProject.Models;

public partial class Sm
{
    public int Serial { get; set; }

    public string User { get; set; } = null!;

    public DateTime? Time { get; set; }

    public int Id { get; set; }

    public int? Km { get; set; }

    public int? EngineId { get; set; }

    public string? SellerNum { get; set; }

    public string? CarType { get; set; }

    public string? Type { get; set; }

    public string? Codes { get; set; }
}
