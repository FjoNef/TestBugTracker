using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Interfaces;

namespace TestBugTracker.Models.TicketStates
{
    public class TicketStateSolved : ITicketState
    {
        public bool IsOpenable => true;

        public void DownStatus(Ticket ticket)
        {
            ticket.Status = Status.Opened;
        }

        public void UpStatus(Ticket ticket)
        {
            ticket.Status = Status.Closed;
        }
    }
}
