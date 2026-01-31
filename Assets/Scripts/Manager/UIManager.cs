using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Manager
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { private set; get; }

        [SerializeField]
        private Transform _maskBtnContainer;

        [SerializeField]
        private GameObject _maskBtnPrefab;

        private void Awake()
        {
            Instance = this;
        }

        public void AddButton(MaskType mask, int count)
        {

        }
    }
}
