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
public class RentalServiceTests
{
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;

    private readonly IRentalService _rentalService;

    public RentalServiceTests()
    {
        _rentalRepositoryMock = new Mock<IRentalRepository>();
        
        _rentalService = new RentalService(_rentalRepositoryMock.Object);
    }

    [Fact]
    public void GivenExistRentals_WhenGetAllRentals_ThenReturnsRentalList()
    {
        var rentals = new List<RentalViewModel>
        {
            new(1, 2, 1),
            new(2, 3, 2),
        };
        _rentalRepositoryMock.Setup(repository => repository.GetAll()).Returns(rentals);

        var actual = _rentalService.GetAll();

        actual.Should().BeEquivalentTo(rentals);
    }
    
    [Fact]
    public void GivenExistsRental_WhenGetRentalById_ThenReturnRental()
    {
        var rental = new RentalViewModel(1, 4, 1);
        _rentalRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(rental);

        var actual = _rentalService.GetById(rental.Id);

        actual.Should().BeEquivalentTo(rental);
    }
    
    [Fact]
    public void GivenNotExistsBooking_WhenGetBookingById_ThenThrowBookingNotFoundException()
    {
        _rentalRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>()))
            .Throws(new ApplicationException("Rental not found"));

        Action act = () => _rentalService.GetById(0);

        act.Should().Throw<ApplicationException>().WithMessage("Rental not found");
    }
    
    [Fact]
    public void GivenExistRentals_WhenCreateRental_ThenReturnNewRentalId()
    {
        var rentals = new List<RentalViewModel>
        {
            new(1, 1, 1),
            new(2, 1, 2),
        };
        _rentalRepositoryMock.Setup(repository => repository.Add(It.IsAny<RentalViewModel>()));
        _rentalRepositoryMock.Setup(repository => repository.GetAll()).Returns(rentals);

        var actual = _rentalService.Create(new RentalBindingModel()
        {
            Units = 3,
            PreparationTimeInDays = 2
        });

        actual.Should().BeEquivalentTo(new ResourceIdViewModel {Id = 3});
    }
    
    [Fact]
    public void GivenExistsRental_WhenCheckIsExistsRental_ThenReturnTrue()
    {
        _rentalRepositoryMock.Setup(repository => repository.IsExist(1)).Returns(true);

        var actual = _rentalService.IsExist(1);

        actual.Should().BeTrue();
    }
    
    [Fact]
    public void GivenNotExistsRental_WhenCheckIsExistsRental_ThenReturnFalse()
    {
        _rentalRepositoryMock.Setup(repository => repository.IsExist(1)).Returns(false);

        var actual = _rentalService.IsExist(1);

        actual.Should().BeFalse();
    }
}