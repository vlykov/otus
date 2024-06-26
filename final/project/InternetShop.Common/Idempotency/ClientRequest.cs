namespace InternetShop.Common.Idempotency;

public class ClientRequest(Guid id, string name, string? dataId = default)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string? DataId { get; private set; } = dataId;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
}