using FluentValidation;
using Shared.Resources.Addresses;

namespace Application.Validators.Addresses
{
    public class UpdateAddressValidator : AbstractValidator<AddressData>
    {
        public UpdateAddressValidator()
        {
            
        }
    }
}
