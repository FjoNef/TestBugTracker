using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Interfaces;
using TestBugTracker.Models.TicketStatuses;

namespace TestBugTracker.Models
{
    public enum Status
    {
        New, Opened, Solved, Closed
    }

    public enum Urgency
    {
        [Display(Name = "Very High")]
        VeryHigh,
        High,
        Medium,
        Low
    }

    public enum Criticality
    {
        Fatal,
        Critical,
        Uncritical,
        [Display(Name = "Change Request")]
        ChangeRequest
    }

    public class Ticket
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public DateTime DateCreation { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public Status Status { get; set; }

        public Urgency Urgency { get; set; }

        public Criticality Criticality { get; set; }

        [NotMapped]
        public ITicketStatus TicketStatus { get; set; }


        public User User { get; set; }

        public ICollection<TicketHistory> TicketHistories { get; set; }

        public Ticket()
        {
            switch (Status)
            {
                case Status.New:
                    TicketStatus = new TicketSatusNew();
                    break;
                case Status.Opened:
                    TicketStatus = new TicketStatusOpen();
                    break;
                case Status.Solved:
                    TicketStatus = new TicketStatusSolved();
                    break;
                case Status.Closed:
                    TicketStatus = new TicketStatusClosed();
                    break;
            }
        }

        public void UpStatus()
        {
            TicketStatus.UpStatus(this);
        }

        public void DownStatus()
        {
            TicketStatus.DownStatus(this);
        }
    }
}
