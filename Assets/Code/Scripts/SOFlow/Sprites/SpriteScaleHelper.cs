// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.CameraUtilities;
using UnityEngine;

namespace SOFlow.Sprites
{
    public class SpriteScaleHelper : MonoBehaviour
    {
        /// <summary>
        ///     The sprite scale.
        /// </summary>
        private Vector3 _spriteScale = Vector3.one;

        /// <summary>
        ///     The sprite size.
        /// </summary>
        private Vector3 _spriteSize;

        /// <summary>
        ///     Indicates whether the sprite size has been captured.
        /// </summary>
        private bool _spriteSizeCaptured;

        /// <summary>
        ///     The resolution state.
        /// </summary>
        public ResolutionState ResolutionState;

        /// <summary>
        ///     Scales the sprite.
        /// </summary>
        public void ScaleSprite()
        {
            if(!_spriteSizeCaptured)
            {
                _spriteSize         = transform.localScale;
                _spriteScale.y      = _spriteSize.y;
                _spriteSizeCaptured = true;
            }

            float resolutionScaling = ResolutionState.DesignedScreenResolution.y /
                                      ResolutionState.DesignedScreenResolution.x *
                                      ResolutionState.CurrentScreenResolution.x /
                                      ResolutionState.CurrentScreenResolution.y;

            _spriteScale.x = _spriteSize.x * resolutionScaling;

            transform.localScale = _spriteScale;
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Sprite Scale Helper to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/Sprites/Add Sprite Scale Helper", false, 10)]
        public static void AddComponentToScene()
        {
            GameObject _gameObject = new GameObject("Sprite Scale Helper", typeof(SpriteScaleHelper));

            if(UnityEditor.Selection.activeTransform != null)
            {
                _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
            }

            UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}