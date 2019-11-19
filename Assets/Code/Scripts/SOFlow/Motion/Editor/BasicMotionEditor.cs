// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using SOFlow.Internal;
using UnityEditor;

namespace SOFlow.Motion
{
    [CustomEditor(typeof(BasicMotion))]
    [CanEditMultipleObjects]
    public class BasicMotionEditor : SOFlowCustomEditor
    {
        /// <summary>
        ///     The BasicMotion target.
        /// </summary>
        private BasicMotion _target;

        private void OnEnable()
        {
            _target = (BasicMotion)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            SOFlowEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   serializedObject.DrawProperty("UseLocalPosition");
                                                   serializedObject.DrawProperty("StayActiveForSetDuration");

                                                   if(_target.StayActiveForSetDuration)
                                                       serializedObject.DrawProperty("ActiveDuration");

                                                   serializedObject.DrawProperty("VelocityMultiplier");

                                                   DrawVelocityFields("Forward");
                                                   DrawVelocityFields("Horizontal");
                                                   DrawVelocityFields("Vertical");
                                               });
        }

        /// <summary>
        ///     Draws the provided velocity fields.
        /// </summary>
        /// <param name="axis"></param>
        private void DrawVelocityFields(string axis)
        {
            SOFlowEditorUtilities.DrawSecondaryLayer(() =>
                                                 {
                                                     serializedObject.DrawProperty($"Use{axis}Velocity");

                                                     if(serializedObject.FindProperty($"Use{axis}Velocity").boolValue)
                                                         SOFlowEditorUtilities.DrawTertiaryLayer(() =>
                                                                                             {
                                                                                                 serializedObject
                                                                                                    .DrawProperty($"{axis}Velocity");

                                                                                                 serializedObject
                                                                                                    .DrawProperty($"Invert{axis}Velocity");
                                                                                             });
                                                 });
        }
    }
}
#endif