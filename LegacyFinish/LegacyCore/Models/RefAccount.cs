using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("refAccounts")]
    public partial class RefAccount: IModel
    {
        public RefAccount()
        {
            TblTransactions = new HashSet<TblTransaction>();
        }

        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string AccountType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? LimitAmount { get; set; }
        public string Comments { get; set; }

        [InverseProperty(nameof(TblTransaction.Account))]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
