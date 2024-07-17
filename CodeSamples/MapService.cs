using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game.View;
using Services;
using Settings;
using UniRx;
using Zenject;

/// <summary>
/// This class is from a board game project and it is responsible for generating the map.
/// It showcases the use of Addressables to load assets asynchronously and the use of Zenject for dependency injection.
/// Performance is not a concern in this case, so the code is simple and straightforward.
/// I also added the ServiceBase class to the bottom of the file so it would be clearer what it represents.
/// </summary>
public class MapService : ServiceBase
{
    [Inject] GameSettings _gameSettings;

    public IReadOnlyReactiveProperty<MapView> Map => _mapView;

    public async Task GenerateMap(BoardSettings settings)
    {
        var islands = _gameSettings.GetIslandSettings();
        var list = new List<IslandView>();
        var tmp = new List<AssetReferenceGameObject>(4);
        var go = new GameObject("Map", typeof(MapView), typeof(MapInputController));
        var mapView = go.GetComponent<MapView>();

        await Task.WhenAll(
            AddIslands(tmp, list, islands.LargeIslands, settings.LargeIslandsAmount),
            AddIslands(tmp, list, islands.MediumIslands, settings.MediumIslandsAmount),
            AddIslands(tmp, list, islands.SmallIslands, settings.SmallIslandsAmount));

        mapView.Initialize(list, settings.IslandPlacement);
        _mapView.Value = mapView;
    }

    async Task AddIslands(List<AssetReferenceGameObject> tmp, List<IslandView> results, AssetReferenceGameObject[] pool, int amount)
    {
        if (amount <= 0)
            return;

        var tasks = new List<Task>();
        tmp.AddRange(pool);

        for (int i = 0; i < amount; i++)
        {
            var index = Random.Range(0, tmp.Count);
            var asyncOperationHandle = tmp[index].LoadAssetAsync();
            tmp.RemoveAt(index);

            tasks.Add(asyncOperationHandle.Task);

            asyncOperationHandle.Completed += handle =>
            {
                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("Failed to load island prefab");
                    return;
                }
                var islandView = handle.Result.GetComponent<IslandView>();
                results.Add(islandView);
            };
        }
        tmp.Clear();

        await Task.WhenAll(tasks);
    }

    readonly ReactiveProperty<MapView> _mapView = new();
}

public abstract class ServiceBase : IInitializable, IDisposable
{
    protected readonly CompositeDisposable _disposables = new CompositeDisposable();

    public virtual void Initialize() { }

    public virtual void Dispose()
    {
        _disposables.Dispose();
    }
}