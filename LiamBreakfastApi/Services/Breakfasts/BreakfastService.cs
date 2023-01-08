using ErrorOr;
using LiamBreakfastApi.Models;
using LiamBreakfastApi.ServiceErrors;

namespace LiamBreakfastApi.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{   
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();
    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        _breakfasts.Add(breakfast.Id, breakfast);
        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        _breakfasts.Remove(id);
        return Result.Deleted;
    }

    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
      if(_breakfasts.TryGetValue(id, out var breakfast))
      {
        return breakfast;
      }

      return Errors.Breakfast.NotFound;
    }
    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    {
        //if the id isnt present in the breakfasts dictionary then it is a newly created breakfast
        var isNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
        _breakfasts[breakfast.Id] = breakfast;

        return new UpsertedBreakfast(isNewlyCreated);
    }
}