using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestBugTracker.Interfaces;
using TestBugTracker.Models.TicketStates;

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

        [Display(Name ="Date Creation")]
        public DateTime DateCreation { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [StringLength(450)]
        [Display(Name = "Detailed Description")]
        public string DetailedDescription { get; set; }

        public Status Status
        {
            get
            {
                if (TicketStatus is TicketStateOpen)
                {
                    return Status.Opened;
                }
                if (TicketStatus is TicketStateSolved)
                {
                    return Status.Solved;
                }
                if (TicketStatus is TicketStateClosed)
                {
                    return Status.Closed;
                }
                return Status.New;
            }
            set
            {
                switch (value)
                {
                    case Status.New:
                        TicketStatus = new TicketStateNew();
                        break;
                    case Status.Opened:
                        TicketStatus = new TicketStateOpen();
                        break;
                    case Status.Solved:
                        TicketStatus = new TicketStateSolved();
                        break;
                    case Status.Closed:
                        TicketStatus = new TicketStateClosed();
                        break;
                }
            }
        }

        public Urgency Urgency { get; set; }

        public Criticality Criticality { get; set; }

        [NotMapped]
        public ITicketState TicketStatus { get; set; }


        public User User { get; set; }

        public ICollection<TicketHistory> TicketHistories { get; set; }


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
