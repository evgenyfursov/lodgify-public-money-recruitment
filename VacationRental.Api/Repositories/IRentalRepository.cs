using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repositories;

public interface IRentalRepository
{ 
    List<RentalViewModel> GetAll();

    RentalViewModel GetById(int id);

    void Add(RentalViewModel model);
    
    bool IsExist(int id);
}