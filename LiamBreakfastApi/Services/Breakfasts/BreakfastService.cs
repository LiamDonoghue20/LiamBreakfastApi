using ErrorOr;
using LiamBreakfastApi.Models;
using LiamBreakfastApi.ServiceErrors;

namespace LiamBreakfastApi.Services.Breakfasts;
public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();
    //create breakfast methods which adds the new breakfast to the dictonary and returns the created result
    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        _breakfasts.Add(breakfast.Id, breakfast);

        return Result.Created;
    }
    //the same for deleted
    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        _breakfasts.Remove(id);

        return Result.Deleted;
    }
    //get existing breakfast
    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (_breakfasts.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }

        return Errors.Breakfast.NotFound;
    }


    //update breakfast
    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    {
        //if the breakfast id doesnt exist already in the breakfast dictionary, set isNewlyCreated as true
        var isNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
        _breakfasts[breakfast.Id] = breakfast;
    
        return new UpsertedBreakfast(isNewlyCreated);
    }
}