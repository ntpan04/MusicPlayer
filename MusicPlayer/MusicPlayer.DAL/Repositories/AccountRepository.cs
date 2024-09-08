using MusicPlayer.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.DAL.Repositories
{
    public class AccountRepository
    {
        private MusicPlayerManagementContext _context;

        public Account? GetAccount(string userName, string password)
        {
            _context = new();
            return _context.Accounts.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower() && x.Password == password);
        }

        public void Add(string userName, string password)
        {
            _context = new();
            Account acc = new Account()
            {
                UserName = userName,
                Password = password,
            };
            _context.Accounts.Add(acc);
            _context.SaveChanges();
        }

        public Account GetUserName(String userName)
        {
            _context = new();
            return _context.Accounts.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
