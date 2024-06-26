namespace InternetShop.Common.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistAsync(Guid id);

    ValueTask<ClientRequest?> GetAsync(Guid id);

    Task<ClientRequest> CreateRequestAsync<T>(Guid id, string? dataId = null);
}
