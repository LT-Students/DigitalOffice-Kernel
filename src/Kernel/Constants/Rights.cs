﻿namespace LT.DigitalOffice.Kernel.Constants;

/// <summary>
/// Rights give access to admin rights to the corresponding services or blocks
/// Admin, Feedback, Email, create/update Roles only work with admin rights
/// </summary>
/// <value name="AddEditRemoveUsers">Right grants admin rights to user, family, education, skill, office users, achievement </value>
/// <value name="AddEditRemoveProjects">Right grants admin rights to project, task services</value>
/// <value name="AddEditRemoveEmailsTemplates">Right grants admin rights to text template service</value>
/// <value name="AddEditRemoveDepartments">Right grants admin rights to department service, work with project department</value>
/// <value name="AddEditRemoveNews">Right grants admin rights to news service</value>
/// <value name="AddEditRemovePositions">Right grants admin rights to position service</value>
/// <value name="AddEditRemoveTime">Right grants admin rights to time service</value>
/// <value name="AddEditRemoveOffices">Right grants admin rights to office service</value>
/// <value name="AddEditRemoveCompanies">Right grants admin rights to company service</value>
/// <value name="AddEditRemoveBooking">Right grants admin rights to booking service</value>
/// <value name="AddEditRemoveCompanyData">Right grants admin rights to company service to work with company users</value>
/// <value name="AddRemoveUsersRoles">Right grants admin rights to right service to work with users roles</value>
/// <value name="AddEditRemoveWiki">Right grants admin rights to wiki service</value>
/// <value name="AddEditRemoveLibrary">Right grants admin rights to library service</value>
/// <value name="AddEditRemoveCompanyStructure">Right grants admin rights to company structure service</value>
/// <value name="AddEditRemoveEvents">Right grants admin rights to event service</value>
/// <value name="AddEditRemoveSurvey">Right grants admin rights to survey service</value>
/// <value name="AddEditRemoveHrProcesses">Right grants admin rights to HR processes automation</value>
/// <value name="AddEditRemoveClaims">Right allows you to edit all claims. Specify the department for claims that do not have it specified. Assign responsible persons</value>
/// <value name="AddEditRemoveClaimsResponsible">Right allows you to edit claims inside your department</value>
/// <value name="AddEditRemoveClaimsManager">Right allows you to reject or approve claims that come from departments</value>
/// <value name="AddEditRemovePrProcesses">Right grants admin rights to PR processes automation</value>
public static class Rights
{
  public const int AddEditRemoveUsers = 1;
  public const int AddEditRemoveProjects = 2;
  public const int AddEditRemoveEmailsTemplates = 3;
  public const int AddEditRemoveDepartments = 4;
  public const int AddEditRemoveNews = 5;
  public const int AddEditRemovePositions = 6;
  public const int AddEditRemoveTime = 7;
  public const int AddEditRemoveOffices = 8;
  public const int AddEditRemoveCompanies = 9;
  public const int AddEditRemoveBooking = 10;
  public const int AddEditRemoveCompanyData = 11;
  public const int AddRemoveUsersRoles = 12;
  public const int AddEditRemoveWiki = 13;
  public const int AddEditRemoveLibrary = 14;
  public const int AddEditRemoveCompanyStructure = 15;
  public const int AddEditRemoveEvents = 16;
  public const int AddEditRemoveSurvey = 17;
  public const int AddEditRemoveHrProcesses = 18;
  public const int AddEditRemoveClaims = 19;
  public const int AddEditRemoveClaimsResponsible = 20;
  public const int AddEditRemoveClaimsManager = 21;
  public const int AddEditRemovePrProcesses = 22;
}
