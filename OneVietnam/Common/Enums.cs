namespace OneVietnam.Common
{
    public enum PostTypeEnum
    {
        Administration=10,
        Accommodation = 3,
        Job = 4,        
        Furniture = 5,
        HandGoods = 6,
        Trade = 7,        
        Sos = 8,
        Warning = 9, 
        AdminPost = 10
    }

    public enum MessageType
    {
        Send = 0,
        Receive = 1
    }
    public enum Gender
    {
        female = 0,
        male = 1,
        Other = 2
    }
    public enum SignInStatus
    {
        Success,
        Locked,
        LockedOut,
        RequiresConfirmingEmail,
        RequiresTwoFactorAuthentication,
        Failure
    }

    public enum ReportStatus
    {
        Open,
        Pending,
        Closed,
        Canceled
    }
}