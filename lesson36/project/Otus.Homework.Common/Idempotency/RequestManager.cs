using Microsoft.EntityFrameworkCore;

namespace Otus.Homework.Common.Idempotency;

public class RequestManager<TContext> : IRequestManager where TContext : DbContext
{
    private readonly TContext _context;

    public RequestManager(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await GetAsync(id);

        return request != null;
    }

    public ValueTask<ClientRequest?> GetAsync(Guid id)
    {
        return _context.FindAsync<ClientRequest>(id);
    }

    public async Task<ClientRequest> CreateRequestAsync<T>(Guid id, string? dataId = null)
    {
        var exists = await ExistAsync(id);

        var request = exists 
            ? throw new InvalidOperationException($"Запрос с id '{id}' уже существует") 
            : new ClientRequest(id, typeof(T).Name, dataId);

        return request;
    }
}