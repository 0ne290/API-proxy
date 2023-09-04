using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring.Logic;

public class MobileStore: IMobileStore
{
    public MobileStore(IContainer container)
    {
        Container = container;
        Phones = new List<Phone?>();
    }

    public void Process()
    {

        var reader = Container.Resolve<IPhoneReader>();
        var binder = Container.Resolve<IPhoneBinder>();
        var validator = Container.Resolve<IPhoneValidator>();
        var saver = Container.Resolve<IPhoneSaver>();

        var data = reader!.GetInputData();
        var phone = binder!.CreatePhone(data!);
        
        if (validator!.IsValid(phone!))
        {
            Console.WriteLine("Некорректные данные");
            return;
        }

        Phones.Add(phone);
        saver!.Save(phone!, "store.txt");
        
        Console.WriteLine("Данные успешно обработаны");
    }

    private List<Phone?> Phones { get; }
    private IContainer Container { get; }

}