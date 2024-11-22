using System.Linq.Expressions;
using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.UseCases.Transaction.GetTransactions;
using Moq;


namespace ATM.Tests
{
    public class GetTransactionsHandlerTests
    {
        private readonly Mock<IAtmRepository<Card>> _mockCardRepository;
        private readonly Mock<IAtmRepository<Transaction>> _mockTransactionRepository;
        private readonly GetTransactionsHandler _handler;

        public GetTransactionsHandlerTests()
        {
            _mockCardRepository = new Mock<IAtmRepository<Card>>();
            _mockTransactionRepository = new Mock<IAtmRepository<Transaction>>();
            _handler = new GetTransactionsHandler(
                _mockCardRepository.Object,
                _mockTransactionRepository.Object);
        }

        private Card CreateValidCard() => new(
            id: 1,
            number: "123456789023456",
            status: CardStatus.Active
        );

        private PaginatedList<Transaction> CreateMockTransactions() => new(
            [
                new Transaction{Date= DateTime.UtcNow.AddDays(-1), Amount=  100m },
                new Transaction{Date=  DateTime.UtcNow, Amount=  200m }
            ],2,1,10);

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginatedTransactions()
        {           
            var validCard = CreateValidCard();
            var expectedTransactions = CreateMockTransactions();

            _mockCardRepository
                .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Card, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(validCard);

            _mockTransactionRepository
                .Setup(r => r.GetPaginatedAsync(It.IsAny<Expression<Func<Transaction, bool>>>(), It.IsAny<Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedTransactions);


            var command = new GetTransactionsCommand("123456789000000", 10, 1);         
            var result = await _handler.Handle(command, CancellationToken.None);
          
            Assert.True(result.Error is null);
            var response = result.Value;
            Assert.Equal(expectedTransactions.First().Amount, response.Transactions.First().Amount);
            Assert.Equal(expectedTransactions.First().Date, response.Transactions.First().TransactionDate);            
            Assert.Equal(expectedTransactions.CurrentPage, response.CurrentPage);
            Assert.Equal(expectedTransactions.TotalPages, response.TotalPages);
            Assert.Equal(expectedTransactions.PageSize, response.PageSize);
            Assert.Equal(expectedTransactions.TotalCount, response.TotalCount);
        }
    }
}