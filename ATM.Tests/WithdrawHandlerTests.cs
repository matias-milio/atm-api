using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.Core.Events;
using ATM.UseCases.Account.Withdraw;
using System.Linq.Expressions;
using MediatR;
using Moq;

namespace ATM.Tests
{
    public class WithdrawCommandHandlerTests
    {
        private readonly Mock<IAtmRepository<Card>> _mockCardRepository;
        private readonly Mock<IAtmRepository<Account>> _mockAccountRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly WithdrawCommandHandler _handler;

        public WithdrawCommandHandlerTests()
        {
            _mockCardRepository = new Mock<IAtmRepository<Card>>();
            _mockAccountRepository = new Mock<IAtmRepository<Account>>();
            _mockMediator = new Mock<IMediator>();
            _handler = new WithdrawCommandHandler(
                _mockCardRepository.Object,
                _mockMediator.Object,
                _mockAccountRepository.Object);
        }

        private Card CreateValidCard() => new(
            id: 1,
            number: "123456789034342",
            status: CardStatus.Active
        );

        [Fact]
        public async Task Handle_SuccessfulWithdraw_ReturnsWithdrawResponse()
        {
            var card = CreateValidCard();
            card.Account = new Account { Balance = 50000 };

            _mockCardRepository
                           .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                           .ReturnsAsync(card);

            var command = new WithdrawCommand("123456789034342", 500);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Error is null);
            var response = result.Value;
            Assert.Equal("123456789034342", response.CardNumber);
            Assert.Equal(500, response.Amount);
            Assert.Equal(49500, response.NewBalance);
            _mockAccountRepository.Verify(r => r.UpdateAsync(It.IsAny<Account>()), Times.Once);
            _mockMediator.Verify(m => m.Publish(It.IsAny<TransactionCreatedEvent>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_InsufficientFunds_ReturnsError()
        {
            var card = CreateValidCard();
            card.Account = new Account { Balance = 500 };
            _mockCardRepository
                .Setup(r => r.GetByFilterAsync(
                    It.IsAny<Expression<Func<Card, bool>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(card);
            var command = new WithdrawCommand("123456789034342", 10000);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Error is null);
            Assert.Equal(WithdrawErrors.InsuficientFunds, result.Error);
        }
    }
}
