using SodaBox.Services.Interfaces;

namespace SodaBox.Services.Classes
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartSessionKey;

        public TransactionService(IHttpContextAccessor? httpContextAccessor = null, string? cartSessionKey = null)
        {
            if (httpContextAccessor == null)
                _httpContextAccessor = new HttpContextAccessor();
            else
                _httpContextAccessor = httpContextAccessor;

            if (cartSessionKey == null)
                _cartSessionKey = Guid.NewGuid().ToString();
            else
                _cartSessionKey = cartSessionKey;
        }

        public void CompleteTransaction()
        {
            _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, "true");
        }

        public bool IsTransactionCompleted()
        {
            return _httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey) == "true";
        }

        public void ResetTransaction()
        {
            _httpContextAccessor.HttpContext.Session.Remove(_cartSessionKey);
        }
    }

}
