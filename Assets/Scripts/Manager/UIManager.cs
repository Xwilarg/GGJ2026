using GGJ2026.UserInterface;
using Sketch.Translation;
using Sketch.VN;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private int _vnIndex = 1;

        public bool IsInEnding { private set; get; }

        private float _timer = -1f;

        private void Awake()
        {
            Instance = this;

            _descriptionText.ToDisplay = string.Empty;
            _endingContainer.SetActive(false);
        }

        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _descriptionText.ToDisplay = string.Empty;
                }
            }
        }

        public void ShowStory()
        {
            var mask = GameManager.Instance.GetMask(MaskManager.Instance.CurrentMask);

            _endingContainer.SetActive(true);
            _endingCG.gameObject.SetActive(true);
            _endingCG.sprite = mask.EndingCG;
            _endingDisplay.ToDisplay = Translate.Instance.Tr($"{mask.EndingLine}_1");

            IsInEnding = true;
        }

        public void ShowNextDialogue()
        {
            if (_endingDisplay.IsDisplayDone)
            {
                _vnIndex++;
                var mask = GameManager.Instance.GetMask(MaskManager.Instance.CurrentMask);
                if (_vnIndex == mask.EndingLineCount + 1) SceneManager.LoadScene("Menu");
                else _endingDisplay.ToDisplay = Translate.Instance.Tr($"{mask.EndingLine}_{_vnIndex}");
            }
            else
            {
                _endingDisplay.ForceDisplay();
            }
        }

        public Button AddButton(Sprite sprite, int count)
        {
            var btn = Instantiate(_maskBtnPrefab, _maskBtnContainer);
            btn.GetComponentInChildren<MaskButton>().Init(sprite, count.ToString());
            return btn.GetComponent<Button>();
        }

        public void SetDescriptionText(string text, bool setTimer = false)
        {
            _descriptionText.ToDisplay = text;

            if (setTimer)
            {
                _timer = 2f;
            }
            else
            {
                _timer = -1f;
            }
        }
    }
}
