using UnityEngine;

namespace Landmarks
{
    public class ActivateCanvasGroupLandmark : Landmark
    {
        [SerializeField] CanvasGroup _canvasGroup;

        protected override void ClickLogic()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
