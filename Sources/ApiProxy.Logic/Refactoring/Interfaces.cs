using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring;

public interface IServiceLocator:IDisposable
{
    IServiceLocator Add<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "");
    IServiceLocator Add<TInterface>(Func<TInterface> functor, string key = "");
    IServiceLocator AddSingleton<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "");
    IServiceLocator AddSingleton<TInterface>(Func<TInterface> functor, string key = "");
    TInterface Resolve<TInterface>(string key = "");
}

public interface IMobileStore
{
    void Process();
}

public interface IPhoneReader
{
    string?[] GetInputData();
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