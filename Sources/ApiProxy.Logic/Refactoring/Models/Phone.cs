namespace ApiProxy.Logic.Refactoring.Models;

public class Phone
{
    public Phone(string model, int price)
    {
        Model = model;
        Price = price;
    }

    public string Model { get; }
    public int Price { get; }
}