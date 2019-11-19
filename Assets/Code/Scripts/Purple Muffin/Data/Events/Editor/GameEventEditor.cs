// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PurpleMuffin.Internal;

namespace PurpleMuffin.Data.Events.Editor
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : PMCustomEditor
    {
        /// <summary>
        ///     The current error data.
        /// </summary>
        private List<GameEventErrorData> _currentErrorData = new List<GameEventErrorData>();

        /// <summary>
        ///     The currently displayed error message.
        /// </summary>
        private string _currentErrorMessage = string.Empty;

        /// <summary>
        ///     The error scroll position.
        /// </summary>
        private Vector2 _errorScrollPosition;

        /// <summary>
        ///     The listeners scroll position.
        /// </summary>
        private Vector2 _listenersScrollPosition;

        /// <summary>
        ///     The log scroll position.
        /// </summary>
        private Vector2 _logScrollPosition;

        /// <summary>
        ///     The scroll height.
        /// </summary>
        private readonly float _scrollHeight = 200f;

        /// <summary>
        ///     The GameEvent target.
        /// </summary>
        private GameEvent _target;

        private void OnEnable()
        {
            _target = (GameEvent)target;
        }

        /// <inheritdoc />
        protected override void OnCustomInspectorGUI()
        {
            base.OnCustomInspectorGUI();

            if(GUI.changed) EditorUtility.SetDirty(_target);
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   if(PMEditorUtilities.DrawColourButton("Raise",
                                                                                         PMEditorSettings
                                                                                            .AcceptContextColour))
                                                       _target.Raise();

                                                   if(PMEditorUtilities.DrawColourButton("Search In Scene",
                                                                                         PMEditorSettings
                                                                                            .TertiaryLayerColour))
                                                       SearchEventInScene();

                                                   if(PMEditorUtilities.DrawColourButton("Add To Scene",
                                                                                         PMEditorSettings
                                                                                            .TertiaryLayerColour))
                                                       GameEvent.AddGameEventToScene(_target);
                                               });

            PMEditorUtilities.DrawSecondaryLayer(DrawEventListeners);
            PMEditorUtilities.DrawTertiaryLayer(DrawEventStack);
            PMEditorUtilities.DrawTertiaryLayer(DrawErrorMessage);
        }

        /// <summary>
        ///     Draws the list of event listeners.
        /// </summary>
        private void DrawEventListeners()
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Listeners", PMStyles.BoldCenterLabel);
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Size: {_target.Listeners.Count}", PMStyles.WordWrappedMiniLabel);

            EditorGUILayout.EndHorizontal();

            PMEditorUtilities.DrawScrollViewColourLayer(PMEditorSettings.SecondaryLayerColour,
                                                        ref _listenersScrollPosition,
                                                        () =>
                                                        {
                                                            for(int index = 0; index < _target.Listeners.Count; index++)
                                                            {
                                                                IEventListener listener = _target.Listeners[index];

                                                                PMEditorUtilities
                                                                   .DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                                                              () =>
                                                                                              {
                                                                                                  EditorGUILayout
                                                                                                     .LabelField($"{index} | {listener.GetObjectType().Name}");

                                                                                                  EditorGUILayout
                                                                                                     .ObjectField(listener.GetGameObject(),
                                                                                                                  typeof
                                                                                                                  (GameObject
                                                                                                                  ),
                                                                                                                  true);
                                                                                              });
                                                            }
                                                        }, GUILayout.MaxHeight(_scrollHeight));
        }

        /// <summary>
        ///     Draws the event stack.
        /// </summary>
        private void DrawEventStack()
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Event Stack", PMStyles.BoldCenterLabel);
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Size: {_target.EventStack.Count}", PMStyles.WordWrappedMiniLabel);

            EditorGUILayout.EndHorizontal();

            PMEditorUtilities.DrawScrollViewColourLayer(PMEditorSettings.TertiaryLayerColour, ref _logScrollPosition,
                                                        () =>
                                                        {
                                                            foreach(GameEventLog log in _target.EventStack)
                                                                PMEditorUtilities.DrawSecondaryLayer(() =>
                                                                                                     {
                                                                                                         for(int i = 0,
                                                                                                                 errorIndex
                                                                                                                     = 0,
                                                                                                                 condition
                                                                                                                     = log
                                                                                                                      .Listener
                                                                                                                      .Count;
                                                                                                             i <
                                                                                                             condition;
                                                                                                             i++)
                                                                                                         {
                                                                                                             PMEditorUtilities
                                                                                                                .DrawHorizontalColourLayer(log.IsError[i] ? PMEditorSettings.DeclineContextColour : PMEditorSettings.SecondaryLayerColour,
                                                                                                                                           () =>
                                                                                                                                           {
                                                                                                                                               EditorGUILayout
                                                                                                                                                  .LabelField($"[{log.LogTime:T}] {log.Listener[i].GetObjectType().Name}");

                                                                                                                                               EditorGUILayout
                                                                                                                                                  .ObjectField(log
                                                                                                                                                              .Listener
                                                                                                                                                                   [i]
                                                                                                                                                              .GetGameObject(),
                                                                                                                                                               typeof
                                                                                                                                                               (GameObject
                                                                                                                                                               ),
                                                                                                                                                               true);

                                                                                                                                               if
                                                                                                                                               (log
                                                                                                                                                  .IsError
                                                                                                                                                       [i]
                                                                                                                                               )
                                                                                                                                                   if
                                                                                                                                                   (PMEditorUtilities
                                                                                                                                                      .DrawColourButton("Log",
                                                                                                                                                                        PMEditorSettings
                                                                                                                                                                           .TertiaryLayerColour)
                                                                                                                                                   )
                                                                                                                                                   {
                                                                                                                                                       _currentErrorMessage
                                                                                                                                                           = $"{log.ErrorMessages[errorIndex]}\n\n{log.StackTraces[errorIndex]}";

                                                                                                                                                       _currentErrorData
                                                                                                                                                           = log
                                                                                                                                                              .ErrorData
                                                                                                                                                                   [errorIndex];
                                                                                                                                                   }
                                                                                                                                           });

                                                                                                             if(log
                                                                                                                .IsError
                                                                                                                     [i]
                                                                                                             )
                                                                                                                 errorIndex
                                                                                                                     ++;
                                                                                                         }
                                                                                                     });
                                                        }, GUILayout.MaxHeight(_scrollHeight));
        }

        /// <summary>
        ///     Draws the currently viewed error message.
        /// </summary>
        private void DrawErrorMessage()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Error Log", PMStyles.CenterTextHelpBox);
            GUILayout.FlexibleSpace();

            PMEditorUtilities.DrawScrollViewColourLayer(PMEditorSettings.DeclineContextColour, ref _errorScrollPosition,
                                                        () =>
                                                        {
                                                            EditorGUILayout.TextArea(_currentErrorMessage,
                                                                                     PMStyles.Label);
                                                        });

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("<<< Open Scripts In IDE >>>", PMStyles.CenterTextHelpBox);
            GUILayout.FlexibleSpace();

            foreach(GameEventErrorData errorData in _currentErrorData)
                if(PMEditorUtilities.DrawColourButton($"{errorData.ErrorMethod} >> {errorData.ErrorLine}",
                                                      PMEditorSettings.SecondaryLayerColour))
                    InternalEditorUtility.OpenFileAtLineExternal(errorData.ErrorFile, errorData.ErrorLine);
        }

        /// <summary>
        ///     Searches for this event within the scene.
        /// </summary>
        private void SearchEventInScene()
        {
            List<Object> foundListeners = new List<Object>();

            for(int i = 0, condition = SceneManager.sceneCount; i < condition; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if(scene.isLoaded)
                    foreach(GameObject gameObject in scene.GetRootGameObjects())
                    {
                        IEventListener[] listeners = gameObject.GetComponentsInChildren<IEventListener>(true);

                        foreach(IEventListener listener in listeners)
                            if(listener.GetEvents().Exists(@event => @event == _target))
                                foundListeners.Add(listener.GetGameObject());
                    }
            }

            if(foundListeners.Count > 0)
                Selection.objects = foundListeners.ToArray();
            else
                EditorUtility.DisplayDialog("Game Event Search",
                                            $"Game Event |{_target.name}| not found in open scenes.", "OK");
        }
    }
}
#endif