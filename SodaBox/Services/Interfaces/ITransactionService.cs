namespace SodaBox.Services.Interfaces
{
    public interface ITransactionService
    {
        void CompleteTransaction();  // Завершение транзакции
        bool IsTransactionCompleted();  // Проверка, завершена ли транзакция
        void ResetTransaction();  // Сброс состояния транзакции
    }
}
