namespace Loan2022.Application.Interfaces.Shared
{
    public interface IAuthenticatedUserService
    {
        string? UserId { get; }
        public string Username { get; }
    }
}