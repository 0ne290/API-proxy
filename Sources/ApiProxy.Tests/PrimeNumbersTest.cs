using ApiProxy.Logic.ClearCode;
using ApiProxy.Logic.ClearCode.Interfaces;
using ApiProxy.Logic.Refactoring.Logic;
using AutoFixture.Xunit2;
using Xunit;

namespace ApiProxy.Tests
{
    public class PrimeNumbersTest
    {
        [Theory]
        [InlineAutoData]
        public void NewCodeExecute_Test(RegistratorTypes registratorTypes, string[] args)
        {
            //arrange
            registratorTypes.RegisterAllTypes(ServiceLocator.Current);
            var programm = ServiceLocator.Current.Resolve<INewCode>();

            //act
            programm.Execute(args);
        }
    }
}
