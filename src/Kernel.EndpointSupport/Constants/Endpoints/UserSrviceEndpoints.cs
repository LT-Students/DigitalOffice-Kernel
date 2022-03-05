using System;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Constants.Endpoints
{
  static public class UserSrviceEndpoints
  {
    public const string AvatarCreateSt = "9abbc3ae4e854c9cbffb874d0c1f2453";
    public static Guid AvatarCreate = new Guid(AvatarCreateSt);

    public const string AvatarRemoveSt = "c9983021eba84b1cade9150ce7f5ecaf";
    public static Guid AvatarRemove = new Guid(AvatarRemoveSt);

    public const string AvatarEditCurrentSt = "04e3bfa4e63741908a8019ff238cb755";
    public static Guid AvatarEditCurrent = new Guid(AvatarEditCurrentSt);

    public const string CommunicationCreateSt = "c8b4df63813249b3a3a4ed74151cf72b";
    public static Guid CommunicationCreate = new Guid(CommunicationCreateSt);

    public const string CommunicationConfirmSt = "a3d751a68b3449e4ad6e0bccf0251e7e";
    public static Guid CommunicationConfirm = new Guid(CommunicationConfirmSt);

    public const string PasswordForgotSt = "c1a20e6283e84b578f13855fd2af32d9";
    public static Guid PasswordForgot = new Guid(PasswordForgotSt);

    public const string PasswordChangeSt = "f84fb9991b5445d0aafb6bd9abfac225";
    public static Guid PasswordChange = new Guid(PasswordChangeSt);

    public const string UserCreateSt = "ec5ed4ab28584b23978482b77244dffe";
    public static Guid UserCreate = new Guid(UserCreateSt);
    
    public const string UserEditSt = "2a3c774c9b8b46b589edf493cf831c71";
    public static Guid UserEdit = new Guid(UserEditSt);

    public const string UserEditActiveSt = "5f56534eb5dd425ea36ad7daf92f9bc7";
    public static Guid UserEditActive = new Guid(UserEditActiveSt);
  }
}
