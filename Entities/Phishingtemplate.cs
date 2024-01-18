using System;
using System.Collections.Generic;

namespace Phishing_Platform_Midterm.Entities;

public partial class Phishingtemplate
{
    public int Id { get; set; }

    public string Templatemail { get; set; } = null!;

    public string Content { get; set; } = string.Empty;

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Sentemail> Sentemails { get; set; } = new List<Sentemail>();
}
