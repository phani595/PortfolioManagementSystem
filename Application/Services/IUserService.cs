namespace Application.Services
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}
