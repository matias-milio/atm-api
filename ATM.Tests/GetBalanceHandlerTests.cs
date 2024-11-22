using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.UseCases.Account.GetBalance;
using Moq;
using System.Linq.Expressions;

namespace ATM.Tests
{
    public class GetBalanceHandlerTests
    {
        private readonly Mock<IAtmRepository<Card>> _mockCardRepository;
        private readonly GetBalanceCommandHandler _handler;

        public GetBalanceHandlerTests()
        {
            _mockCardRepository = new Mock<IAtmRepository<Card>>();
            _handler = new GetBalanceCommandHandler(_mockCardRepository.Object);
        }


        private Card CreateValidCard() => new(
            id: 1,
            number: "123456789034342",
            status: CardStatus.Active
        );

        [Fact]
        public async Task Handle_ValidCard_ReturnsBalanceSuccessfully()
        {
            
            var card = CreateValidCard();
            card.Account = new Account { Balance = 50000, Number = "02234356"};
            card.CardHolder = new CardHolder { Name = "Matias Gaudio"};
            card.Transactions =
            [
                new Transaction{Date= DateTime.UtcNow.AddDays(-1), Amount=  100 },
                new Transaction{Date=  DateTime.UtcNow, Amount=  200 }
            ];

            _mockCardRepository
                            .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                            .ReturnsAsync(card);

            var command = new GetBalanceCommand ("123456789034342");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Error is null);
            var response = result.Value;
            Assert.Equal("02234356", response.AccountNumber);
            Assert.Equal("Matias Gaudio", response.CardHolderName);
            Assert.Equal(50000, response.CurrentBalance);
            Assert.NotNull(response.LastExtractionDate);
        }

        [Fact]
        public async Task Handle_CardNotFound_ReturnsCardNotFoundError()
        {
            _mockCardRepository
                            .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                            .ReturnsAsync((Card)null);

            var command = new GetBalanceCommand("123456789034342");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Error is null);
            Assert.Equal(GenericErrors.CardNotFound, result.Error);
        }

        [Fact]
        public async Task Handle_InactiveCard_ReturnsInactiveCardError()
        {

            var card = new Card(
                id: 2,
                number: "123456789034342",
                status: CardStatus.Inactive
            );

            card.Account = new Account { Balance = 50000, Number = "02234356" };
            card.CardHolder = new CardHolder { Name = "Matias Gaudio" };
          
            _mockCardRepository
                .Setup(r => r.GetByFilterAsync(
                    It.IsAny<Expression<Func<Card, bool>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(card);

            var command = new GetBalanceCommand("123456789034342");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Error is null);
            Assert.Equal(GenericErrors.InactiveCard, result.Error);
        }
    }
}
