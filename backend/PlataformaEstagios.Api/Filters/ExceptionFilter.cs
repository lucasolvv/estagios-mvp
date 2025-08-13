using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlataformaEstagios.Exceptions.ExceptionBase;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is AppBaseException appBaseException)
                HandleProjectException(appBaseException, context);
            else
                ThrowUnknowException(context);

        }
        private static void HandleProjectException(AppBaseException myRecipeBookException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages()));
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(context.Exception.Message));
        }

    }
}
