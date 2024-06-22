namespace Otus.Homework.Notify.Domain;

public class Notification(int userId, string message)
{
    public int Id { get; private set; }
    public int UserId { get; private set; } = userId;
    public string Message { get; private set; } = message;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
}
