using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface IRentalService
{
    List<RentalViewModel> GetAll();

    RentalViewModel GetById(int id);
    
    ResourceIdViewModel Create(RentalBindingModel model);
    
    bool IsExist(int id);
}