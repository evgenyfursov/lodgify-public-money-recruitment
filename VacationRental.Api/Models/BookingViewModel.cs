using System;

namespace VacationRental.Api.Models;

public class BookingViewModel
{
    public BookingViewModel(int id, int rentalId, DateTime start, int nights, int unit)
    {
        Id = id;
        RentalId = rentalId;
        Start = start;
        Nights = nights;
        Unit = unit;
    }

    public int Id { get; private set; }
    public int RentalId { get; private set; }
    public DateTime Start { get; private set; }
    public int Nights { get; private set; }
    public int Unit { get; private set; }

    public DateTime End => Start.AddDays(Nights);
}