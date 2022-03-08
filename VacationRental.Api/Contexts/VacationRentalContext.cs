using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Contexts;

public class VacationRentalContext : IVacationRentalContext
{
    private readonly IDictionary<int, RentalViewModel> _rentals;
    private readonly IDictionary<int, BookingViewModel> _bookings;

    public VacationRentalContext(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings)
    {
        _rentals = rentals;
        _bookings = bookings;
    }

    #region Booking
    
    public List<BookingViewModel> GetAllBooking()
    {
        return _bookings.Values.ToList();
    }

    public BookingViewModel GetBookingById(int bookingId)
    {
        if (!_bookings.ContainsKey(bookingId))
            throw new ApplicationException("Booking not found");

        return _bookings[bookingId];
    }

    public void AddBooking(BookingViewModel model)
    {
        _bookings.Add(model.Id, new BookingViewModel(model.Id, model.RentalId, model.Start.Date, model.Nights, model.Unit));
    }
    
    #endregion

    #region Rental
    
    public List<RentalViewModel> GetAllRentals()
    {
        return _rentals.Values.ToList();
    }

    public bool IsRentalExist(int rentalId)
    {
        return _rentals.ContainsKey(rentalId);
    }
    
    public RentalViewModel GetRentalById(int rentalId)
    {
        if (!_rentals.ContainsKey(rentalId))
            throw new ApplicationException("Rental not found");

        return _rentals[rentalId];
    }
    
    public void AddRental(RentalViewModel model)
    {
        _rentals.Add(model.Id, new RentalViewModel(model.Id, model.Units, model.PreparationTimeInDays));
    }
    
    #endregion
    

}