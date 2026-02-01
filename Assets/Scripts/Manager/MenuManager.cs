using Sketch.Translation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2026.Manager
{
    public class MenuManager : MonoBehaviour
    {
        private void Awake()
        {
            Translate.Instance.SetLanguages(new[]
            {
                "english", "french", "turkish", "dutch", "spanish"
            });
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void SetLanguage(string name)
            => Translate.Instance.CurrentLanguage = name;
    }
}
