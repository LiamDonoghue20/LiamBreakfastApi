namespace LiamBreakfastApi.Contracts.Breakfast;
public record UpsertBreakfastRequest 
(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savoury,
    List<String> Sweet
);