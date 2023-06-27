﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    [Table("Contents of armies")]
    public partial class ContentsOfArmies
    {
        [Key]
        [Column("line")]
        public int Line { get; set; }
        [Required]
        [Column("army_name")]
        [StringLength(50)]
        public string ArmyName { get; set; }
        [Required]
        [Column("division_name")]
        [StringLength(50)]
        public string DivisionName { get; set; }
        [Column("division_count")]
        public int DivisionCount { get; set; }

        [ForeignKey("ArmyName")]
        [InverseProperty("ContentsOfArmies")]
        public virtual Armies ArmyNameNavigation { get; set; }
        [ForeignKey("DivisionName")]
        [InverseProperty("ContentsOfArmies")]
        public virtual Divisions DivisionNameNavigation { get; set; }
    }
}