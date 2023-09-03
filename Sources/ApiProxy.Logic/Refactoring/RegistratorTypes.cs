using ApiProxy.Logic.Refactoring.Logic;
using Tests.AAAPattern.xUnit.Refactoring.Logic;

namespace ApiProxy.Logic.Refactoring;

public class RegistratorTypes
{
    public void RegisterAllTypes()
    {
        var container = Container.GetInstance();

        container.Add<IPhoneReader, ConsolePhoneReader>(new ConsolePhoneReader())
            .Add<IPhoneBinder, GeneralPhoneBinder>(new GeneralPhoneBinder())
            .Add<IPhoneValidator, GeneralPhoneValidator>(new GeneralPhoneValidator())
            .Add<IPhoneSaver, TextPhoneSaver>(new TextPhoneSaver())
            .Add<IMobileStore, MobileStore>(new MobileStore(container));
    }
}