using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiProxy.Logic.Database
{
    /// <summary>
    /// Инвойс с информацией о его мерчанте
    /// </summary>
    public class Invoice
    {
        [Key]
        public string? InvoiceId { get; set; }
        public string? MerchantGuid { get; set; }
        public string? InvoiceStatus { get; set; }
        public string? InvoiceFiat { get; set; }
        public string? InvoiceFiatAmount { get; set; }
    }
}
