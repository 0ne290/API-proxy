namespace ApiProxy.Logic.Refactoring.Logic;

public class ConsolePhoneReader : IPhoneReader
{
    public string?[] GetInputData()
    {
        Console.WriteLine("Введите модель:");
        var model = Console.ReadLine();
        Console.WriteLine("Введите цену:");
        var price = Console.ReadLine();
        return new[] { model, price };
    }
}