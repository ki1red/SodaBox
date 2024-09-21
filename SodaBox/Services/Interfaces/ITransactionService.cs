namespace SodaBox.Services.Interfaces
{
    public interface ITransactionService
    {
        bool isStart { get; }
        bool isComplete { get; }
        int? requestSum { get; }
        int? completeSum { get; }
        // Начало транзакции, определение требуемой суммы
        void StartTransaction(int totalAmount);

        // Подтверждение транзакции, определение уплаченной суммы
        void CompleteTransaction(int priceAmount);

        // Сброс транзакции
        void RemoveTransaction();
    }
}
