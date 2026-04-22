using System;
using System.Collections.Generic;

namespace onilne_service.Entities;

public partial class BankAccountHistory
{
    public string Id { get; set; } = null!;

    public string BankAccountId { get; set; } = null!;

    public string? BankName { get; set; }

    public string? AccountNumber { get; set; }

    public string? Ifsccode { get; set; }

    public string? AccountHolderName { get; set; }

    public string? UserId { get; set; }

    public DateTime? ArchivedAt { get; set; }
}
