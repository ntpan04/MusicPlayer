using MusicPlayer.DAL.Models;
using MusicPlayer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.BLL.Services
{
    public class SongService
    {
        private SongRepository _repo = new SongRepository();

        public List<Song> GetAllSongs()
        {
            return _repo.GetAll();
        }

        public Song GetSongById(int id)
        {
            return _repo.GetSongById(id);
        }

        public List<Song> SearchSongNameOrArtist(string searchValue)
        {
            return _repo.SearchSongNameOrArtist(searchValue);
        }

        public List<Song> GetSongsFromFavoritePlaylist(int accountId)
        {
            return _repo.GetFavoritePlaylist(accountId);
        }


        public void RemoveSongFromPlaylist(int songId, int accountId)
        {
            PlayList playlist = _repo.GetPlayListByAccId(accountId);
            _repo.RemoveFromPlaylist(songId, playlist.PlayListId);
        }

        public void AddSong(Song song)
        {
            _repo.Add(song);
        }

        public void UpdateSong(Song song)
        {
            _repo.Update(song);
        }

        public void RemoveSong(Song s)
        {
            _repo.Remove(s);
        }

        public void AddSongToFavourite(int songId, int accountId)
        {
            _repo.AddToFavourite(songId, accountId);
        }

        public bool checkExistSongName(string name)
        {
            return _repo.checkExistName(name);
        }

        public bool checkExistSongArtist(string artist)
        {
            return _repo.checkExistArtist(artist);
        }
    }
}
