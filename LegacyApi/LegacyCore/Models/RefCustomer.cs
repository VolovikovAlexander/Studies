using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("refCustomers")]
    public partial class RefCustomer: IModel
    {
        public RefCustomer()
        {
            RepCustomerHistories = new HashSet<RepCustomerHistory>();
            RepCustomerTotals = new HashSet<RepCustomerTotal>();
            TblTransactions = new HashSet<TblTransaction>();
        }

        [Key]
        public long Id { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        [StringLength(50)]
        public string CustomerType { get; set; }

        [InverseProperty(nameof(RepCustomerHistory.Customer))]
        public virtual ICollection<RepCustomerHistory> RepCustomerHistories { get; set; }
        [InverseProperty(nameof(RepCustomerTotal.Customer))]
        public virtual ICollection<RepCustomerTotal> RepCustomerTotals { get; set; }
        [InverseProperty(nameof(TblTransaction.Customer))]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
