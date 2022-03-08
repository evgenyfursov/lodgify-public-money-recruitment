using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests.UnitTests;

[Collection("Unit")]
public class CalendarServiceTests
{
    private readonly Mock<IRentalService> _rentalService;
    private readonly Mock<IBookingService> _bookingService;

    private readonly ICalendarService _calendarService;

    public CalendarServiceTests()
    {
        _rentalService = new Mock<IRentalService>();
        _bookingService = new Mock<IBookingService>();

        _calendarService = new CalendarService(_bookingService.Object, _rentalService.Object);
    }

    [Fact]
    public void GivenExistBookingsAndRental_WhenGetCalendar_ThenReturnsValidCalendar()
    {
        var rentals = new List<RentalViewModel> {new(1, 3, 2)};
        var bookings = new List<BookingViewModel>
        {
            new(1, 1, new DateTime(2022, 03, 06), 2, 1),
            new(2, 1, new DateTime(2022, 03, 05), 1, 2),
            new(3, 1, new DateTime(2022, 03, 09), 1, 2),
            new(4, 1, new DateTime(2022, 03, 06), 3, 3),
        };
        _rentalService.Setup(repository => repository.GetAll()).Returns(rentals);
        _rentalService.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(rentals.First());
        _rentalService.Setup(repository => repository.IsExist(It.IsAny<int>())).Returns(true);
        _bookingService.Setup(repository => repository.GetAll()).Returns(bookings);

        var actual = _calendarService.GetCalendar(1, new DateTime(2022, 03, 05), 6);

        actual.Should().BeEquivalentTo(GetTestCalendar());
    }

    [Fact]
    public void GivenNotExistRental_WhenGetCalendar_ThenThrowRentalNotFoundException()
    {
        _rentalService.Setup(repository => repository.IsExist(1)).Returns(false);

        Action act = () => _calendarService.GetCalendar(1, new DateTime(2022, 03, 05), 6);

        act.Should().Throw<ApplicationException>().WithMessage("Rental not found");
    }
    
    [Fact]
    public void GivenNightsLessThanZero_WhenGetCalendar_ThenThrowRentalNotFoundException()
    {
        Action act = () => _calendarService.GetCalendar(1, new DateTime(2022, 03, 05), -1);

        act.Should().Throw<ApplicationException>().WithMessage("Nights must be positive");
    }

    private CalendarViewModel GetTestCalendar()
    {
        return
            new CalendarViewModel
            {
                RentalId = 1,
                Dates = new List<CalendarDateViewModel>()
                {
                    new()
                    {
                        Date = new DateTime(2022, 03, 05),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new() {Id = 2, Unit = 2}
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    },
                    new()
                    {
                        Date = new DateTime(2022, 03, 06),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new() {Id = 1, Unit = 1},
                            new() {Id = 4, Unit = 3}
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new() {Unit = 2}
                        }
                    },
                    new()
                    {
                        Date = new DateTime(2022, 03, 07),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new() {Id = 1, Unit = 1},
                            new() {Id = 4, Unit = 3}
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new() {Unit = 2}
                        }
                    },
                    new()
                    {
                        Date = new DateTime(2022, 03, 08),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new() {Id = 4, Unit = 3}
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new() {Unit = 1}
                        }
                    },
                    new()
                    {
                        Date = new DateTime(2022, 03, 09),
                        Bookings = new List<CalendarBookingViewModel>()
                        {
                            new() {Id = 3, Unit = 2},
                        },
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new() {Unit = 1},
                            new() {Unit = 3}
                        }
                    },
                    new()
                    {
                        Date = new DateTime(2022, 03, 10),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                        {
                            new() {Unit = 2},
                            new() {Unit = 3}
                        }
                    }
                }
            };
    }
}