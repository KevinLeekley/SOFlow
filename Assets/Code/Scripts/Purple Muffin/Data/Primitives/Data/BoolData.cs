﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Data.Events;
using PurpleMuffin.Utilities;
using UnityEngine;

namespace PurpleMuffin.Data.Primitives
{
    [CreateAssetMenu(menuName = "PurpleMuffin/Data/Primitives/Bool")]
    public class BoolData : PrimitiveData
    {
        /// <summary>
        ///     The Play Mode safe representation of this data.
        /// </summary>
        [SerializeField]
        protected bool _playModeValue;

        /// <summary>
        ///     The true asset value of this data.
        /// </summary>
        public bool AssetValue;

        /// <summary>
        ///     Event raised when this primitive data has changed.
        /// </summary>
        public DynamicEvent OnDataChanged;

        /// <summary>
        ///     Event raised when an update occurs on this primitive data.
        ///     The data does not necessarily change when this event is called.
        /// </summary>
        public DynamicEvent OnDataUpdated;

        /// <summary>
        ///     Determines whether the true asset value should retain
        ///     value changes during Play Mode.
        /// </summary>
        public bool PersistInPlayMode;

        /// <summary>
        ///     The value for this data.
        /// </summary>
        public bool Value
        {
            get
            {
#if UNITY_EDITOR
                // Return the Play Mode safe representation of the data during
                // Play Mode.
                if(Application.isPlaying) return _playModeValue;
#endif
                // Always return the true asset value during Edit Mode.
                return AssetValue;
            }
            set
            {
#if UNITY_EDITOR
                if(Application.isPlaying)
                {
                    // Only alter the Play Mode safe representation of
                    // this data during Play Mode.
                    if(!_playModeValue.Equals(value))
                    {
                        _playModeValue = value;

                        OnDataChanged.Invoke(new PMDynamic
                                             {
                                                 Value = GetValue()
                                             });

                        if(PersistInPlayMode)
                            // If desired, the true asset value can maintain
                            // the changes created during Play Mode.
                            AssetValue = value;
                    }
                }
                else
                {
                    if(!AssetValue.Equals(value))
                    {
                        AssetValue = value;

                        OnDataChanged.Invoke(new PMDynamic
                                             {
                                                 Value = GetValue()
                                             });
                    }
                }
#else
                if(!AssetValue.Equals(value))
                {
                    AssetValue = value;
                    
                    OnDataChanged.Invoke(new PMDynamic
                                         {
                                             Value = GetValue()
                                         });
                }
#endif
                OnDataUpdated.Invoke(new PMDynamic
                                     {
                                         Value = GetValue()
                                     });
            }
        }

        /// <summary>
        ///     Returns the value of this data.
        /// </summary>
        /// <returns></returns>
        public bool GetValue()
        {
            return Value;
        }

        /// <inheritdoc />
        public override object GetValueData()
        {
            return Value;
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            // Resync the Play Mode safe representation with the
            // true asset value during editing.
            _playModeValue = AssetValue;
        }
#endif

        /// <summary>
        ///     Attempts to set the value of this data to the supplied value.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(PMDynamic value)
        {
            if(value.Value is bool)
                Value = (bool)value.Value;
            else if(value.Value is BoolData)
                Value = ((BoolData)value.Value).Value;
            else
                Debug.LogWarning($"[BoolData] Supplied value is not a supported data type.\n{name}");
        }

        /// <summary>
        ///     Attempts to set the value of this data to the supplied value.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(BoolData value)
        {
            Value = value.Value;
        }

        #region Base Comparisons

        /// <summary>
        ///     Checks if the data value is equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Equal(bool value)
        {
            return Value == value;
        }

        /// <summary>
        ///     Checks if the data value is not equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool NotEqual(bool value)
        {
            return Value != value;
        }

        #endregion

        #region Data Comparisons

        /// <summary>
        ///     Checks if the data value is equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Equal(BoolData value)
        {
            return Value == value.Value;
        }

        /// <summary>
        ///     Checks if the data value is not equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool NotEqual(BoolData value)
        {
            return Value != value.Value;
        }

        #endregion
    }
}