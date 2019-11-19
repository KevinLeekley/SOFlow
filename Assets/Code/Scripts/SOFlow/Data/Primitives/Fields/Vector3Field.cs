// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using UnityEngine;

namespace SOFlow.Data.Primitives
{
    [Serializable]
    public class Vector3Field : DataField
    {
        /// <summary>
        ///     The explicitly inferred type for the ConstantValue property.
        /// </summary>
        [HideInInspector]
        public Vector3 ConstantValueType;

        /// <summary>
        ///     The explicitly inferred type for the Variable property.
        /// </summary>
        [HideInInspector]
        public Vector3Data VariableType;

        /// <summary>
        ///     The value of this field.
        /// </summary>
        public Vector3 Value
        {
            get
            {
                if(UseConstant) return ConstantValue;

                if(Variable == null) return ConstantValue;

                return Variable.Value;
            }
            set
            {
                if(UseConstant)
                {
                    ConstantValue = value;
                }
                else
                {
                    if(Variable != null) Variable.Value = value;
                }
            }
        }

        /// <summary>
        ///     The volatile data for this field.
        /// </summary>
        public Vector3 ConstantValue
        {
            get => ConstantValueType;
            set => ConstantValueType = value;
        }

        /// <summary>
        ///     The non-volatile data for this field.
        /// </summary>
        public Vector3Data Variable
        {
            get => VariableType;
            set => VariableType = value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Value.x},{Value.y},{Value.z}";
        }

        /// <summary>
        ///     Implicit conversion to the corresponding data type.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator Vector3(Vector3Field data)
        {
            return data.Value;
        }

        /// <inheritdoc />
        public override PrimitiveData GetVariable()
        {
            return Variable;
        }
    }
}