using System;
using Character;
using Services.Character;
using UniRx;
using UnityEngine;

namespace Services.Interaction
{
    /// <summary>
    /// This is a slightly bigger class from a 2D action platformer that handles player's interactions with the world.
    /// It showcases reactive programming and a possible approach to making interactions.
    /// </summary>
    public class InteractionService : PlayerCharacterServiceBase, ICharacterActivator
    {
        public IReadOnlyReactiveCollection<PlayerInteractionData> PlayerInteractions => _playerInteractions;
        public IReadOnlyReactiveCollection<Interaction> Interactions => _interactions;

        private readonly ReactiveCollection<Interaction> _interactions = new();
        private readonly ReactiveCollection<PlayerInteractionData> _playerInteractions = new();

        public InteractionService(PlayerCharacterService playerCharacterService) : base(playerCharacterService)
        {
            _disposables.Add(Observable.EveryUpdate().Subscribe(_ => Update()));
        }

        public void AddInteraction(Interaction interaction)
        {
            _interactions.Add(interaction);
        }

        bool ICharacterActivator.IsCharacterActive(PlayerCharacter character)
        {
            foreach (var data in _playerInteractions)
            {
                if (data.ActiveInteraction.Value != null && data.ActiveInteraction.Value.State.Value == InteractionState.InProgress
                    && data.ActiveInteraction.Value.InteractingCharacter == data.Character)
                    return false;
            }
            return true;
        }

        protected override void OnCharacterAdded(Player.Player player, PlayerCharacter character) => AddCharacter(character);

        protected override void OnCharacterRemoved(Player.Player player, PlayerCharacter character) => RemoveCharacter(character);

        protected override void OnCharacterActivated(PlayerCharacter character, bool active)
        {
            if (active)
                AddCharacter(character);
            else
                RemoveCharacter(character);
        }

        private void Update()
        {
            foreach (var data in _playerInteractions)
            {
                var minDistance = float.MaxValue;
                var activeInteraction = default(Interaction);
                var count = _interactions.Count;
                for (var i = 0; i < count; i++)
                {
                    var interaction = _interactions[i];
                    if (interaction.State.Value == InteractionState.Finished)
                    {
                        if (interaction.IsDisposable)
                        {
                            interaction.Dispose();
                            _interactions.RemoveAt(i--);
                            --count;
                        }
                        continue;
                    }

                    var sqrDistance = (data.Character.State.Position - interaction.Position).sqrMagnitude;
                    if (sqrDistance <= interaction.Radius * interaction.Radius && sqrDistance < minDistance)
                    {
                        minDistance = sqrDistance;
                        activeInteraction = interaction;
                    }
                }
                data.SetActiveInteraction(activeInteraction);
            }
        }

        private void AddCharacter(PlayerCharacter character)
        {
            _playerInteractions.Add(new PlayerInteractionData(character));
        }

        private void RemoveCharacter(PlayerCharacter character)
        {
            for (int i = 0; i < _playerInteractions.Count; i++)
            {
                if (_playerInteractions[i].Character != character)
                    continue;

                _playerInteractions[i].ClearInteractions();
                _playerInteractions[i].Dispose();
                _playerInteractions.RemoveAt(i);
                return;
            }
        }

        public class PlayerInteractionData : IDisposable
        {
            public readonly PlayerCharacter Character;
            public IReadOnlyReactiveProperty<Interaction> ActiveInteraction => _activeInteraction;

            private readonly ReactiveProperty<Interaction> _activeInteraction = new();

            private readonly IDisposable _startInteraction;
            private IDisposable _interactionFinished;
            private IDisposable _cancelInteraction;
            public PlayerInteractionData(PlayerCharacter character)
            {
                Character = character;
                _startInteraction = character.Player.InputProvider.InteractStart.Subscribe(_ => OnStartInteraction());
            }

            public void SetActiveInteraction(Interaction interaction)
            {
                if (_activeInteraction.Value == interaction)
                    return;

                _activeInteraction.Value?.RemoveCharacter(Character);

                _activeInteraction.Value = interaction;
                interaction?.AddCharacter(Character);
            }

            public void ClearInteractions()
            {
                _activeInteraction.Value?.RemoveCharacter(Character);
                _activeInteraction.Value = null;
            }

            private void OnStartInteraction()
            {
                if (_activeInteraction.Value == null || _activeInteraction.Value.State.Value is InteractionState.InProgress or InteractionState.Finished)
                    return;

                _activeInteraction.Value.Start(Character);
                if (_activeInteraction.Value.Duration > Mathf.Epsilon)
                {
                    _cancelInteraction = Character.Player.InputProvider.InteractCancel.Subscribe(_ => OnCancelInteraction());
                    _interactionFinished = _activeInteraction.Value.State.Subscribe(state =>
                    {
                        if (state == InteractionState.InProgress)
                            return;

                        _interactionFinished?.Dispose();
                        _cancelInteraction?.Dispose();
                    });
                }
            }

            private void OnCancelInteraction()
            {
                _activeInteraction.Value.Cancel();
                _interactionFinished?.Dispose();
                _cancelInteraction?.Dispose();
            }

            public void Dispose()
            {
                _activeInteraction?.Dispose();
                _startInteraction?.Dispose();
                _cancelInteraction?.Dispose();
                _interactionFinished?.Dispose();
            }
        }
    }
}