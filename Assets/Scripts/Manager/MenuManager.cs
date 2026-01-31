using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2026.Manager
{
    public class MenuManager : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
