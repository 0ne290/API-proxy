using ApiProxy.Logic.Refactoring.Logic;

namespace ApiProxy.Logic.Refactoring;

public class RegistratorTypes
{
    public void RegisterAllTypes()
    {
        var container = ServiceLocator.GetInstance();

        container.Add<IPhoneReader, ConsolePhoneReader>()
            .Add<IPhoneBinder, GeneralPhoneBinder>()
            .Add<IPhoneValidator, GeneralPhoneValidator>()
            .Add<IPhoneSaver, TextPhoneSaver>()
            .Add<IMobileStore, MobileStore>(new MobileStore(container));
    }
}