using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceCounterBackend.Models;

namespace FinanceCounterBackend.Data
{
    public class FinanceCounterBackendContext : DbContext
    {
        public FinanceCounterBackendContext (DbContextOptions<FinanceCounterBackendContext> options)
            : base(options)
        {
        }

        public DbSet<FinanceCounterBackend.Models.Payment> Payment { get; set; }

        public DbSet<FinanceCounterBackend.Models.Category> Category { get; set; }
    }
}
