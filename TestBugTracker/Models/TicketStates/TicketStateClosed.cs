using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Interfaces;

namespace TestBugTracker.Models.TicketStates
{
    public class TicketStateClosed : ITicketState
    {
        public bool IsOpenable => false;

        public void DownStatus(Ticket ticket){}

        public void UpStatus(Ticket ticket){}
    }
}
