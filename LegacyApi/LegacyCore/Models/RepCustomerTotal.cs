using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("repCustomerTotals")]
    public partial class RepCustomerTotal: IModel
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? RecipientId { get; set; }
        public long? PeriodId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BeginRest { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DebitAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CreditAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SuspiciousAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ControlsAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? EndRest { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(RefCustomer.RepCustomerTotals))]
        public virtual RefCustomer Customer { get; set; }
        [ForeignKey(nameof(PeriodId))]
        [InverseProperty(nameof(RefReportPeriod.RepCustomerTotals))]
        public virtual RefReportPeriod Period { get; set; }
        [ForeignKey(nameof(RecipientId))]
        [InverseProperty(nameof(RefRecipient.RepCustomerTotals))]
        public virtual RefRecipient Recipient { get; set; }
    }
}
