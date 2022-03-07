using System;
using System.Collections.Generic;
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
        if (model.Nights <= 0)
            throw new ApplicationException("Nigts must be positive");
        ;
        if (!_rentalRepository.IsExist(model.RentalId))
            throw new ApplicationException("Rental not found");
        
        for (var i = 0; i < model.Nights; i++)
        {
            var count = 0;
            foreach (var booking in _bookingRepository.GetAll())
            {
                if (booking.RentalId == model.RentalId
                    && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                    || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                    || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                {
                    count++;
                }
            }
            if (count >= _rentalRepository.GetById(model.RentalId).Units)
                throw new ApplicationException("Not available");
        }
        
        var key = new ResourceIdViewModel { Id = _bookingRepository.GetAll().Count + 1 };

        _bookingRepository.Add(new BookingViewModel
        {
            Id = key.Id,
            Nights = model.Nights,
            RentalId = model.RentalId,
            Start = model.Start.Date
        });

        return key;
    }
}