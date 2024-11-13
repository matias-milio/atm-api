using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.Core.Events;
using MediatR;

namespace ATM.UseCases.CardHolder.Login
{
    public class LoginCommandHandler(IAtmRepository<Card> cardRepository, ICacheService cacheHelper, IJwtService jwtService, IMediator _mediator)
        : IRequestHandler<LoginCommand, Result<string, Error>>
    {
        private readonly IAtmRepository<Card> _cardRepository = cardRepository;
        private readonly ICacheService _cacheHelper = cacheHelper;
        private readonly IJwtService _jwtService = jwtService;
        public async Task<Result<string, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByFilterAsync(filter: c => c.Number.Equals(request.CardNumber),
                includeProperties: "CardHolder");

            if (card == null)
                return LoginErrors.CardNotFound;

            if (card.Status.Equals(CardStatus.Inactive))
                return LoginErrors.InactiveCard;

            if (!IsPinValid(request.PIN,card.CardHolder.PIN))
            {
                var cachekey = $"cacheEntry_{"Login"}_{card.CardHolder.Name}_{card.Id}";
                int tries = Convert.ToInt32(_cacheHelper.Get<string>(cachekey));
                if (tries >= 3)
                {
                    await _mediator.Publish(new CardBlockedEvent(card.Id), cancellationToken);
                    return LoginErrors.CardBlocked;
                }
                else
                {
                    return LoginErrors.IncorrectPIN;
                }
            }
            return _jwtService.GenerateToken(card.CardHolderId);
        }            
        private bool IsPinValid(string requestPin, string actualPin) => BCrypt.Net.BCrypt.Verify(requestPin, actualPin);       
               
    }
}
