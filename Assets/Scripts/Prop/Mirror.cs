using GGJ2026.Manager;
using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Prop
{
    public class Mirror : MonoBehaviour, IInteractible
    {
        public string AssociatedLine { set; get; }

        EntityId IInteractible.Key => gameObject.GetEntityId();

        public void CancelInteraction(CustomPlayerController player)
        {
            UIManager.Instance.SetDescriptionText(string.Empty);
        }

        public void Interact(CustomPlayerController player)
        {
            UIManager.Instance.SetDescriptionText(AssociatedLine);
        }
    }
}
