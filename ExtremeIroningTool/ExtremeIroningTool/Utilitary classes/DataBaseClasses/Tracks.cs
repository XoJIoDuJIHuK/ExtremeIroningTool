﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    public partial class Tracks
    {
        [Key]
        [StringLength(100)]
        public string TrackName { get; set; }
        [StringLength(200)]
        public string TrackPath { get; set; }
    }
}