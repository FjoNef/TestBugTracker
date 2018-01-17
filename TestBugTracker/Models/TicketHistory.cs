using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBugTracker.Models
{
    public enum Action
    {
        Creation, Opening, Solving, Closing
    }

    public class TicketHistory
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public int TicketID { get; set; }

        public DateTime DateOfChange { get; set; }

        public string Comment { get; set; }

        public Action Action { get; set; }

        
        public User User { get; set; }

        public Ticket Ticket { get; set; }
    }
}
