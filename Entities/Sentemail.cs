using System;
using System.Collections.Generic;

namespace Phishing_Platform_Midterm.Entities;

public partial class Sentemail
{
    public int Emailid { get; set; }

    public int Templateid { get; set; }

    public int Targetid { get; set; }

    public DateTime? Sentat { get; set; }

    public bool? Isclicked { get; set; }

    public DateTime? Clickedat { get; set; }

    public virtual Targetemail? Target { get; set; }

    public virtual Phishingtemplate? Template { get; set; }

    public virtual ICollection<Userinteraction> Userinteractions { get; set; } = new List<Userinteraction>();
}
