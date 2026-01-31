using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Prop
{
    public interface IInteractible
    {
        public bool CanInteract(CustomPlayerController player);

        public void Prepare(CustomPlayerController player);

        public void Interact(CustomPlayerController player);

        public void CancelInteraction(CustomPlayerController player);

        public EntityId Key { get; }
    }
}
