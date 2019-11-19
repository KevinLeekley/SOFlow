// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.ObjectPooling
{
    public interface IPoolObject<T> where T : Object
    {
	    /// <summary>
	    ///     The object ID.
	    /// </summary>
	    int ID
        {
            get;
            set;
        }

	    /// <summary>
	    ///     Activates this object.
	    /// </summary>
	    void ActivateObject();

	    /// <summary>
	    ///     Deactivates this object.
	    /// </summary>
	    void DeactivateObject();
    }
}