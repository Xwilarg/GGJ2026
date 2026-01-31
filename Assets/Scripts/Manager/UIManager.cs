using UnityEngine;

namespace GGJ2026.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _maskWheel;

        private void Awake()
        {
            _maskWheel.SetActive(false);
        }
    }
}
