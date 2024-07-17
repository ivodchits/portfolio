using System;
using Loading;
using UnityEngine;
using Zenject;

public class MapVignetteHelper : MonoBehaviour
{
    [SerializeField] VignettePositionByLastScene[] _vignettePositionByLastScene;

    [Inject] SceneLoadingSystem _sceneLoadingSystem;

    void Start()
    {
        var position = Vector3.zero;
        foreach (var vignettePosition in _vignettePositionByLastScene)
        {
            if (vignettePosition.SceneIndex == _sceneLoadingSystem.LastSceneIndex)
            {
                position = new Vector2(vignettePosition.VignettePosition.localPosition.x / Screen.width, vignettePosition.VignettePosition.localPosition.y / Screen.height);
            }
        }
        _sceneLoadingSystem.OpenUpVignette(position);
    }

    [Serializable]
    private class VignettePositionByLastScene
    {
        public int SceneIndex;
        public Transform VignettePosition;
    }
}
