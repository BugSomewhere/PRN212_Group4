using System;
using System.Collections.Generic;

namespace PRN212_Group4.DAL.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public decimal? Price { get; set; }

    public int? ProductId { get; set; }

    public int? BuyerId { get; set; }
}
