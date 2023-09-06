using ApiProxy.Logic.Refactoring;
using ApiProxy.Logic.Refactoring.Logic;
using Xunit;

namespace ApiProxy.Tests;

public class FirstTest
{
    [Fact]
    public void ResolivingServiceLocatorTest()
    {
        var serviceLocator = ServiceLocator.GetInstance();

        var registrator = new RegistratorTypes();
        registrator.RegisterAllTypes(serviceLocator);
        var mobileStore = serviceLocator.Resolve<IMobileStore>();

        mobileStore.Process();

        Assert.True(true);
    }
}