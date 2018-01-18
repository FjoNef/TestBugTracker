using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Interfaces;

namespace TestBugTracker.Models.TicketStatuses
{
    public class TicketStatusSolved : ITicketStatus
    {
        public bool IsOpenable => true;

        public void DownStatus(Ticket ticket)
        {
            ticket.Status = Status.Opened;
            ticket.TicketStatus=new TicketStatusOpen();
        }

        public void UpStatus(Ticket ticket)
        {
            ticket.Status = Status.Closed;
            ticket.TicketStatus = new TicketStatusClosed();
        }
    }
}
