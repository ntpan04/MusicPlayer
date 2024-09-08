using System;
using System.Collections.Generic;

namespace MusicPlayer.DAL.Models;

public partial class Include
{
    public int? SongId { get; set; }

    public int? PlayListId { get; set; }

    public virtual PlayList? PlayList { get; set; }

    public virtual Song? Song { get; set; }
}
