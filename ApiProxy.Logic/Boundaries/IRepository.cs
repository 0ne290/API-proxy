namespace ApiProxy.Logic.Boundaries
{
    public interface IRepository
    {
		Dto.Repository.Merchant? FindMerchant(string? guid);
        Dto.Repository.Merchant? FindMerchant(string? login, string? password);
		
		void AddInvoice(Dto.Repository.Invoice invoice);
		void AddMerchant(Dto.Repository.Merchant merchant);
		
		void SetRedirectUrl(string? id, string? redirectUrl);
		void SetCallbackUrl(string? id, string? callbackUrl);

        bool ValidateLoginAndPassword(string? login, string? password);
    }
}