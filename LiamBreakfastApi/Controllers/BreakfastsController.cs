using LiamBreakfastApi.Contracts.Breakfast;
using LiamBreakfastApi.Models;
using LiamBreakfastApi.ServiceErrors;
using LiamBreakfastApi.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
namespace LiamBreakfastApi.Controllers
{

    public class BreakfastsController : ApiController
    {
        private readonly IBreakfastService _breakfastService;
        public BreakfastsController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }

        [HttpPost]
        public IActionResult CreateBreakfast(CreateBreakfastRequest request)
        {
            //request to create the breakfast, which will return a breakfast object or an error
            ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);

            //if it results in an error, send the errors to the problem method in the api controller class 
            if (requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);
            }
            //otherwise get the breakfast from the result
            var breakfast = requestToBreakfastResult.Value;
            ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);
            //call the create breakfast method with the breakfast from 
            //the response if successful, or errors  to problem if it isnt
            return createBreakfastResult.Match(
                created => CreatedAtGetBreakfast(breakfast),
                errors => Problem(errors));
        }
        //takes the breakfast id in the endpoint
        [HttpGet("{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {   
            //get the breakfast that matches the ID passed to here via the URL
            ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
            //map the response to a breakfast object if successful, send errors to problem if not
            return getBreakfastResult.Match(
                breakfast =>Ok(MapBreakfastResponse(breakfast)),
                errors => Problem(errors)
            );

 
        }

 
        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            //select the breakfast that matches the id passed to here
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);
        //if there is an error from the request then return it to the problem method in the apicontroller class
        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        //if its successful then take the breakfast from the value
        var breakfast = requestToBreakfastResult.Value;
        //pass to upsertbreakfast method in the breakfast service 
        ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);
        
        return upsertBreakfastResult.Match(
            //if its a new breakfast, call method to create breakfast
            //otherwise if its updating a current breakfast, return no content
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            //if return an error, pass to problem method
            errors => Problem(errors));
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            //call to delete breakfast via ID passed to controller url
           ErrorOr<Deleted> deleteBreakfastResult =  _breakfastService.DeleteBreakfast(id);
            return deleteBreakfastResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }
        //method that has been extracted to be used in both update and create to map a new breakfast (returned from a response)
        private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            return new BreakfastResponse(
                breakfast.Id,
                breakfast.Name,
                breakfast.Description,
                breakfast.StartDateTime,
                breakfast.EndDateTime,
                breakfast.LastModifiedDateTime,
                breakfast.Savory,
                breakfast.Sweet
            );
        }
        //method that has been extracted to create a new breakfast, also used in the create and update endpoints
        private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
        {   
            return CreatedAtAction(
                //the action to create the breakfast
                actionName: nameof(GetBreakfast),
                routeValues: new { id = breakfast.Id },
                value: MapBreakfastResponse(breakfast)
                );
        }
    }

}
