using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LiamBreakfastApi.Controllers;
//creating our own base controller the other api controllers can inherit from
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    //Problem action result which takes the list of errors as parameters
    protected IActionResult Problem(List <Error> errors)
    {
        //if the only errors we have are validation errors
        if(errors.All(e => e.Type == ErrorType.Validation))
        {   
            var modelStateDictionary = new ModelStateDictionary();
            //iterate through each error in the list
            foreach(var error in errors)
            {
                //add all errors to the model state dictionary
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
            //then return all the validation errors in the model state dictionary
            return ValidationProblem(modelStateDictionary);
        }
        //if any of the errors are a 400 unexpected error, return a seperate error because it means we cant trust any other errors
        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            //return the problem function in the controller base class
            return Problem();
        }
        //otherwise return the first error
        var firstError = errors[0];
        //when we get a list of errors from our controllers that are being called
        //use switch statement on the type of the error and return the appropriate response
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        //return the error code selected from the switch and the error description
        return Problem(statusCode: statusCode, title: firstError.Description);
    }
    

}
