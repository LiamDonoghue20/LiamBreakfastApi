namespace LiamBreakfastApi.Contracts.Breakfast;
public record BreakfastResponse
(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime LastModifiedDateTime,
    List<string> Savoury,
    List<String> Sweet
);