﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    public partial class Battalions
    {
        public Battalions()
        {
            ContentsOfDivisions = new HashSet<ContentsOfDivisions>();
        }

        [Key]
        [Column("battalion_name")]
        [StringLength(50)]
        public string BattalionName { get; set; }
        [Column("health")]
        public float Health { get; set; }
        [Column("organization")]
        public float Organization { get; set; }
        [Column("soft_attack")]
        public float SoftAttack { get; set; }
        [Column("hard_attack")]
        public float HardAttack { get; set; }
        [Column("defence")]
        public float Defence { get; set; }
        [Column("breakthrough")]
        public float Breakthrough { get; set; }
        [Column("armor")]
        public float Armor { get; set; }
        [Column("piercing")]
        public float Piercing { get; set; }
        [Column("front_width")]
        public byte FrontWidth { get; set; }
        [Required]
        [Column("path_to_icon")]
        [StringLength(200)]
        public string PathToIcon { get; set; }
        [Column("vehicle_ratio")]
        public float VehicleRatio { get; set; }
        [Column("type")]
        [StringLength(3)]
        public string Type { get; set; }

        [InverseProperty("BattalionNameNavigation")]
        public virtual UnitSpecificAttackAdjusters UnitSpecificAttackAdjusters { get; set; }
        [InverseProperty("BattalionNameNavigation")]
        public virtual UnitSpecificDefenseAdjusters UnitSpecificDefenseAdjusters { get; set; }
        [InverseProperty("BattalionNameNavigation")]
        public virtual ICollection<ContentsOfDivisions> ContentsOfDivisions { get; set; }
    }
}