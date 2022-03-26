using EntitySystem;
using Unstable.Entities;
using Uxt;

namespace CardSystem
{
    /// <summary>
    /// Handles player pressing the A B X Y buttons
    /// </summary>
    public class CardButtonHandler : StandardComponent, IPrototypeComponent
    {
        private readonly PlayerInputHandler _inputHandler;
        private readonly ICardUserComponent _cardUser;
        private readonly DependencyBag _cardUseDependencies;

        public CardButtonHandler(
            PlayerInputHandler inputHandler,
            ICardUserComponent cardUser,
            DependencyBag cardUseDependencies)
        {
            _inputHandler = inputHandler;
            _cardUser = cardUser;
            _cardUseDependencies = cardUseDependencies;
            
            _inputHandler.onSouthButton += UseSouthCard;
            _inputHandler.onEastButton += UseEastCard;
            _inputHandler.onNorthButton += UseNorthCard;
            _inputHandler.onWestButton += UseWestCard;
        }

        public void Tick(float deltaTime)
        {
        }

        public override void Destroy()
        {
            base.Destroy();
            _inputHandler.onSouthButton -= UseSouthCard;
            _inputHandler.onEastButton -= UseEastCard;
            _inputHandler.onNorthButton -= UseNorthCard;
            _inputHandler.onWestButton -= UseWestCard;
        }

        private void UseSouthCard() => _cardUser.UseCard(0, _cardUseDependencies);

        private void UseEastCard() => _cardUser.UseCard(1, _cardUseDependencies);

        private void UseNorthCard() => _cardUser.UseCard(2, _cardUseDependencies);

        private void UseWestCard() => _cardUser.UseCard(3, _cardUseDependencies);
    }
}