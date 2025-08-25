namespace PlataformaEstagios.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        Task<Domain.Entities.User?> GetByEmailOrNicknameAsync(string emailOrNickname, CancellationToken ct);
        Task<bool> VerifyUserExistsByEmailOrUsername(string emailOrNickname, CancellationToken ct);

    }
}
