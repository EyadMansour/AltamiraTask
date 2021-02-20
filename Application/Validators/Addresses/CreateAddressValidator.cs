using FluentValidation;
using Shared.Resources.Addresses;

namespace Application.Validators.Addresses
{
    public class CreateAddressValidator : AbstractValidator<AddressData>
    {
        public CreateAddressValidator()
        {
            
        }
    }
}
