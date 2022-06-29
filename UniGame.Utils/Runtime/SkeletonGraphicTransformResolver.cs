namespace UniGame.Utils.Runtime
{
    using UnityEngine;

    public class SkeletonGraphicTransformResolver : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2       _anchoredPosition;

        private void Awake()
        {
            _rectTransform    = transform as RectTransform;
            _anchoredPosition = _rectTransform.anchoredPosition;
        }

        private void LateUpdate()
        {
            if (_rectTransform.anchoredPosition != _anchoredPosition)
                _rectTransform.anchoredPosition = _anchoredPosition;
        }
    }
}