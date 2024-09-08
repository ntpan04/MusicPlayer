using MusicPlayer.DAL.Models;
using MusicPlayer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.BLL.Services
{
    public class AccountService
    {
        private AccountRepository _repo = new();
        public Account? Authenticate(string userName, string password)
        {
            return _repo.GetAccount(userName, password);
        }

        public void AddAccount(string userName, string password)
        {
            _repo.Add(userName,password);
        }

        public Account GetAccountByUserName(string userName)
        {
            return _repo.GetUserName(userName);
        }
    }
}
