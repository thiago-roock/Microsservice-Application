using FluentValidation;
using MediatR;

namespace Microsservice.Domain.Validations
{
    public class MicrosserviceValidator : AbstractValidator<Unit>
    {
        public MicrosserviceValidator()
        {
            
        }
    }
}