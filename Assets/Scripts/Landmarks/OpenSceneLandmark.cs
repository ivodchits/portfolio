using Loading;
using UnityEngine;
using Zenject;

namespace Landmarks
{
    public class OpenSceneLandmark : Landmark
    {
        [SerializeField] Camera _camera;
        [SerializeField] int _sceneIndex;

        [Inject] SceneLoadingSystem _sceneLoadingSystem;

        protected override void ClickLogic()
        {
            var position = new Vector2(transform.localPosition.x / Screen.width, transform.localPosition.y / Screen.height);
            _sceneLoadingSystem.LoadScene(_sceneIndex, position);
        }
    }
}
