using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestBugTracker.Models
{
    public class User
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        public string Login { get; set; }

        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


        public ICollection<TicketHistory> TicketHistories { get; set; }

        public ICollection<Ticket> Tickets { get; set; }


        public string FullName => FirstName + " " + LastName;
    }
}
