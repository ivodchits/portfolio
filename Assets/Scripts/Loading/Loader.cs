using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] GameObject[] _objectsToKeep;
    [SerializeField] int _sceneToLoad = 1;

    void Start()
    {
        foreach (var obj in _objectsToKeep)
        {
            DontDestroyOnLoad(obj);
        }
        SceneManager.LoadScene(_sceneToLoad);
    }
}
