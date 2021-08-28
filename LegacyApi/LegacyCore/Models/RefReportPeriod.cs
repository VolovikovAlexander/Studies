using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("refReportPeriods")]
    public partial class RefReportPeriod: IModel
    {
        public RefReportPeriod()
        {
            RepCustomerHistories = new HashSet<RepCustomerHistory>();
            RepCustomerTotals = new HashSet<RepCustomerTotal>();
            TblTransactions = new HashSet<TblTransaction>();
        }

        [Key]
        public long Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Period { get; set; }
        public string Comments { get; set; }

        [InverseProperty(nameof(RepCustomerHistory.Period))]
        public virtual ICollection<RepCustomerHistory> RepCustomerHistories { get; set; }
        [InverseProperty(nameof(RepCustomerTotal.Period))]
        public virtual ICollection<RepCustomerTotal> RepCustomerTotals { get; set; }
        [InverseProperty(nameof(TblTransaction.Period))]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
