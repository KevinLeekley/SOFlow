// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using SOFlow.Data.Primitives;

namespace SOFlow
{
    [Serializable]
    public class EventCondition
    {
	    /// <summary>
	    ///     The condition to test against.
	    /// </summary>
	    public BoolData Condition;

	    /// <summary>
	    ///     Indicates whether this condition should be inverted.
	    /// </summary>
	    public bool InvertCondition;

	    /// <summary>
	    ///     Evaluates the event condition.
	    /// </summary>
	    /// <returns></returns>
	    public bool Evaluate()
        {
            if(InvertCondition) return !Condition.Value;

            return Condition.Value;
        }
    }
}