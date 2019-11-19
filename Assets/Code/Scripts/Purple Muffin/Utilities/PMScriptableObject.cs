// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace PurpleMuffin.Utilities
{
    /// <inheritdoc />
    public class PMScriptableObject : ScriptableObject
    {
        protected bool Equals(PMScriptableObject other)
        {
            return base.Equals(other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj)) return false;

            if(ReferenceEquals(this, obj)) return true;

            if(obj.GetType() != GetType()) return false;

            return Equals((PMScriptableObject)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(PMScriptableObject first, PMScriptableObject second)
        {
            try
            {
                return first?.name == second?.name;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator !=(PMScriptableObject first, PMScriptableObject second)
        {
            return !(first == second);
        }
    }
}