using ApiProxy.Logic.Refactoring;
using ApiProxy.Logic.Refactoring.Logic;
using ApiProxy.Logic.Refactoring.Models;
using AutoFixture.Xunit2;
using Moq;
using Tests.AAAPattern.xUnit.Attributes;
using Xunit;

namespace ApiProxy.Tests;

public class FirstTest
{
    //[Fact]
    //public void RealResolivingServiceLocatorTest()
    //{
    //    var serviceLocator = ServiceLocator.GetInstance();

    //    var registrator = new RegistratorTypes();
    //    registrator.RegisterAllTypes(serviceLocator);
    //    var mobileStore = serviceLocator.Resolve<IMobileStore>();

    //    mobileStore.Process();

    //    Assert.True(true);
    //}

    [Theory]
    [MoqInlineAutoData("store.txt", true)]
    [MoqInlineAutoData("store.txt", false)]
    public void AaaPatternResolivingServiceLocatorTest(string fileName, bool phoneValidValue, [Frozen] Mock<IServiceLocator> serviceLocator, 
        Mock<IPhoneReader> phoneReader, Mock<IPhoneBinder> phoneBinder, Mock<IPhoneValidator> phoneValidator,
        Mock<IPhoneSaver> phoneSaver, Mock<MobileStore> mobileStore, Phone modelPhone)
    {
        //Arrange
        var phoneData = new[] { modelPhone.Model, $"{modelPhone.Price}" };
        
        serviceLocator.Setup(e => e.Resolve<IPhoneReader>(string.Empty, true)).Returns(phoneReader.Object);
        serviceLocator.Setup(e => e.Resolve<IPhoneBinder>(string.Empty, true)).Returns(phoneBinder.Object);
        serviceLocator.Setup(e => e.Resolve<IPhoneValidator>(string.Empty, true)).Returns(phoneValidator.Object);
        serviceLocator.Setup(e => e.Resolve<IPhoneSaver>(string.Empty, true)).Returns(phoneSaver.Object);
        serviceLocator.Setup(e => e.Resolve<IMobileStore>(string.Empty, true)).Returns(mobileStore.Object);

        phoneReader.Setup(e => e.GetInputData()).Returns(phoneData);
        phoneBinder.Setup(e => e.CreatePhone(phoneData)).Returns(modelPhone);
        phoneValidator.Setup(e => e.IsValid(modelPhone)).Returns(phoneValidValue);
        if (phoneValidValue)
            phoneSaver.Setup(e => e.Save(modelPhone, fileName));

        var ms = serviceLocator.Object.Resolve<IMobileStore>();

        //Act
        ms.Process();

        //Assert
        serviceLocator.Verify(e=>e.Resolve<IPhoneReader>("", true), Times.Once);
        serviceLocator.Verify(e => e.Resolve<IPhoneBinder>("", true), Times.Once);
        serviceLocator.Verify(e => e.Resolve<IPhoneValidator>("", true), Times.Once);
        serviceLocator.Verify(e => e.Resolve<IPhoneSaver>("", true), Times.Once);
        serviceLocator.Verify(e => e.Resolve<IMobileStore>("", true), Times.Once);

        phoneReader.Verify(e => e.GetInputData(), Times.Once);
        phoneBinder.Verify(e => e.CreatePhone(phoneData), Times.Once);
        phoneValidator.Verify(e => e.IsValid(modelPhone), Times.Once);
        if (phoneValidValue)
            phoneSaver.Verify(e => e.Save(modelPhone, fileName), Times.Once);
    }
}