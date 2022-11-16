namespace LT.DigitalOffice.Kernel.Constants
{
  /// <summary>
  /// Rights give access to admin rights to the corresponding services or blocks
  /// Admin, Feedback, Email, create/update Roles only work with admin rights
  /// </summary>
  /// <param name="AddEditRemoveUsers">Right grants admin rights to user, family, education, skill, office users, achievement </param>
  /// <param name="AddEditRemoveProjects">Right grants admin rights to project, task services</param>
  /// <param name="AddEditRemoveEmailsTemplates">Right grants admin rights to text template service</param>
  /// <param name="AddEditRemoveDepartments">Right grants admin rights to department service, work with project department</param> 
  /// <param name="AddEditRemoveNews">Right grants admin rights to news service</param> 
  /// <param name="AddEditRemovePositions">Right grants admin rights to position service</param> 
  /// <param name="AddEditRemoveTime">Right grants admin rights to time service</param> 
  /// <param name="AddEditRemoveCompanies">Right grants admin rights to company, office services</param> 
  /// <param name="AddEditRemoveCompanyData">Right grants admin rights to company service to work with company users, office workspaces/workspaceTypes, booking service</param> 
  /// <param name="AddRemoveUsersRoles">Right grants admin rights to right service to work with users roles</param> 
  /// <param name="AddEditRemoveWiki">Right grants admin rights to wiki service</param>
  public static class Rights
  {
    public const int AddEditRemoveUsers = 1;
    public const int AddEditRemoveProjects = 2;
    public const int AddEditRemoveEmailsTemplates = 3;
    public const int AddEditRemoveDepartments = 4;
    public const int AddEditRemoveNews = 5;
    public const int AddEditRemovePositions = 6;
    public const int AddEditRemoveTime = 7;
    public const int AddEditRemoveHistories = 8; // can be reused
    public const int AddEditRemoveCompanies = 9;
    public const int AddRemoveDepartmentData = 10; // reuse the int, we do not need the right 
    public const int AddEditRemoveCompanyData = 11;
    public const int AddRemoveUsersRoles = 12;
    public const int AddEditRemoveWiki = 13;
  }
}
