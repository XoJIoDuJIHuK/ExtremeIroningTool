﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    public partial class TerrainTypes
    {
        [Key]
        [Column("terrain_type")]
        [StringLength(10)]
        public string TerrainType { get; set; }
        [Column("attack_modifier")]
        public float? AttackModifier { get; set; }
        [Column("combat_width")]
        public byte? CombatWidth { get; set; }
    }
}