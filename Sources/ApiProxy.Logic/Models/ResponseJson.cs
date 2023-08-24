namespace ApiProxy.Logic.Models
{
    /// <summary>
    /// Универсальный ответ нашим мерчантам. Чаще всего используется в формате
    /// "Выполняемое действие - результат"
    /// </summary>
    public class ResponseJson
    {
        public object? Response { get; set; }
        public string? Message { get; set; }
    }
}