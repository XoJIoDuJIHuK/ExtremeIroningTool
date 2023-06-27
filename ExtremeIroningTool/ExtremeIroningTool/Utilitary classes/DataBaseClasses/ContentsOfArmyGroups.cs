﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    [Table("Contents of army groups")]
    [Index("ArmyName", Name = "unique_army_names", IsUnique = true)]
    public partial class ContentsOfArmyGroups
    {
        [Key]
        [Column("line")]
        public int Line { get; set; }
        [Required]
        [Column("army_name")]
        [StringLength(50)]
        public string ArmyName { get; set; }
        [Required]
        [Column("army_group")]
        [StringLength(50)]
        public string ArmyGroup { get; set; }

        [ForeignKey("ArmyGroup")]
        [InverseProperty("ContentsOfArmyGroups")]
        public virtual ArmyGroups ArmyGroupNavigation { get; set; }
        [ForeignKey("ArmyName")]
        [InverseProperty("ContentsOfArmyGroups")]
        public virtual Armies ArmyNameNavigation { get; set; }
    }
}