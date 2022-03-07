using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Contexts;

public interface IVacationRentalContext
{
    List<BookingViewModel> GetAllBooking();

    BookingViewModel GetBookingById(int bookingId);
    
    void AddBooking(BookingViewModel model);

    List<RentalViewModel> GetAllRentals();
    
    RentalViewModel GetRentalById(int rentalId);

    void AddRental(RentalViewModel model);
    
    bool IsRentalExist(int rentalId);
}