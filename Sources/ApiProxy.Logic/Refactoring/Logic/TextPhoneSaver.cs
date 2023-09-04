using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring.Logic;

public class TextPhoneSaver : IPhoneSaver
{
    public void Save(Phone phone, string fileName)
    {
        using var writer = new StreamWriter(fileName, true);
        writer.WriteLine(phone.Model);
        writer.WriteLine(phone.Price);
    }
}