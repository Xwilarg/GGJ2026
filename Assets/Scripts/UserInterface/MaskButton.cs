using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2026.UserInterface
{
    public class MaskButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private Image _image;

        public void Init(Sprite sprite, string text)
        {
            _text.text = text;
            _image.sprite = sprite;
        }
    }
}
