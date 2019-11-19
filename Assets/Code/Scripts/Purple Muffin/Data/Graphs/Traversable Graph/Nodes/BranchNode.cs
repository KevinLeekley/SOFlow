// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using PurpleMuffin.Data.Events;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CreateNodeMenu("Purple Muffin/Logic/Branch")]
    public class BranchNode : TraversableNode
    {
        /// <summary>
        ///     The set of conditions for this branch node.
        /// </summary>
        [Output(dynamicPortList = true)]
        public List<ConditionalEventSet> Conditions = new List<ConditionalEventSet>();

        /// <inheritdoc />
        public override void TriggerNode()
        {
            base.TriggerNode();

            TraverseToNextNode();
        }

        /// <inheritdoc />
        public override void TraverseToNextNode()
        {
            TraversableGraph traversableGraph = (TraversableGraph)graph;

            for(int i = 0; i < Conditions.Count; i++)
            {
                ConditionalEventSet condition = Conditions[i];

                if(condition.Evaluate())
                {
                    TraversableNode evaluatedCondition = GetConnectedNode($"{nameof(Conditions)} {i}");

                    if(evaluatedCondition != null)
                        evaluatedCondition.TriggerNode();
                    else
                        traversableGraph.SignalEndReached();

                    return;
                }
            }

            TraversableNode defaultNode = GetConnectedNode("Default");

            if(defaultNode != null)
                defaultNode.TriggerNode();
            else
                traversableGraph.SignalEndReached();
        }
    }
}