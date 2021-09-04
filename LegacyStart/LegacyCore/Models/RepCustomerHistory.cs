using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("repCustomerHistory")]
    public partial class RepCustomerHistory: IModel
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? PeriodId { get; set; }
        [StringLength(50)]
        public string ReportStatus { get; set; }
        public long? RecipientId { get; set; }
        public string Xml { get; set; }
        public string Pdf { get; set; }
        public string Csv { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(RefCustomer.RepCustomerHistories))]
        public virtual RefCustomer Customer { get; set; }
        [ForeignKey(nameof(PeriodId))]
        [InverseProperty(nameof(RefReportPeriod.RepCustomerHistories))]
        public virtual RefReportPeriod Period { get; set; }
        [ForeignKey(nameof(RecipientId))]
        [InverseProperty(nameof(RefRecipient.RepCustomerHistories))]
        public virtual RefRecipient Recipient { get; set; }
    }
}
