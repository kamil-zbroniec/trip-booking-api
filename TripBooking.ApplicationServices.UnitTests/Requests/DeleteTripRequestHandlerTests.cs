namespace TripBooking.ApplicationServices.UnitTests.Requests;

using ApplicationServices.Requests;
using Domain.Entities;
using Domain.Repositories;
using Moq;

public class DeleteTripRequestHandlerTests
{
    [Fact]
    public async Task Delete_WhenTripNotExists_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TripEntity?)null);
        
        var sut = new DeleteTripRequestHandler(repository.Object);

        var request = new DeleteTripRequest("name");

        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Delete(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Delete_WhenTripExists_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity());
        
        var sut = new DeleteTripRequestHandler(repository.Object);

        var request = new DeleteTripRequest("name");

        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Delete(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}