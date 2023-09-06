using ApiProxy.Logic.Refactoring.Logic;

namespace ApiProxy.Logic.Refactoring;

public class RegistratorTypes
{
    public void RegisterAllTypes(IServiceLocator serviceLocator)
    {
        serviceLocator.AddSingleton<IPhoneReader>(() => new ConsolePhoneReader())
            .AddSingleton<IPhoneBinder>(() => new GeneralPhoneBinder())
            .AddSingleton<IPhoneValidator>(() => new GeneralPhoneValidator())
            .AddSingleton<IPhoneSaver>(() => new TextPhoneSaver())
            .AddSingleton<IMobileStore>(sl => new MobileStore(sl));
    }
}