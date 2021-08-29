using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Table("refContracts")]
    public partial class RefContract: IModel
    {
        public RefContract()
        {
            TblTransactions = new HashSet<TblTransaction>();
        }

        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string ContractNumber { get; set; }
        public string Comments { get; set; }

        [InverseProperty(nameof(TblTransaction.Contract))]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
