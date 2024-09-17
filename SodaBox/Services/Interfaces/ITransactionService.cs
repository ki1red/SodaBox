namespace SodaBox.Services.Interfaces
{
    public interface ITransactionService
    {
        int? requestSum { get; }
        int? completeSum { get; }
        // Начало транзакции, определение требуемой суммы
        void StartTransaction(int sum);

        // Подтверждение транзакции, определение уплаченной суммы
        void CompleteTransaction(int sum);

        // Проверка завершённости транзакции
        bool IsTransactionCompleted();

        // Сброс транзакции
        void EndTransaction();
    }
}
