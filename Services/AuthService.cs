﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managing.Models;

namespace Managing.Services
{
    public class AuthService : IAuthService
    {
        // For demo: hardcoded users with individual passwords
        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "adminpass", Permissions = new List<string> { "Painel", "Receção", "Produção", "Inventário", "Administração" } },
            new User { Username = "user", Password = "userpass", Permissions = new List<string> { "Painel", "Receção", "Produção" } }
        };

        public Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = _users.Find(u => u.Username == username && u.Password == password);
            return Task.FromResult<User?>(user);
        }
    }
}
