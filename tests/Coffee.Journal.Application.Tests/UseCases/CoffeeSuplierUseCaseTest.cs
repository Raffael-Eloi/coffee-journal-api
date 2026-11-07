using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeSuplierUseCaseTest
{
    [Fact]
    public async Task GivenCoffeeSupplier_WhenAdd_ThenTheCoffeeSupplierShouldBeAdded()
    {
        #region Arrange

        CoffeeSupplierRequest request = new()
        {
            Name = "Unique Cafés",
            Website = "https://uniquecafes.com.br",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee supplier",
            LogoUrl = "https://logo.png"
        };

        Mock<ICoffeeSupplierRepository> repositoryMock = new Mock<ICoffeeSupplierRepository>();

        ICoffeeSuplierUseCase useCase = new CoffeeSuplierUseCase(repositoryMock.Object);

        #endregion

        #region Act

        await useCase.AddAsync(request);

        #endregion

        #region Assert

        repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeSupplier>(x => 
                x.Name == request.Name &&
                x.Website == request.Website &&
                x.Recommend == request.Recommend &&
                x.HasMembershipOption == request.HasMembershipOption &&
                x.Rate == request.Rate &&
                x.Review == request.Review &&
                x.LogoUrl == request.LogoUrl)),
            Times.Once());

        #endregion
    }
}
