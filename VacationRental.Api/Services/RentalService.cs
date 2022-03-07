using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Repositories;

namespace VacationRental.Api.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;

    public RentalService(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public List<RentalViewModel> GetAll()
    {
        return _rentalRepository.GetAll();
    }

    public RentalViewModel GetById(int id)
    {
        return _rentalRepository.GetById(id);
    }

    public void Create(RentalViewModel model)
    {
        _rentalRepository.Add(model);
    }

    public bool IsExist(int id)
    {
        return _rentalRepository.IsExist(id);
    }
}