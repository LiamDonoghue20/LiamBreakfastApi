using LiamBreakfastApi.Contracts.Breakfast;
using LiamBreakfastApi.ServiceErrors;
using ErrorOr;

namespace LiamBreakfastApi.Models;

public class Breakfast
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;

    public const int MinDescriptionLength = 50;
    public const int MaxDescriptionLength = 150;

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }
    //breakfast model, all the variables a breakfast must containt
    private Breakfast(
        Guid id,
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        DateTime lastModifiedDateTime,
        List<string> savory,
        List<string> sweet)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }
    //method to create a breakfast
    public static ErrorOr<Breakfast> Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        List<string> savory,
        List<string> sweet,
        Guid? id = null)
    {
        List<Error> errors = new();
        //if the name length is too big or too small, add invalid name error to errors list
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }
        //same for description
        if (description.Length is < MinDescriptionLength or > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }   
        //return errors if there are any
        if (errors.Count > 0)
        {
            return errors;
        }
        //otherwise return the newly created breakfast entity
        return new Breakfast(
            id ?? Guid.NewGuid(),
            name,
            description,
            startDateTime,
            endDateTime,
            DateTime.UtcNow,
            savory,
            sweet);
    }

    public static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savoury,
            request.Sweet);
    }

    public static ErrorOr<Breakfast> From(Guid id, UpsertBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savoury,
            request.Sweet,
            id);
    }
}