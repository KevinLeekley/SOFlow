// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

namespace SOFlow.Fading
{
    public class SimpleUIFader : MonoBehaviour
    {
        /// <summary>
        ///     The fade target.
        /// </summary>
        public GameObject FadeTarget;

        /// <summary>
        ///     The unfaded colour.
        /// </summary>
        public Color UnfadedColour = Color.white;

        /// <summary>
        ///     The faded colour.
        /// </summary>
        public Color FadedColour = Color.white;

        /// <summary>
        ///     Enable to only allow fading in.
        /// </summary>
        public bool OnlyFade;

        /// <summary>
        ///     The fade time.
        /// </summary>
        public float FadeTime;

        /// <summary>
        ///     The unfade time.
        /// </summary>
        public float UnfadeTime;

        /// <summary>
        ///     The wait between fades.
        /// </summary>
        public float WaitBetweenFades;

        /// <summary>
        ///     The list of objects to ignore.
        /// </summary>
        public List<GameObject> IgnoredObjects = new List<GameObject>();

        /// <summary>
        ///     Event raised when the fading is completed.
        /// </summary>
        public UltEvent OnFadeComplete;

        /// <summary>
        ///     Event raised when waiting between fades.
        /// </summary>
        public UltEvent OnFadeWait;

        /// <summary>
        ///     The cached set of canvas renderers.
        /// </summary>
        private readonly Dictionary<GameObject, List<CanvasRenderer>> _cachedRenderers =
            new Dictionary<GameObject, List<CanvasRenderer>>();

        /// <summary>
        ///     Indicates whether we are currently fading.
        /// </summary>
        public bool Fading
        {
            get;
            private set;
        }

        /// <summary>
        ///     Caches the canvas renders for the current fade target.
        /// </summary>
        private void CacheCanvasRenderers()
        {
            if(!_cachedRenderers.ContainsKey(FadeTarget))
            {
                CanvasRenderer[]     canvasRenderers = FadeTarget.GetComponentsInChildren<CanvasRenderer>();
                List<CanvasRenderer> targetRenderers = new List<CanvasRenderer>();

                foreach(CanvasRenderer canvasRenderer in canvasRenderers)
                    if(!IgnoredObjects.Contains(canvasRenderer.gameObject))
                        targetRenderers.Add(canvasRenderer);

                _cachedRenderers.Add(FadeTarget, targetRenderers);
            }
        }

        /// <summary>
        ///     Updates the colours for the provided canvas renderers.
        /// </summary>
        /// <param name="canvasRenderers"></param>
        /// <param name="colour"></param>
        private void UpdateCanvasRendererColours(List<CanvasRenderer> canvasRenderers, Color colour)
        {
            foreach(CanvasRenderer canvasRenderer in canvasRenderers) canvasRenderer.SetColor(colour);
        }

        /// <summary>
        ///     Initiates the fade.
        /// </summary>
        public void Fade()
        {
            if(!Fading && gameObject.activeInHierarchy)
            {
                Fading = true;
                StartCoroutine(nameof(FadeRoutine));
            }
        }

        private IEnumerator FadeRoutine()
        {
            CacheCanvasRenderers();
            List<CanvasRenderer> canvasRenderers;

            if(!_cachedRenderers.TryGetValue(FadeTarget, out canvasRenderers)) yield break;

            float startTime = Time.realtimeSinceStartup;
            float endTime   = startTime + FadeTime;

            while(Time.realtimeSinceStartup < endTime)
            {
                UpdateCanvasRendererColours(canvasRenderers,
                                            Color.Lerp(UnfadedColour, FadedColour,
                                                       (Time.realtimeSinceStartup - startTime) /
                                                       (endTime                   - startTime)));

                yield return null;
            }

            UpdateCanvasRendererColours(canvasRenderers, FadedColour);

            OnFadeWait.Invoke();

            if(!OnlyFade)
            {
                yield return new WaitForSeconds(WaitBetweenFades);

                StartCoroutine(nameof(Unfade));
            }
            else
            {
                Fading = false;
            }
        }

        private IEnumerator Unfade()
        {
            CacheCanvasRenderers();
            List<CanvasRenderer> canvasRenderers;

            if(!_cachedRenderers.TryGetValue(FadeTarget, out canvasRenderers)) yield break;

            float startTime = Time.realtimeSinceStartup;
            float endTime   = startTime + UnfadeTime;

            while(Time.realtimeSinceStartup < endTime)
            {
                UpdateCanvasRendererColours(canvasRenderers,
                                            Color.Lerp(FadedColour, UnfadedColour,
                                                       (Time.realtimeSinceStartup - startTime) /
                                                       (endTime                   - startTime)));

                yield return null;
            }

            UpdateCanvasRendererColours(canvasRenderers, UnfadedColour);

            Fading = false;
            OnFadeComplete.Invoke();
        }
    }
}