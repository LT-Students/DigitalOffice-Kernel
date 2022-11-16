namespace LT.DigitalOffice.Kernel.Constants
{
  /// <summary>
  /// Rights give access to admin rights to the corresponding services or blocks
  /// Admin, Feedback, Email, create/update Roles only work with admin rights
  /// </summary>
  /// <field name="AddEditRemoveUsers">Right grants admin rights to user, family, education, skill, office users, achievement </field>
  /// <field name="AddEditRemoveProjects">Right grants admin rights to project, task services</field>
  /// <field name="AddEditRemoveEmailsTemplates">Right grants admin rights to text template service</field>
  /// <field name="AddEditRemoveDepartments">Right grants admin rights to department service, work with project department</field>
  /// <field name="AddEditRemoveNews">Right grants admin rights to news service</field>
  /// <field name="AddEditRemovePositions">Right grants admin rights to position service</field>
  /// <field name="AddEditRemoveTime">Right grants admin rights to time service</field>
  /// <field name="AddEditRemoveCompanies">Right grants admin rights to company, office services</field>
  /// <field name="AddEditRemoveCompanyData">Right grants admin rights to company service to work with company users, office workspaces/workspaceTypes, booking service</field>
  /// <field name="AddRemoveUsersRoles">Right grants admin rights to right service to work with users roles</field>
  /// <field name="AddEditRemoveWiki">Right grants admin rights to wiki service</field>
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
    public const int AddEditRemoveBookings = 10;
    public const int AddEditRemoveCompanyData = 11;
    public const int AddRemoveUsersRoles = 12;
    public const int AddEditRemoveWiki = 13;
  }
}
