// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;

namespace SOFlow.Utilities
{
	/// <summary>
	///     A wrapper class for the dynamic C# type. The main purpose of this class is to
	///     allow exposure of the contained value within the dynamic type to the Unity inspector.
	///     Note : Only Unity serializable values will be exposed and visible in the Unity inspector.
	///     All data types supported by the dynamic type are still saved within this wrapper class,
	///     even if the values for those types are not visible within the inspector.
	/// </summary>
	[Serializable]
    public class SOFlowDynamic
    {
	    /// <summary>
	    ///     The internal dynamic value.
	    /// </summary>
	    private object _internalValue;

	    /// <summary>
	    ///     The dynamic value within this object.
	    /// </summary>
	    public object Value
        {
            get => _internalValue;
            set => _internalValue = value;
        }

	    /// <summary>
	    ///     Returns the strongly typed version of the dynamically stored value.
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <returns></returns>
	    public T GetTypedValue<T>()
        {
            T typedValue = (T)_internalValue;

            if(typedValue != null) return typedValue;

            return default;
        }
    }
}