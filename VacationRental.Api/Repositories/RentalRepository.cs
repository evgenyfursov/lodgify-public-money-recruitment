using System.Collections.Generic;
using VacationRental.Api.Contexts;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly IVacationRentalContext _vacationRentalContext;

    public RentalRepository(IVacationRentalContext vacationRentalContext)
    {
        _vacationRentalContext = vacationRentalContext;
    }

    public List<RentalViewModel> GetAll()
    {
        return _vacationRentalContext.GetAllRentals();
    }

    public RentalViewModel GetById(int id)
    {
        return _vacationRentalContext.GetRentalById(id);
    }

    public void Add(RentalViewModel model)
    {
        _vacationRentalContext.AddRental(model);
    }

    public bool IsExist(int id)
    {
        return _vacationRentalContext.IsRentalExist(id);
    }
}