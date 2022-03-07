using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface IRentalService
{
    List<RentalViewModel> GetAll();

    RentalViewModel GetById(int id);
    
    void Create(RentalViewModel model);
    
    bool IsExist(int id);
}