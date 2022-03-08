namespace VacationRental.Api.Models;

public class RentalViewModel
{
    public RentalViewModel(int id, int units, int preparationTimeInDays)
    {
        Id = id;
        Units = units;
        PreparationTimeInDays = preparationTimeInDays;
    }

    public int Id { get; private set; }
    public int Units { get; private set; }
    public int PreparationTimeInDays { get; private set; }
}