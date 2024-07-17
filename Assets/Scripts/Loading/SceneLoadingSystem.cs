using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Loading
{
    public class SceneLoadingSystem : IInitializable
    {
        [Inject] SceneLoadingSettings _sceneLoadingSettings;

        public int LastSceneIndex { get; private set; }

        public void Initialize()
        {
            _vignetteControllerInstance = Object.Instantiate(_sceneLoadingSettings.VignetteControllerPrefab);
            Object.DontDestroyOnLoad(_vignetteControllerInstance.gameObject);
        }

        public void LoadScene(int sceneIndex, Vector2 vignettePosition)
        {
            LastSceneIndex = _currentSceneIndex;
            _currentSceneIndex = sceneIndex;
            _vignetteControllerInstance.Animate(() => SceneManager.LoadSceneAsync(sceneIndex), true, vignettePosition, _sceneLoadingSettings.PauseValue);
        }

        public void OpenUpVignette(Vector2 vignettePosition)
        {
            _vignetteControllerInstance.Animate(null, false, vignettePosition);
        }

        VignetteController _vignetteControllerInstance;
        int _currentSceneIndex = 1;
    }
}