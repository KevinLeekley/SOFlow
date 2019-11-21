// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace SOFlow.Data.Primitives
{
    public class PrimitiveData : ScriptableObject
    {
        /// <summary>
        /// The developer description for this primitive data.
        /// </summary>
        [Multiline]
        public string Description;
        
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