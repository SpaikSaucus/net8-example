namespace CoreLibrary.Security
{
    public interface ICurrentUserAccessor
    {
        IUserInformation User { get; }
    }
}