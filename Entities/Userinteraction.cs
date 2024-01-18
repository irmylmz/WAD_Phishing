using System;
using System.Collections.Generic;

namespace Phishing_Platform_Midterm.Entities;

public partial class Userinteraction
{
    public int Interactionid { get; set; }

    public int? Emailid { get; set; }

    public int? Userid { get; set; }

    public string Interactiontype { get; set; } = null!;

    public string? Interactiondetail { get; set; }

    public DateTime? Interactiontime { get; set; }

    public virtual Sentemail? Email { get; set; }

    public virtual User? User { get; set; }
}
