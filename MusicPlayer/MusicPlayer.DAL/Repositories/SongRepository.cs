using Microsoft.EntityFrameworkCore;
using MusicPlayer.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.DAL.Repositories
{
    public class SongRepository
    {
        private MusicPlayerManagementContext _context;

        public List<Song> GetAll()
        {
            _context = new();
            return _context.Songs.ToList();
        }

        public Song GetSongById(int songId)
        {
            _context = new();
            return _context.Songs.Find(songId);
        }

        public List<Song> SearchSongNameOrArtist(string searchValue)
        {
            _context = new();
            return _context.Songs.Where(x => x.SongName.ToLower().Contains(searchValue.ToLower()) || x.Artist.ToLower().Contains(searchValue.ToLower())).ToList();
        }

        public PlayList GetPlayListByAccId(int accountId)
        {
            _context = new();
            return _context.PlayLists.FirstOrDefault(p => p.AccountId == accountId);
        }

        public List<Song> GetFavoritePlaylist(int accountId)
        {
            // Find the playlist associated with the account
            var playlist = GetPlayListByAccId(accountId);

            if (playlist == null)
            {
                return null;
            }

            // Get the songs in the playlist
            var songs = (from include in _context.Includes
                         join song in _context.Songs on include.SongId equals song.SongId
                         where include.PlayListId == playlist.PlayListId
                         select song).ToList();

            return songs;
        }

        public void RemoveFromPlaylist(int songId, int playlistId)
        {
            // Find the record in the Include table that links the song to the playlist
            var includeRecord = _context.Includes
                .FirstOrDefault(i => i.SongId == songId && i.PlayListId == playlistId);

            if (includeRecord == null)
            {
                throw new ArgumentException("The song is not in the specified playlist.");
            }

            // Remove the record
            _context.Database.ExecuteSqlRaw("DELETE FROM Include WHERE SongID = {0} AND PlayListID = {1};", songId, playlistId);
        }

        public void Add(Song x)
        {
            _context = new();
            _context.Songs.Add(x);
            _context.SaveChanges();
        }

        public void Update(Song x)
        {
            _context = new();
            _context.Songs.Update(x);
            _context.SaveChanges();
        }

        public void Remove(Song s)
        {
            _context = new();
            _context.Remove(s);
            _context.SaveChanges();
        }

        public void AddToFavourite(int songId, int accountId)
        {
            _context = new();

            // Get the user's playlist
            var playlist = GetPlayListByAccId(accountId);

            if (playlist == null)
            {
                // Create a new playlist if one does not exist
                _context.Database.ExecuteSqlRaw("INSERT INTO PlayList (PlayListName, AccountID) VALUES ('Favorite Songs', {0})", accountId);

                _context.SaveChanges(); // Save to get the new PlaylistID

                // Update the reference to the new playlist
                playlist = GetPlayListByAccId(accountId);
            }

            // Check if the song is already in the playlist
            var existingInclude = _context.Includes
                .FirstOrDefault(i => i.SongId == songId && i.PlayListId == playlist.PlayListId);


            // Add the song to the playlist using SQL directly
            _context.Database.ExecuteSqlRaw("INSERT INTO Include (SongID, PlayListID) VALUES ({0}, {1})", songId, playlist.PlayListId);

            // No need for _context.SaveChanges() as ExecuteSqlRaw already executes the SQL command directly
        }

        public bool checkExistName(string name)
        {
            _context = new();
            Song rusult = _context.Songs.FirstOrDefault(x => x.SongName.ToLower().Trim() == name.ToLower().Trim());
            if(rusult != null)
            {
                return true;
            }
            return false;
        }
        public bool checkExistArtist(string artist)
        {
            _context = new();
            Song rusult = _context.Songs.FirstOrDefault(x => x.Artist.ToLower().Trim() == artist.ToLower().Trim());
            if (rusult != null)
            {
                return true;
            }
            return false;
        }
    }
}