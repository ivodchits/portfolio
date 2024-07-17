using UnityEngine;

public class OpenSceneComponent : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void OpenScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}