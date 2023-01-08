using ErrorOr;

namespace LiamBreakfastApi.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Breakfast name must be between {Models.Breakfast.MinNameLength} " + 
            $"to {Models.Breakfast.MaxNameLength} characters long"
        );
        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Breakfast descrption must be between {Models.Breakfast.MinDescriptionLength} " + 
            $"to {Models.Breakfast.MaxDescriptionLength} characters long"
        );
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast Not Found"
        );
    }
}