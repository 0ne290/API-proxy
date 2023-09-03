using ApiProxy.Logic.Refactoring.Models;

namespace ApiProxy.Logic.Refactoring.Logic;

public class GeneralPhoneValidator : IPhoneValidator
{
    public bool IsValid(Phone phone) =>
        !string.IsNullOrEmpty(phone.Model) && phone.Price > 0;
}