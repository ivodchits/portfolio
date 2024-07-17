using Sound;
using UnityEngine;
using Zenject;

namespace Landmarks
{
    public class URLRedirectionLandmark : Landmark
    {
        [SerializeField] string _url;

        [Inject] MusicSystem _musicSystem;

        protected override void ClickLogic()
        {
            _musicSystem.Mute();
            Application.OpenURL(_url);
        }
    }
}