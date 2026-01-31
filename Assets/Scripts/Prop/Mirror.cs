using GGJ2026.Manager;
using GGJ2026.Player;
using Sketch.Translation;
using System.Linq;
using UnityEngine;

namespace GGJ2026.Prop
{
    public class Mirror : MonoBehaviour, IInteractible
    {
        public string AssociatedLine { set; get; }
        private MaskType _mask;
        private Collider _coll;

        private bool _isUsed;

        public void SetAssociatedMask(MaskType mask)
        {
            _mask = mask;
        }

        private void Awake()
        {
            _coll = GetComponents<Collider>().First(x => !x.isTrigger);
        }

        EntityId IInteractible.Key => gameObject.GetEntityId();

        public void CancelInteraction(CustomPlayerController player)
        {
            UIManager.Instance.SetDescriptionText(string.Empty);
        }

        public void Interact(CustomPlayerController player)
        {
            if (_isUsed) return;

#if UNITY_EDITOR
            Debug.Log($"[MIRROR] Used mask {MaskManager.Instance.CurrentMask} on requirement {_mask}");
#endif
            if (_mask == MaskManager.Instance.CurrentMask)
            {
                _coll.enabled = false;
                UIManager.Instance.SetDescriptionText(Translate.Instance.Tr("memory_good"));
            }
            else
            {
                UIManager.Instance.SetDescriptionText(Translate.Instance.Tr("memory_bad"));
            }
            _isUsed = true;
        }

        public void Prepare(CustomPlayerController player)
        {
            if (_isUsed) return;

            UIManager.Instance.SetDescriptionText(AssociatedLine);
        }

        public bool CanInteract(CustomPlayerController player)
        {
            return !_isUsed;
        }
    }
}
