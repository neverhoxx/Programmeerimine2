using FluentValidation;
using KooliProjekt.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectCommandValidator : AbstractValidator<SaveProjectCommand>
    {
        public SaveProjectCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters")
                // Oma loogikaga valideerimise reegel
                // Siin võib kasutada DbContexti klassi
                .Custom((s, context) =>
                {
                    // Command või query, mida valideerime
                    var command = context.InstanceToValidate;

                    // Oma valideerimise loogika
                    // koos vea lisamisega
                    //var failure = new ValidationFailure();
                    //failure.AttemptedValue = command.ProjectId;
                    //failure.ErrorMessage = "Cannot find project with id " + command.ProjectId;
                    //failure.PropertyName = nameof(command.ProjectId);

                    //context.AddFailure(failure);
                });


        }
    }

}
