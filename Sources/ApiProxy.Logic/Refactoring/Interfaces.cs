﻿using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring;

public interface IServiceLocator
{
    IServiceLocator Add<TInterface, TImplementation>(string key = "") where TImplementation : TInterface, new();
    IServiceLocator Add<TInterface, TImplementation>(TImplementation implementation, string key = "") where TImplementation : TInterface;
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