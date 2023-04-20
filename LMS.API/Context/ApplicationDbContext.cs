﻿using System;
using LMS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Context
{
    public class ApplicationDbContext : IdentityDbContext<User,Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions option)
            :base(option) { }
    }
}
