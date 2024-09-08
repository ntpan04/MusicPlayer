using System;
using System.Collections.Generic;

namespace MusicPlayer.DAL.Models;

public partial class PlayList
{
    public int PlayListId { get; set; }

    public string? PlayListName { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
