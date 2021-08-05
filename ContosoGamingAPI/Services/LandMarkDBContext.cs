using ContosoGamingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI.Services
{
    public class LandMarkDBContext : DbContext
    {
        public DbSet<LandMark> LandMarks { get; set; }
        public DbSet<RouteConnection> RouteConnections { get; set; }

        public LandMarkDBContext(DbContextOptions<LandMarkDBContext> options)
            : base(options)
        {

        }
    }
}
