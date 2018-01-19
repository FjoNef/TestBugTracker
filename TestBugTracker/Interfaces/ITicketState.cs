using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Models;

namespace TestBugTracker.Interfaces
{
    public interface ITicketState
    {
        bool IsOpenable { get; }
        void UpStatus(Ticket ticket);
        void DownStatus(Ticket ticket);
    }
}
