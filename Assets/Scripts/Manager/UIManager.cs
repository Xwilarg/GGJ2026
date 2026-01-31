using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public Button AddButton(Sprite sprite, int count)
        {
            var btn = Instantiate(_maskBtnPrefab, _maskBtnContainer);
            btn.GetComponentInChildren<Image>().sprite = sprite;
            btn.GetComponentInChildren<TMP_Text>().text = count.ToString();
            return btn.GetComponent<Button>();
        }
    }
}
