using GGJ2026.Manager;
using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Prop
{
    public class Gem : MonoBehaviour, IInteractible
    {
        public EntityId Key => gameObject.GetEntityId();

        public void CancelInteraction(CustomPlayerController player)
        {
        }

        public bool CanInteract(CustomPlayerController player)
        {
            return true;
        }

        public void Interact(CustomPlayerController player)
        {
            UIManager.Instance.ShowStory();
        }

        public void Prepare(CustomPlayerController player)
        {
        }
    }
}
