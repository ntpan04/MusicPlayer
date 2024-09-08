using System;
using System.Collections.Generic;

namespace MusicPlayer.DAL.Models;

public partial class Song
{
    public int SongId { get; set; }

    public string? SongName { get; set; }

    public string? Artist { get; set; }

    public string? FilePath { get; set; }
}
