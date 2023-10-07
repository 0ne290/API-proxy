

using ApiProxy.Logic.Boundaries;

namespace ApiProxy.Logic
{
    public class Accounting
    {
        private readonly IRepository _repository;
		
		public Accounting(IServiceLocator serviceLocator)
        {
            _repository = serviceLocator.Resolve<IRepository>();
        }
		
        public void Registration(string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(redirectUrl) || string.IsNullOrEmpty(callbackUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute) || !Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            if (!_repository.ValidateLoginAndPassword(login, password))
				throw new Exception("Enter another username or password");

            var merchant = new Dto.Repository.Merchant() { Login = login, Password = password, RedirectUrl = redirectUrl, CallbackUrl = callbackUrl };
            _repository.AddMerchant(merchant);
        }
        public Dto.Repository.Merchant GetMerchant(string? login = null, string? password = null)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw new Exception("Set correct values for required parameters");

            var merchant = _repository.FindMerchant(login, password);

            if (merchant == null)
                throw new Exception("Wrong login or password");

            return merchant;
        }
        public Dto.Repository.Merchant GetMerchant(string id)
        {
            var merchant = _repository.FindMerchant(id);

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            return merchant;
        }
        public void SetRedirect(string? redirectUrl, string id)
        {
            if (string.IsNullOrEmpty(redirectUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            var merchant = _repository.FindMerchant(id);

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            merchant.RedirectUrl = redirectUrl;
        }
        public void SetCallback(string? callbackUrl, string id)
        {
            if (string.IsNullOrEmpty(callbackUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            var merchant = _repository.FindMerchant(id);

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            merchant.CallbackUrl = callbackUrl;
        }
    }
}
