using GGJ2026.Player;
using GGJ2026.SO;
using System.Linq;
using UnityEngine;

namespace GGJ2026.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private MaskInfo[] _masks;

        public static GameManager Instance { private set; get; }

        public MaskInfo GetMask(MaskType type)
            => _masks.First(x => x.Type == type);

        public MaskInfo[] GetAllMasks()
            => _masks;

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
