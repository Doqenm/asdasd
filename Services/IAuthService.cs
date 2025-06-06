﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managing.Models;

namespace Managing.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
