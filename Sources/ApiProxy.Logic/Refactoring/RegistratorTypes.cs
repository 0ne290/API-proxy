using ApiProxy.Logic.Refactoring.Logic;

namespace ApiProxy.Logic.Refactoring;

public class RegistratorTypes
{
    public void RegisterAllTypes()
    {
        var container = ServiceLocator.GetInstance();

        container.AddSingleton<IPhoneReader>(() => new ConsolePhoneReader())
            .AddSingleton<IPhoneBinder>(() => new GeneralPhoneBinder())
            .AddSingleton<IPhoneValidator>(() => new GeneralPhoneValidator())
            .AddSingleton<IPhoneSaver>(() => new TextPhoneSaver())
            .AddSingleton<IMobileStore>(serviceLocator => new MobileStore(serviceLocator));
    }
}