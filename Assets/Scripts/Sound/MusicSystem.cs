using UnityEngine;
using Zenject;

namespace Sound
{
    public class MusicSystem : IInitializable
    {
        [Inject] MusicSettings _musicSettings;
        
        public void Initialize()
        {
            _musicControllerInstance = Object.Instantiate(_musicSettings.MusicControllerPrefab);
            Object.DontDestroyOnLoad(_musicControllerInstance.gameObject);
        }

        public void Mute()
        {
            _musicControllerInstance.Mute();
        }

        MusicController _musicControllerInstance;
    }
}
