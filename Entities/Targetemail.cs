using System;
using System.Collections.Generic;

namespace Phishing_Platform_Midterm.Entities;

public partial class Targetemail
{
    public int Id { get; set; }

    public string Targetemail1 { get; set; } = null!;

    public virtual ICollection<Sentemail> Sentemails { get; set; } = new List<Sentemail>();
}
