using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATM.UseCases.CardHolder.Login
{
    public class LoginCommandHandler(IAtmRepository<Card> cardRepository, ICacheService cacheHelper, IJwtService jwtService, ILogger<Card> logger, IMediator _mediator)
        : IRequestHandler<LoginCommand, Result<string, Error>>
    {
        private readonly IAtmRepository<Card> _cardRepository = cardRepository;
        private readonly ICacheService _cacheHelper = cacheHelper;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILogger<Card> _logger = logger;

        public async Task<Result<string, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByFilterAsync(filter: c => c.Number.Equals(request.CardNumber),
                includeProperties: "CardHolder");

            if (card == null)
                return GenericErrors.CardNotFound;

            if (card.Status.Equals(CardStatus.Inactive))
                return GenericErrors.InactiveCard;

            if (!IsPinValid(request.PIN, card.CardHolder.PIN))
            {
                var cachekey = $"cacheEntry_{"Login"}_{card.CardHolder.Name}_{card.Id}";
                int tries = Convert.ToInt32(_cacheHelper.Get<string>(cachekey)) + 1;
                _cacheHelper.Set(cachekey, tries.ToString());
                if (tries >= 3)
                {
                    await _mediator.Publish(new CardBlockedEvent(card.Id), cancellationToken);
                    _logger.LogError($"Blocked card with CardId: {card.Id}", card);
                    return LoginErrors.CardBlocked;
                }
                else
                {
                    _logger.LogError($"Wrong PIN attempt with CardId: {card.Id}", card);
                    return LoginErrors.IncorrectPIN;
                }
            }
            return _jwtService.GenerateToken(card.CardHolderId);
        }            
        private bool IsPinValid(string requestPin, string actualPin) => BCrypt.Net.BCrypt.Verify(requestPin, actualPin);       
               
    }
}
