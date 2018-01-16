using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBugTracker.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TrackerContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
