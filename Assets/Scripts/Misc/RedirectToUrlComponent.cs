using UnityEngine;

public class RedirectToUrlComponent : MonoBehaviour
{
    [SerializeField] string _url;

    public void RedirectToUrl()
    {
        Application.OpenURL(_url);
    }
}