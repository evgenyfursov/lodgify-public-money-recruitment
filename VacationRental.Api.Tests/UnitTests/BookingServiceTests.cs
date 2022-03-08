using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using VacationRental.Api.Models;
using VacationRental.Api.Repositories;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests.UnitTests;

[Collection("Unit")]
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;

    private readonly IBookingService _bookingService;

    public BookingServiceTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _rentalRepositoryMock = new Mock<IRentalRepository>();

        _bookingService = new BookingService(_bookingRepositoryMock.Object, _rentalRepositoryMock.Object);
    }

    [Fact]
    public void GivenExistBookings_WhenGetAllBookings_ThenReturnsBookingList()
    {
        var bookings = new List<BookingViewModel>
        {
            new(1, 1, new DateTime(2022, 03, 08), 2, 1),
            new(2, 1, new DateTime(2022, 03, 08), 2, 2),
        };
        _bookingRepositoryMock.Setup(repository => repository.GetAll()).Returns(bookings);

        var actual = _bookingService.GetAll();

        actual.Should().BeEquivalentTo(bookings);
    }
    
    [Fact]
    public void GivenExistsBooking_WhenGetBookingById_ThenReturnBooking()
    {
        var booking = new BookingViewModel(1, 1, new DateTime(2022, 03, 08), 2, 1);
        _bookingRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(booking);

        var actual = _bookingService.GetById(booking.Id);

        actual.Should().BeEquivalentTo(booking);
    }
    
    [Fact]
    public void GivenNotExistsBooking_WhenGetBookingById_ThenThrowBookingNotFoundException()
    {
        _bookingRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>()))
            .Throws(new ApplicationException("Booking not found"));

        Action act = () => _bookingService.GetById(0);

        act.Should().Throw<ApplicationException>().WithMessage("Booking not found");
    }
    
    [Fact]
    public void GivenExistBookings_WhenCreateBooking_ThenReturnNewBookingId()
    {
        var bookings = new List<BookingViewModel>
        {
            new(1, 1, new DateTime(2022, 03, 08), 2, 1),
            new(2, 1, new DateTime(2022, 03, 08), 2, 2),
        };
        _rentalRepositoryMock.Setup(repository => repository.IsExist(It.IsAny<int>())).Returns(true);
        _rentalRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>()))
            .Returns(new RentalViewModel(1, 3, 2));
        _bookingRepositoryMock.Setup(repository => repository.GetAll()).Returns(bookings);
        _bookingRepositoryMock.Setup(repository => repository.Add(It.IsAny<BookingViewModel>()));

        var actual = _bookingService.Create(new BookingBindingModel()
        {
            RentalId = 1,
            Nights = 2,
            Start = new DateTime(2022,03,08),
            Unit = 3
        });

        actual.Should().BeEquivalentTo(new ResourceIdViewModel {Id = 3});
    }
    
    [Fact]
    public void GivenNotExistRental_WhenCreateBooking_ThenThrowRentalNotFoundException()
    {
        _rentalRepositoryMock.Setup(repository => repository.IsExist(It.IsAny<int>())).Returns(false);

        Action act = () => _bookingService.Create(new BookingBindingModel()
        {
            RentalId = 1,
            Nights = 2,
            Start = new DateTime(2022, 03, 08),
            Unit = 3
        });

        act.Should().Throw<ApplicationException>().WithMessage("Rental not found");
    }
}