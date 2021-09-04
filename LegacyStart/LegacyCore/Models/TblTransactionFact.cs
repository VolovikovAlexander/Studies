using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyCore.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LegacyCore.Models
{
    [Keyless]
    [Table("tblTransactionFacts")]
    public partial class TblTransactionFact
    {
        [Column(TypeName = "datetime")]
        public DateTime Period { get; set; }
        [Required]
        public string TransactionNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }
        [Required]
        [StringLength(255)]
        public string CustomerNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string ContractNumber { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public string CustomerInn { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
    }
}
