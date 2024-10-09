using System;

namespace DigitalOffice.Kernel.EFSupport.Models;

public class DbModified : DbCreated
{
  public Guid? ModifiedBy { get; set; }

  public DateTime? ModifiedAtUtc { get; set; }
}