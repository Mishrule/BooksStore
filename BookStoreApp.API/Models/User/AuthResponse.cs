﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp.API.Models.User
{
  public class AuthResponse
  {
    public string UserId { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }

  }
}
