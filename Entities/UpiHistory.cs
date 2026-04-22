using System;
using System.Collections.Generic;

namespace onilne_service.Entities;

public partial class UpiHistory
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string UpiId { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }
}
