﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore.Models;
using NetCore_01.Models;
using System;

namespace NetCore.Data
{
    public class BlogContext : IdentityDbContext<IdentityUser>
    {
        public BlogContext()
        {

        }

        public BlogContext(DbContextOptions<BlogContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=il_mio_primo_blog_auth;Integrated Security=True");
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
