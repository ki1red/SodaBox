using SodaBox.DataAccess.IRepositories;
using SodaBox.Services.Interfaces;

namespace SodaBox.Services.Classes
{
    public class TransactionService : ITransactionService
    {
        private readonly ICartService _cartService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _transactionSessionKey;

        private readonly string _transactionRequestKey;
        private readonly string _transactionCompleteKey;

        public bool isStart => requestSum.HasValue;
        public bool isComplete => completeSum.HasValue;
        public int? requestSum => _httpContextAccessor.HttpContext.Session.GetInt32(_transactionRequestKey);
        public int? completeSum => _httpContextAccessor.HttpContext.Session.GetInt32(_transactionCompleteKey);

        public TransactionService(ICartService cartService)
        {
            _cartService = cartService;

            _httpContextAccessor = new HttpContextAccessor();
            _transactionSessionKey = Guid.NewGuid().ToString();

            _transactionRequestKey = Guid.NewGuid().ToString();
            _transactionCompleteKey = Guid.NewGuid().ToString();
        }

        public void StartTransaction(int totalAmount)
        {
            _httpContextAccessor.HttpContext.Session.SetString(_transactionSessionKey, "true");
            _httpContextAccessor.HttpContext.Session.SetInt32(_transactionRequestKey, totalAmount);
        }

        public void CompleteTransaction(int priceAmount)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(_transactionCompleteKey, priceAmount);
        }

        public void RemoveTransaction()
        {
            _httpContextAccessor.HttpContext.Session.Remove(_transactionSessionKey);
            _httpContextAccessor.HttpContext.Session.Remove(_transactionRequestKey);
            _httpContextAccessor.HttpContext.Session.Remove(_transactionCompleteKey);
        }

        
    }

}
