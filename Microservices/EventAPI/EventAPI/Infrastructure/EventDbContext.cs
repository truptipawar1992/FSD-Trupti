using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Infrastructure
{
    public class EventDbContext:DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options):base(options)
        {

        }

        public DbSet<EventInfo> Events { get; set; }
    }
}
