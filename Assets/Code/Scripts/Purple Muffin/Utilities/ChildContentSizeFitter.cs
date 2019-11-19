// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using PurpleMuffin.Internal;
using UnityEngine;

namespace PurpleMuffin.Utilities
{
    /// <summary>
    ///     Adjusts the size of the attached gameobject according to its children.
    ///     Required : Must be attached to a RectTransform
    /// </summary>
    [ExecuteInEditMode]
    public class ChildContentSizeFitter : MonoBehaviour
    {
        /// <summary>
        ///     The current content size.
        /// </summary>
        private Vector2 _contentSize = Vector2.zero;

        /// <summary>
        ///     The current content step.
        /// </summary>
        private int _currentContentStep;

        /// <summary>
        ///     Convenience reference to this RectTransform component.
        /// </summary>
        private RectTransform _thisRectTransform;

        /// <summary>
        ///     The additional content padding.
        /// </summary>
        [Info("The additional content padding.")]
        public Vector2 ContentPadding = Vector2.zero;

        /// <summary>
        ///     Indicates how many items should be skipped per iteration.
        /// </summary>
        [Info("Indicates how many items should be skipped per iteration.")]
        public int ContentStepCount = 1;

        /// <summary>
        ///     A list of objects that will not affect the height of this RectTransform.
        /// </summary>
        [Tooltip("A list of objects that will not affect the height of this RectTransform.")]
        public List<RectTransform> HeightIgnoredObjects = new List<RectTransform>();

        /// <summary>
        ///     A list of objects that will not affect the size of this RectTransform.
        /// </summary>
        [Tooltip("A list of objects that will not affect the size of this RectTransform.")]
        public List<RectTransform> IgnoredObjects = new List<RectTransform>();

        /// <summary>
        ///     When set to true, child gameobjects will not affect the height of this RectTransform.
        /// </summary>
        [Info("When set to true, child gameobjects will not affect the height of this RectTransform.")]
        public bool IgnoreHeight;

        /// <summary>
        ///     When set to true, child gameobjects will not affect the width of this RectTransform.
        /// </summary>
        [Header("Operation Constraints")]
        [Info("When set to true, child gameobjects will not affect the width of this RectTransform.")]
        public bool IgnoreWidth;

        /// <summary>
        ///     The maximum size this RectTransform can take.
        /// </summary>
        [Info("The maximum size this RectTransform can take.")]
        public Vector2 MaxSize = new Vector2(1920f, 1080f);

        /// <summary>
        ///     The minimum size this RectTransform can take.
        /// </summary>
        [Header("Size Constraints")]
        [Info("The minimum size this RectTransform can take.")]
        public Vector2 MinSize = Vector2.zero;

        /// <summary>
        ///     A list of objects that will not affect the width of this RectTransform.
        /// </summary>
        [Header("Ignored Objects")]
        [Tooltip("A list of objects that will not affect the width of this RectTransform.")]
        public List<RectTransform> WidthIgnoredObjects = new List<RectTransform>();

        private void Start()
        {
            // Store a reference to this RectTransform
            _thisRectTransform = transform as RectTransform;
        }

        private void Update()
        {
            _contentSize.x = IgnoreWidth ? _thisRectTransform.sizeDelta.x : ContentPadding.x  * transform.childCount;
            _contentSize.y = IgnoreHeight ? _thisRectTransform.sizeDelta.y : ContentPadding.y * transform.childCount;

            _currentContentStep = 0;

            foreach(RectTransform child in transform)
            {
                // Skip inactive children.
                if(!child.gameObject.activeSelf) continue;

                // Skip children until the content step count is reached.
                if(--_currentContentStep <= 0)
                    _currentContentStep = ContentStepCount;
                else
                    continue;

                // Skip to the next child if the current child is part of the ignore list
                if(IgnoredObjects.Exists(ignoredTransform => ignoredTransform == child)) continue;

                // Skip width adjustments if the IgnoreWidth flag is true or if the current child is part of the width ignore list
                if(!IgnoreWidth && !WidthIgnoredObjects.Exists(ignoredTransform => ignoredTransform == child))
                    _contentSize.x += child.sizeDelta.x;

                // Skip height adjustments if the IgnoreHeight flag is true or if the current child is part of the height ignore list
                if(!IgnoreHeight && !HeightIgnoredObjects.Exists(ignoredTransform => ignoredTransform == child))
                    _contentSize.y += child.sizeDelta.y;
            }

            // Make sure the content size adheres to its minimum and maximum bounds
            _contentSize.x = Mathf.Clamp(_contentSize.x, MinSize.x, MaxSize.x);
            _contentSize.y = Mathf.Clamp(_contentSize.y, MinSize.y, MaxSize.y);

            // Finally apply the new content size
            _thisRectTransform.sizeDelta = _contentSize;
        }
    }
}