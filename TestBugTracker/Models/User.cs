using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBugTracker.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }


        public ICollection<TicketHistory> TicketHistories { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
