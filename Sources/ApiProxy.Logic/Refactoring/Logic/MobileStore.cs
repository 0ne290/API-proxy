using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring.Logic;

public class MobileStore: IMobileStore
{
    public MobileStore(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
        Phones = new List<Phone>();
    }

    public void Process()
    {
        var reader = ServiceLocator.Resolve<IPhoneReader>();
        var binder = ServiceLocator.Resolve<IPhoneBinder>();
        var validator = ServiceLocator.Resolve<IPhoneValidator>();
        var saver = ServiceLocator.Resolve<IPhoneSaver>();

        var data = reader.GetInputData();
        var phone = binder.CreatePhone(data!);
        
        if (!validator.IsValid(phone))
        {
            Console.WriteLine("Некорректные данные");
            return;
        }

        Phones.Add(phone);
        saver.Save(phone, "store.txt");
        
        Console.WriteLine("Данные успешно обработаны");
    }

    private List<Phone> Phones { get; }
    private IServiceLocator ServiceLocator { get; }

}