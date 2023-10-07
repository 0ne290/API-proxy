using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PublicApi.Models
{
    public class DatabaseContext
    {
        /// <summary>
        /// Класс аккаунта мерчанта в БД
        /// </summary>
        public class Merchant
        {
            private string? _merchantLogin, _merchantPassword;
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            /// <summary>
            /// Счетчик и альтернативный ключ записи
            /// </summary>
            public int MerchantId { get; private set; }
            [Key]
            /// <summary>
            /// GUID записи
            /// </summary>
            public string? MerchantGuid { get; private set; }
            /// <summary>
            /// Дата регистрации записи
            /// </summary>
            public string? MerchantDate { get; private set; }
            [Required]
            /// <summary>
            /// Логин записи. Используется для входа
            /// </summary>
            /// <remarks>
            /// В БД хранится Sha256 хеш логина. Также для хеша используется динамическая соль,
            /// состоящая из GUID записи и ее даты регистрации. Можно было бы использовать и
            /// поле "счетчик", но оно задается только в момент создания записи уже в самой БД.
            /// Решение - генерировать счетчик программно самому, но это лишняя головная боль
            /// и понизится отказоустойчивость
            /// </remarks>
            public string? MerchantLogin
            {
                get
                {
                    return _merchantLogin;
                }
                set
                {
                    _merchantLogin = Tools.ComputeSha256Hash(value + MerchantGuid + MerchantDate);
                }
            }
            [Required]
            /// <summary>
            /// Пароль записи. Используется для входа
            /// </summary>
            /// <remarks>
            /// В БД хранится Sha256 хеш пароля. Также для хеша используется динамическая соль,
            /// состоящая из GUID записи и ее даты регистрации. Можно было бы использовать и
            /// поле "счетчик", но оно задается только в момент создания записи уже в самой БД.
            /// Решение - генерировать счетчик программно самому, но это лишняя головная боль
            /// и понизится отказоустойчивость
            /// </remarks>
            public string? MerchantPassword
            {
                get
                {
                    return _merchantPassword;
                }
                set
                {
                    _merchantPassword = Tools.ComputeSha256Hash(value + MerchantGuid + MerchantDate);
                }
            }
            /// <summary>
            /// Адрес мерчанта, на который будут перенаправляться клиенты, совершившие покупку
            /// </summary>
            public string? MerchantRedirectUrl { get; set; }
            /// <summary>
            /// Адрес мерчанта, обрабатывающий коллбэки о смене статуса инвойса
            /// </summary>
            public string? MerchantCallbackUrl { get; set; }
            /// <summary>
            /// Конструктор записи - вызывается Entity Framework'ом в момент "отображения"
            /// записей из БД на объекты
            /// </summary>
            public Merchant(int merchantId, string? merchantGuid, string? merchantDate, string? merchantLogin, string? merchantPassword, string? merchantRedirectUrl, string? merchantCallbackUrl)
            {
                MerchantId = merchantId;
                MerchantGuid = merchantGuid;
                MerchantDate = merchantDate;
                MerchantLogin = merchantLogin;
                MerchantPassword = merchantPassword;
                MerchantRedirectUrl = merchantRedirectUrl;
                MerchantCallbackUrl = merchantCallbackUrl;
            }

            /// <summary>
            /// Конструктор записи - вызывается только из кода в момент создания новой записи
            /// для регистрации ее в БД
            /// </summary>
            public Merchant()
            {
                MerchantGuid = Guid.NewGuid().ToString();
                MerchantDate = DateTime.UtcNow.ToString();
            }
        }
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
        public class MyDbContext : DbContext
        {
            public DbSet<Merchant> Merchants { get; set; } = null!;
            public DbSet<Invoice> Invoices { get; set; } = null!;
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Merchant>().HasAlternateKey(m => m.MerchantId);
            }
            public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
            {
                Database.EnsureCreated();
            }
        }
    }
}
