using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring;

public interface IContainer
{
    IContainer Add<TInterface, TImplementation>(TImplementation implementation) where TImplementation: TInterface;
    TInterface Resolve<TInterface>();
}

public interface IMobileStore
{
    void Process();
}

public interface IPhoneReader
{
    string[] GetInputData();
}

public interface IPhoneBinder
{
    Phone CreatePhone(string[] data);
}

public interface IPhoneValidator
{
    bool IsValid(Phone phone);
}

public interface IPhoneSaver
{
    void Save(Phone phone, string fileName);
}