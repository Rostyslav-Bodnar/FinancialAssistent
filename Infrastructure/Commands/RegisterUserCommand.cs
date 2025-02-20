using FinancialAssistent.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public RegisterModel model { get; }

        public RegisterUserCommand(RegisterModel model)
        {
            this.model = model;
        }
        
    }
}
