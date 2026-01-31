using UnityEngine;

namespace GGJ2026.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;

            /*if (!SceneManager.GetAllScenes().Any(x => x.name.ToLowerInvariant() == "level"))
            {
                SceneManager.LoadScene("Level", LoadSceneMode.Additive);
            }*/
        }
    }
}
