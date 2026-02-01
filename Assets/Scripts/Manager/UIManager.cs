using GGJ2026.UserInterface;
using Sketch.VN;
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

        [SerializeField]
        private TextDisplay _descriptionText;

        [SerializeField]
        private GameObject _endingContainer;

        [SerializeField]
        private TextDisplay _endingDisplay;

        [SerializeField]
        private Image _endingCG;

        private void Awake()
        {
            Instance = this;

            _descriptionText.ToDisplay = string.Empty;
            _endingContainer.SetActive(false);
        }

        public Button AddButton(Sprite sprite, int count)
        {
            var btn = Instantiate(_maskBtnPrefab, _maskBtnContainer);
            btn.GetComponentInChildren<MaskButton>().Init(sprite, count.ToString());
            return btn.GetComponent<Button>();
        }

        public void SetDescriptionText(string text)
        {
            _descriptionText.ToDisplay = text;
        }
    }
}
