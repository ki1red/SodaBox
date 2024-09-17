using SodaBox.Services.Interfaces;

namespace SodaBox.Services.Classes
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _transactionSessionKey;
        private readonly string _transactionRequestKey;
        private readonly string _transactionCompleteKey;

        public int? requestSum => _httpContextAccessor.HttpContext.Session.GetInt32(_transactionRequestKey);
        public int? completeSum => _httpContextAccessor.HttpContext.Session.GetInt32(_transactionCompleteKey);

        public TransactionService(IHttpContextAccessor? httpContextAccessor = null, string? transactionSessionKey = null)
        {
            if (httpContextAccessor == null)
                _httpContextAccessor = new HttpContextAccessor();
            else
                _httpContextAccessor = httpContextAccessor;

            if (transactionSessionKey == null)
                _transactionSessionKey = Guid.NewGuid().ToString();
            else
                _transactionSessionKey = transactionSessionKey;

            _transactionRequestKey = Guid.NewGuid().ToString();
            _transactionCompleteKey = Guid.NewGuid().ToString();
        }

        public void StartTransaction(int sum)
        {
            EndTransaction();
            _httpContextAccessor.HttpContext.Session.SetString(_transactionSessionKey, "true");
            _httpContextAccessor.HttpContext.Session.SetInt32(_transactionRequestKey, sum);
        }

        public void CompleteTransaction(int sum)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(_transactionCompleteKey, sum);
        }

        public bool IsTransactionCompleted()
        {
            return _httpContextAccessor.HttpContext.Session.GetString(_transactionSessionKey) == "true";
        }

        public void EndTransaction()
        {
            _httpContextAccessor.HttpContext.Session.Remove(_transactionSessionKey);
            _httpContextAccessor.HttpContext.Session.Remove(_transactionRequestKey);
            _httpContextAccessor.HttpContext.Session.Remove(_transactionCompleteKey);
        }

        
    }

}
