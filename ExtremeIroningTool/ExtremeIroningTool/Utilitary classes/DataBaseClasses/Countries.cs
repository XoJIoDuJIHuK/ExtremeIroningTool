﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    public partial class Countries
    {
        public Countries()
        {
            Generals = new HashSet<Generals>();
        }

        [Key]
        [Column("country_tag")]
        [StringLength(3)]
        public string CountryTag { get; set; }
        [Required]
        [Column("country_name")]
        [StringLength(50)]
        public string CountryName { get; set; }
        [Required]
        [Column("description")]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [Column("path_to_icon")]
        [StringLength(100)]
        public string PathToIcon { get; set; }
        [Required]
        [Column("path_to_poster")]
        [StringLength(100)]
        public string PathToPoster { get; set; }

        [InverseProperty("CountryNavigation")]
        public virtual ICollection<Generals> Generals { get; set; }
    }
}