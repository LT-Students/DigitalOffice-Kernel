using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalOffice.Kernel.EFSupport.Models;

public class DbCreated
{
  [Required]
  public Guid CreatedBy { get; set; }

  [Required]
  public DateTime CreatedAtUtc { get; set; }
}