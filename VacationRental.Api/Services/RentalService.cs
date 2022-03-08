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

    public ResourceIdViewModel Create(RentalBindingModel model)
    {
        var key = new ResourceIdViewModel { Id = GetAll().Count + 1 };

        _rentalRepository.Add(new RentalViewModel(key.Id, model.Units, model.PreparationTimeInDays));

        return key;
    }

    public bool IsExist(int id)
    {
        return _rentalRepository.IsExist(id);
    }
}