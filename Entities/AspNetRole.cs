using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace onilne_service.Entities;

public partial class AspNetRole : IdentityRole<string>
{
    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
