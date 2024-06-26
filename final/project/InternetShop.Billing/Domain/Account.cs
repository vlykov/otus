namespace InternetShop.Billing.Domain;

public class Account
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public decimal Balance { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    public Account(int userId) => UserId = userId;

    public void Deposit(decimal amount)
    {
        if (!CheckIsValidDeposit(amount))
        {
            throw new InvalidOperationException("Некорректная сумма для пополнения");
        }

        Balance += amount;
    }

    public bool CheckIsValidDeposit(decimal amount) => amount >= 0;

    public bool CheckSufficientBalance(decimal amount) => amount >= 0 && amount <= Balance;

    public void Withdraw(decimal amount)
    {
        if (!CheckSufficientBalance(amount))
        {
            throw new InvalidOperationException("Недостаточно средств на счете");
        }

        Balance -= amount;
    }
}
