using System.Collections.Generic;
using VacationRental.Api.Contexts;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IVacationRentalContext _vacationRentalContext;

    public BookingRepository(IVacationRentalContext vacationRentalContext)
    {
        _vacationRentalContext = vacationRentalContext;
    }

    public List<BookingViewModel> GetAll()
    {
        return _vacationRentalContext.GetAllBooking();
    }

    public void Add(BookingViewModel model)
    {
        _vacationRentalContext.AddBooking(model);
    }

    public BookingViewModel GetById(int id)
    {
        return _vacationRentalContext.GetBookingById(id);
    }
}