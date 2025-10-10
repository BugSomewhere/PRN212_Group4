using System;
using System.Collections.Generic;

namespace PRN212_Group4.DAL.Entities;

public partial class TransactionDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? Type { get; set; }

    public decimal? Amount { get; set; }
}
