using System;
using System.Collections.Generic;

namespace onilne_service.Entities;

public partial class BankAccount
{
    public string Id { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string Ifsccode { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
