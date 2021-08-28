using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("refRecipients")]
    public partial class RefRecipient: IModel
    {
        public RefRecipient()
        {
            RepCustomerHistories = new HashSet<RepCustomerHistory>();
            RepCustomerTotals = new HashSet<RepCustomerTotal>();
        }

        [Key]
        public long Id { get; set; }
        public string Descriptions { get; set; }
        public string Comments { get; set; }

        [InverseProperty(nameof(RepCustomerHistory.Recipient))]
        public virtual ICollection<RepCustomerHistory> RepCustomerHistories { get; set; }
        [InverseProperty(nameof(RepCustomerTotal.Recipient))]
        public virtual ICollection<RepCustomerTotal> RepCustomerTotals { get; set; }
    }
}
