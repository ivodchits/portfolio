using Loading;
using UnityEngine;
using Zenject;

public class ShowcaseVignetteHelper : MonoBehaviour
{
    [SerializeField] Camera _camera;

    [Inject] SceneLoadingSystem _sceneLoadingSystem;

    void Start()
    {
        _sceneLoadingSystem.OpenUpVignette(_camera.WorldToViewportPoint(transform.position) - Vector3.one * 0.5f);
    }

    public void LoadScene(int sceneIndex)
    {
        _sceneLoadingSystem.LoadScene(sceneIndex, _camera.WorldToViewportPoint(transform.position) - Vector3.one * 0.5f);
    }
}