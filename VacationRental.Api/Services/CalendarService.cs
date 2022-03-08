using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public class CalendarService : ICalendarService
{
    private readonly IBookingService _bookingService;
    private readonly IRentalService _rentalService;

    public CalendarService(IBookingService bookingService, IRentalService rentalService)
    {
        _bookingService = bookingService;
        _rentalService = rentalService;
    }
    
    public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
    {
        Validate(rentalId, nights);

        var rental = _rentalService.GetById(rentalId);

        var result = new CalendarViewModel 
        {
            RentalId = rentalId,
            Dates = new List<CalendarDateViewModel>() 
        };
        for (var i = 0; i < nights; i++)
        {
            var date = new CalendarDateViewModel
            {
                Date = start.Date.AddDays(i),
                Bookings = new List<CalendarBookingViewModel>(),
                PreparationTimes = new List<CalendarPreparationTimeViewModel>()
            };

            var bookings = _bookingService.GetAll()
                .Where(booking => booking.RentalId == rentalId && booking.Start <= date.Date && booking.End > date.Date)
                .Select(booking => new CalendarBookingViewModel {Id = booking.Id, Unit = booking.Unit});
            date.Bookings.AddRange(bookings);

            var preparationTimes = _bookingService.GetAll()
                .Where(booking => booking.RentalId == rentalId
                                  && booking.End <= date.Date
                                  && booking.End.AddDays(rental.PreparationTimeInDays) > date.Date)
                .Select(booking => new CalendarPreparationTimeViewModel {Unit = booking.Unit});
            date.PreparationTimes.AddRange(preparationTimes);
            
            result.Dates.Add(date);
        }

        return result;
    }
    
    private void Validate(int rentalId, int nights)
    {
        if (nights < 0)
            throw new ApplicationException("Nights must be positive");
        
        if (!_rentalService.IsExist(rentalId))
            throw new ApplicationException("Rental not found");
    }
}