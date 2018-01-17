﻿using TestBugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace TestBugTracker.Data
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<User> Users { get; set; }                
    }
}