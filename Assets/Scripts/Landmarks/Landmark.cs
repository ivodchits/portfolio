using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Landmark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CanvasGroup _description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _description.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _description.alpha = 0;
    }

    public void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        ClickLogic();
    }

    protected virtual void ClickLogic() {}
}