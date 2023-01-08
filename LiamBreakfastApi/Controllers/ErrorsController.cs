using Microsoft.AspNetCore.Mvc;

namespace LiamBreakfastApi.Controllers;

public class ErrorsController : ControllerBase
{
    //executes the request to the error route
    [Route("/error")]

    public IActionResult Error(){
        //uses the Problem function in the controller base to just return a catch-all 500 error
        return Problem();
    }
}
