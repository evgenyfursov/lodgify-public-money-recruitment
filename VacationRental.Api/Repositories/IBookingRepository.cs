using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repositories;

public interface IBookingRepository
{
    List<BookingViewModel> GetAll();

    BookingViewModel GetById(int id);
    
    void Add(BookingViewModel model);
}