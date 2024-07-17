using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Showcase
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Frame Collection", fileName = "FrameCollection")]
    public class FrameCollection : ScriptableObject, IEnumerable
    {
        [SerializeField] string _selectedPath;
        [SerializeField] string[] _filePaths;

        public IEnumerator GetEnumerator() => _sprites.GetEnumerator();

        public bool Loaded => _loadedSprites == _filePaths.Length;
        public int Length => _filePaths.Length;

        public Sprite Get(int index) => _sprites[index];

        public void Load()
        {
            if (_sprites != null)
                return;

            _sprites = new Sprite[_filePaths.Length];
            for (var i = 0; i < _filePaths.Length; i++)
            {
                var index = i;
                Addressables.LoadAssetAsync<Sprite>(_filePaths[i]).Completed += handle =>
                {
                    _sprites[index] = handle.Result;
                    ++_loadedSprites;
                };
            }
        }

        Sprite[] _sprites;
        int _loadedSprites;
    }
}
