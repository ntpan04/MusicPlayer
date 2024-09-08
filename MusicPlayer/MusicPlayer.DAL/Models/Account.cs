using System;
using System.Collections.Generic;

namespace MusicPlayer.DAL.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<PlayList> PlayLists { get; set; } = new List<PlayList>();
}
