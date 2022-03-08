using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Repositories;

namespace VacationRental.Api.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;

    public BookingService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
    {
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }

    public List<BookingViewModel> GetAll()
    {
        return _bookingRepository.GetAll();
    }

    public BookingViewModel GetById(int id)
    {
        return _bookingRepository.GetById(id);
    }

    public ResourceIdViewModel Create(BookingBindingModel model)
    {
        Validate(model);

        var key = new ResourceIdViewModel {Id = _bookingRepository.GetAll().Count + 1};

        _bookingRepository.Add(new BookingViewModel(key.Id, model.RentalId, model.Start.Date, model.Nights, model.Unit));

        return key;
    }

    private void Validate(BookingBindingModel model)
    {
        if (model.Nights <= 0)
            throw new ApplicationException("Nights must be positive");

        if (!_rentalRepository.IsExist(model.RentalId))
            throw new ApplicationException("Rental not found");

        var rental = _rentalRepository.GetById(model.RentalId);

        var overlapBookingCount = _bookingRepository.GetAll()
            .Count(booking =>
                booking.RentalId == model.RentalId
                && booking.Start >= model.End.AddDays(rental.PreparationTimeInDays)
                && booking.End.AddDays(rental.PreparationTimeInDays) <= model.Start);

        if (overlapBookingCount >= rental.Units)
            throw new ApplicationException("Not available because booking limit has been reached");

        if (model.Unit <= 0 || model.Unit > rental.Units)
            throw new ApplicationException("Incorrect unit value");

        var isExistBookingWithSameUnit = _bookingRepository.GetAll()
            .Any(booking => booking.RentalId == model.RentalId && booking.Unit == model.Unit);
        
        if (isExistBookingWithSameUnit)
            throw new ApplicationException("Not available because the unit already occupied");
    }
}