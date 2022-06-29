namespace UniGame.Utils
{
    using System;
    using System.Collections;
    using System.Linq;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class AnchorsWrapper
    {
        [SerializeField]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(_anchors))]
        [ShowIf("@UseAnchors && _allowStretch")]
#endif
        
        private Vector2 _anchorMin = RectTransformUtils.Center;
        
        [SerializeField]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(_anchors))]
        [ShowIf("@UseAnchors && _allowStretch")]
#endif
        private Vector2 _anchorMax = RectTransformUtils.Center;
        
        [SerializeField]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(_anchors))]
        [ShowIf("@UseAnchors && !_allowStretch")]
#endif
        private Vector2 _anchor    = RectTransformUtils.Center;
        
        [SerializeField]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(_anchors))]
        [ShowIf(nameof(UsePivot))]
#endif
        private Vector2 _pivot     = RectTransformUtils.Center;

        private readonly bool _allowStretch = false;

        private readonly IEnumerable _anchors = new ValueDropdownList<Vector2>().Concat(RectTransformUtils.GetAnchorPresets().Select(p => new ValueDropdownItem<Vector2>(p.Item1, p.Item2)).ToList());

        public Vector2 AnchorMin => _anchorMin;
        public Vector2 AnchorMax => _anchorMax;
        public Vector2 Anchor
        {
            get => _anchor;
            set
            {
                _anchor = value;
                if (_allowStretch)
                {
                    _anchorMin = value;
                    _anchorMax = value;
                }
            }
        }
        public Vector2 Pivot => _pivot;

        public bool UseAnchors { get; set; } = true;
        public bool UsePivot   { get; set; } = true;

        public AnchorsWrapper() { }

        public AnchorsWrapper(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            _allowStretch = true;

            _anchorMin = anchorMin;
            _anchorMax = anchorMax;
            _pivot     = pivot;
        }

        public AnchorsWrapper(Vector2 anchor, Vector2 pivot)
        {
            _anchorMin = anchor;
            _anchorMax = anchor;
            _pivot     = pivot;
        }
    }
}