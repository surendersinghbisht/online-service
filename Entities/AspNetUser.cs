using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace onilne_service.Entities;

public partial class AspNetUser : IdentityUser<string>
{
    public string NormalizedCustomEmail { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
