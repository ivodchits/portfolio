using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(fileName = "MusicSettings", menuName = "ScriptableObjects/MusicSettings")]
    public class MusicSettings : ScriptableObject
    {
        [SerializeField] MusicController _musicControllerPrefab;

        public MusicController MusicControllerPrefab => _musicControllerPrefab;
    }
}