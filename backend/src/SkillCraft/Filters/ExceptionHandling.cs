using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SkillCraft.Application;
using SkillCraft.Contracts.Errors;

namespace SkillCraft.Filters;

internal class ExceptionHandling : ExceptionFilterAttribute
{
  public override void OnException(ExceptionContext context)
  {
    if (context.Exception is ValidationException validation)
    {
      context.Result = new BadRequestObjectResult(BuildError(validation));
      context.ExceptionHandled = true;
    }
    else if (context.Exception is BadRequestException badRequest)
    {
      context.Result = new BadRequestObjectResult(badRequest.Error);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is ConflictException conflict)
    {
      context.Result = new ConflictObjectResult(conflict.Error);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is PermissionDeniedException permissionDenied)
    {
      context.Result = new JsonResult(permissionDenied.Error)
      {
        StatusCode = StatusCodes.Status403Forbidden
      };
      context.ExceptionHandled = true;
    }
    else
    {
      base.OnException(context);
    }
  }

  private static ValidationError BuildError(ValidationException exception)
  {
    PropertyError[] failures = exception.Errors
      .Select(error => new PropertyError(error.ErrorCode, error.ErrorMessage, error.PropertyName, error.AttemptedValue))
      .ToArray();
    return new ValidationError(failures);
  }
}
