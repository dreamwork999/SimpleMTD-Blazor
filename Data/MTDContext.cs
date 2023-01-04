using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using SimplyMTD.Models.MTD;

namespace SimplyMTD.Data
{
    public partial class MTDContext : DbContext
    {
        public MTDContext()
        {
        }

        public MTDContext(DbContextOptions<MTDContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<SimplyMTD.Models.MTD.Planing> Planings { get; set; }
    }
}