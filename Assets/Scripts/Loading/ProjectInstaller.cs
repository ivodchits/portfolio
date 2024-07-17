using Loading;
using Sound;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] MusicSettings _musicSettings;
    [SerializeField] SceneLoadingSettings _sceneLoadingSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MusicSettings>().FromInstance(_musicSettings).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneLoadingSettings>().FromInstance(_sceneLoadingSettings).AsSingle();
        Container.BindInterfacesAndSelfTo<MusicSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SceneLoadingSystem>().AsSingle().NonLazy();
    }
}