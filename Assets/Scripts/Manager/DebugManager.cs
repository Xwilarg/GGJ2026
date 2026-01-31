using UnityEngine;

namespace GGJ2026.Manager
{
    public class DebugManager : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 500, 500), $"Current mask: {MaskManager.Instance.CurrentMask}");
        }
#endif
    }
}
