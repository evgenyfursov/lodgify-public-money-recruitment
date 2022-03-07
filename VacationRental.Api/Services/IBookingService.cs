using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface IBookingService
{
    List<BookingViewModel> GetAll();

    BookingViewModel GetById(int id);
    
    ResourceIdViewModel Create(BookingBindingModel model);
}