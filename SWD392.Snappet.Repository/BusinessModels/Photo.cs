﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SWD392.Snappet.Repository.BusinessModels;

public partial class Photo
{
    public int PhotoId { get; set; }

    public int? PetId { get; set; }

    public string PhotoUrl { get; set; }

    public string Tags { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Status { get; set; }

    public virtual Pet Pet { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}