using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("tblTransactions")]
    public partial class TblTransaction: IModel
    {
        [Key]
        public long Id { get; set; }
        public long? PeriodId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TransactionPeriod { get; set; }
        [StringLength(255)]
        public string TransactionNumber { get; set; }
        public long? CustomerId { get; set; }
        public long? AccountId { get; set; }
        public long? ContractId { get; set; }
        [StringLength(50)]
        public string OperationType { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? Amount { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(RefAccount.TblTransactions))]
        public virtual RefAccount Account { get; set; }
        [ForeignKey(nameof(ContractId))]
        [InverseProperty(nameof(RefContract.TblTransactions))]
        public virtual RefContract Contract { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(RefCustomer.TblTransactions))]
        public virtual RefCustomer Customer { get; set; }
        [ForeignKey(nameof(PeriodId))]
        [InverseProperty(nameof(RefReportPeriod.TblTransactions))]
        public virtual RefReportPeriod Period { get; set; }
    }
}
