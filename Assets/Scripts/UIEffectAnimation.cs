using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sunyunie.FFMPEGNyaa
{
    public class UIEffectAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private bool onClickRotation = false;

        private float scaleFactor  = 1.04f;
        private float scaleTime    = 0.2f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            //transform.DOScale(scaleFactor, scaleTime).SetEase(Ease.InOutCubic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //transform.DOScale(1f, scaleTime * 0.5f).SetEase(Ease.InOutCubic);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(1f, scaleTime * 0.5f).SetEase(Ease.InOutCubic);
            transform.DOScale(scaleFactor, scaleTime).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                transform.DOScale(1f, scaleTime * 0.5f).SetEase(Ease.InOutCubic);
                if (onClickRotation)
                {
                    transform.DORotate(new Vector3(0, 0, 15f), scaleTime * 0.5f, RotateMode.LocalAxisAdd)
                        .SetEase(Ease.InOutCubic)
                        .OnComplete(() =>
                        {
                            transform.DORotate(new Vector3(0, 0, -15f), scaleTime * 0.5f, RotateMode.LocalAxisAdd)
                                .SetEase(Ease.InOutCubic);
                        });
                }
            });
        }
    }
}