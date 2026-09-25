using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException myRecipeBookException) //o .Net converte system.exception em ErrorOnValidationException caso a condicao seja true e coloca o resultado nessa variavel no final
        {
            context.HttpContext.Response.StatusCode = (int)myRecipeBookException.getStatusCode();

            context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.getErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
