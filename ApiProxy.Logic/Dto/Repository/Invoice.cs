namespace ApiProxy.Logic.Dto.Repository;

public class Invoice
{
    public Invoice(int id, string? merchantGuid, string? status, string? fiatName, double fiatAmount)
    {
        Id = id;
        MerchantGuid = merchantGuid;
        Status = status;
        FiatName = fiatName;
        FiatAmount = fiatAmount;
    }

    public int Id { get; set; }
    public string? MerchantGuid { get; set; }
    public string? Status { get; set; }
    public string? FiatName { get; set; }
    public double FiatAmount { get; set; }
}