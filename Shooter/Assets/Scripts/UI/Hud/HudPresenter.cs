using System;
using Shooter.Abilities;
using Shooter.Combat;

namespace Shooter.UI
{
    public sealed class HudPresenter
    {
        private readonly IHudView _view;
        private readonly HealthComponent _playerHealth;
        private readonly AbilityLoadoutComponent _abilityController;

        public HudPresenter(
            IHudView view,
            HealthComponent playerHealth,
            AbilityLoadoutComponent abilityController)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _playerHealth = playerHealth ?? throw new ArgumentNullException(nameof(playerHealth));
            _abilityController = abilityController ?? throw new ArgumentNullException(nameof(abilityController));
        }

        public void Bind()
        {
            _view.Bind(_playerHealth, _abilityController);
        }
    }
}
