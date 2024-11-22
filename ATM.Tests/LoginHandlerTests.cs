using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.Core.Events;
using ATM.UseCases.CardHolder.Login;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace ATM.Tests
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IAtmRepository<Card>> _mockCardRepository;
        private readonly Mock<ICacheService> _mockCacheHelper;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<ILogger<Card>> _mockLogger;
        private readonly Mock<IMediator> _mockMediator;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _mockCardRepository = new Mock<IAtmRepository<Card>>();
            _mockCacheHelper = new Mock<ICacheService>();
            _mockJwtService = new Mock<IJwtService>();
            _mockLogger = new Mock<ILogger<Card>>();
            _mockMediator = new Mock<IMediator>();
            _handler = new LoginCommandHandler(
                _mockCardRepository.Object,
                _mockCacheHelper.Object,
                _mockJwtService.Object,
                _mockLogger.Object,
                _mockMediator.Object);
        }

        private Card CreateValidCard() => new(
            id: 1,
            number: "123456789034342",
            status: CardStatus.Active
        );

        [Fact]
        public async Task Handle_ValidLogin_GeneratesToken()
        {
            var card = CreateValidCard();
            card.CardHolderId = 123;
            card.CardHolder = new CardHolder{ Name = "Matias Gaudio", PIN = BCrypt.Net.BCrypt.HashPassword("1234")};

            _mockCardRepository
                           .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                           .ReturnsAsync(card);

            _mockJwtService
                .Setup(j => j.GenerateToken(card.CardHolderId))
                .Returns("testToken");

            var command = new LoginCommand("123456789034342", "1234");         
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result is not null);
            Assert.Equal("testToken", result.Value);
        }

        [Fact]
        public async Task Handle_IncorrectPin_BlocksCardAfterThreeAttempts()
        {
            var card = CreateValidCard();
            card.CardHolderId = 123;
            card.CardHolder = new CardHolder { Name = "Matias Gaudio Ilegal", PIN = BCrypt.Net.BCrypt.HashPassword("5667") };

            _mockCardRepository
                .Setup(r => r.GetByFilterAsync(
                    It.IsAny<Expression<Func<Card, bool>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(card);

            _mockCacheHelper
                .Setup(c => c.Get<string>(It.IsAny<string>()))
                .Returns("2");

            var command = new LoginCommand("123456789034342", "1234");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Error is null);
            Assert.Equal(LoginErrors.CardBlocked, result.Error);
            _mockMediator.Verify(m => m.Publish(It.IsAny<CardBlockedEvent>(), CancellationToken.None), Times.Once);
        }
    }
}
