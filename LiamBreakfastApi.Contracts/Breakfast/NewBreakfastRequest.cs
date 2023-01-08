namespace LiamBreakfastApi.Contracts.Breakfast;
public record CreateBreakfastRequest 
(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savoury,
    List<String> Sweet
);