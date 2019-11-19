// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.Data.Primitives
{
    public class PrimitiveData : ScriptableObject
    {
        /// <summary>
        ///     Returns the value of this data.
        /// </summary>
        /// <returns></returns>
        public virtual object GetValueData()
        {
            return null;
        }
    }
}