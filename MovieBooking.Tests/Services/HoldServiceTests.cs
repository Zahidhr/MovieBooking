using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Application.Interfaces.Services;
using MovieBooking.Application.Options;
using MovieBooking.Application.Services;
using MovieBooking.Domain.Screenings;
using MovieBooking.UnitTests.Helpers;
using Xunit;

namespace MovieBooking.UnitTests.Services;

public sealed class HoldServiceTests
{
    private readonly FakeClock _clock =
        new(new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero));

    private readonly Mock<IHoldRepository> _holdRepo = new();
    private readonly Mock<ISeatReservationRepository> _seatRepo = new();
    private readonly Mock<IScreeningRepository> _screeningRepo = new();
    private readonly Mock<IHoldCleanupService> _cleanup = new();

    private HoldService CreateSut(int holdMinutes = 5)
    {
        var options = Options.Create(new HoldOptions
        {
            HoldDurationMinutes = holdMinutes
        });

        return new HoldService(
            _clock,
            _holdRepo.Object,
            _seatRepo.Object,
            _screeningRepo.Object,
            _cleanup.Object,
            options
        );
    }

    [Fact]
    public async Task CreateHoldAsync_WhenSeatsEmpty_ThrowsValidationException()
    {
        var sut = CreateSut();

        _screeningRepo
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = () => sut.CreateHoldAsync(
            Guid.NewGuid(),
            Array.Empty<(int, int)>(),
            CancellationToken.None);

        var ex = await Assert.ThrowsAsync<ValidationException>(act);
        ex.ErrorCode.Should().Be("EMPTY_SEAT_SELECTION");
    }

    [Fact]
    public async Task CreateHoldAsync_WhenScreeningNotFound_ThrowsNotFoundException()
    {
        var sut = CreateSut();

        _screeningRepo
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = () => sut.CreateHoldAsync(
            Guid.NewGuid(),
            new[] { (1, 1) },
            CancellationToken.None);

        var ex = await Assert.ThrowsAsync<NotFoundException>(act);
        ex.ErrorCode.Should().Be("NOT_FOUND");
    }

    [Fact]
    public async Task CreateHoldAsync_WhenSeatsUnavailable_ThrowsConflictException()
    {
        var sut = CreateSut();
        var screeningId = Guid.NewGuid();

        _screeningRepo
            .Setup(x => x.ExistsAsync(screeningId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _seatRepo
            .Setup(x => x.TryHoldSeatsAsync(
                screeningId,
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<IReadOnlyList<(int, int)>>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = () => sut.CreateHoldAsync(
            screeningId,
            new[] { (1, 1) },
            CancellationToken.None);

        var ex = await Assert.ThrowsAsync<ConflictException>(act);
        ex.ErrorCode.Should().Be("SEAT_NOT_AVAILABLE");
    }

    [Fact]
    public async Task CreateHoldAsync_WhenSuccessful_ReturnsHoldId()
    {
        var sut = CreateSut(10);
        var screeningId = Guid.NewGuid();

        _screeningRepo
            .Setup(x => x.ExistsAsync(screeningId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _seatRepo
            .Setup(x => x.TryHoldSeatsAsync(
                screeningId,
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<IReadOnlyList<(int, int)>>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var holdId = await sut.CreateHoldAsync(
            screeningId,
            new[] { (1, 1) },
            CancellationToken.None);

        holdId.Should().NotBeEmpty();
        _holdRepo.Verify(x => x.AddAsync(It.IsAny<Hold>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
