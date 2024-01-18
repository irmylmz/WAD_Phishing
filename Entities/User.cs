using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Phishing_Platform_Midterm.Entities;

public partial class User 
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Sourcepage { get; set; } = null!;

    public DateTime? RegistrationDate { get; set; } = null!;
    
    public int nCount = 0, uCount = 0, iCount = 0;

    public virtual ICollection<Userinteraction> Userinteractions { get; set; } = new List<Userinteraction>();
    
}
