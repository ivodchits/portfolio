using UnityEngine;

namespace Loading
{
    [CreateAssetMenu(fileName = "SceneLoadingSettings", menuName = "ScriptableObjects/SceneLoadingSettings")]
    public class SceneLoadingSettings : ScriptableObject
    {
        [SerializeField] VignetteController _vignetteControllerPrefab;
        [SerializeField] float _pauseValue = 0.2f;

        public VignetteController VignetteControllerPrefab => _vignetteControllerPrefab;
        public float PauseValue => _pauseValue;
    }
}
