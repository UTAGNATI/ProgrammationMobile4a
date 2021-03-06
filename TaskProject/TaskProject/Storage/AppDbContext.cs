﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Storage
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public static async Task InitAsync()
        {
            using (AppDbContext database = new AppDbContext())
            {
                await database.Database.EnsureCreatedAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = PCLStorage.FileSystem.Current.LocalStorage.Path + @"\" + String.Format("{0}.db3", "database");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}

