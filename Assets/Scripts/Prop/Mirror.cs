using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Prop
{
    public class Mirror : MonoBehaviour, IInteractible
    {
        EntityId IInteractible.Key => gameObject.GetEntityId();

        public void Interact(CustomPlayerController player)
        {
            throw new System.NotImplementedException();
        }
    }
}
