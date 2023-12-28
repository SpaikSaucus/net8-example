namespace CoreLibrary.Security
{
    public interface IUserInformation
    {
        string Guid { get; }
        string UserName { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
    }
}
