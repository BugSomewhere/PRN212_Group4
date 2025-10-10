using System;
using System.Collections.Generic;

namespace PRN212_Group4.DAL.Entities;

public partial class Feedback
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string? Comment { get; set; }
}
