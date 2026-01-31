using GGJ2026.Manager;
using GGJ2026.Player;
using System.Linq;
using UnityEngine;

namespace GGJ2026.Prop
{
    public class Mirror : MonoBehaviour, IInteractible
    {
        public string AssociatedLine { set; get; }
        private MaskType _mask;
        private Collider _coll;

        public void SetAssociatedMask(MaskType mask)
        {
            _mask = mask;
            UpdateCollisions(MaskManager.Instance.CurrentMask);
        }

        private void Awake()
        {
            _coll = GetComponents<Collider>().First(x => !x.isTrigger);
            MaskManager.Instance.OnMaskChange.AddListener((mask) =>
            {
                UpdateCollisions(mask);
            });
        }

        private void UpdateCollisions(MaskType currMask)
        {
            _coll.enabled = _mask != currMask;
        }

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
