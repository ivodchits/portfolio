using UnityEngine;

public class CopyToClipboardComponent : MonoBehaviour
{
    [SerializeField] string _text;

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = _text;
    }
}