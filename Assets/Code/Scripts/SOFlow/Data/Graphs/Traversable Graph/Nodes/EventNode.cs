// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Events;
using SOFlow.Utilities;

namespace SOFlow.Data.Graphs
{
    /// <inheritdoc />
    [CreateNodeMenu("SOFlow/Logic/Event")]
    public class EventNode : TraversableNode
    {
        /// <summary>
        ///     The parameter cache for this event.
        /// </summary>
        private readonly SOFlowDynamic _parameterCache = new SOFlowDynamic();

        /// <summary>
        ///     The event raised by this node.
        /// </summary>
        public DynamicEvent Event;

        private void OnValidate()
        {
            _parameterCache.Value = this;
        }

        /// <inheritdoc />
        public override void TriggerNode()
        {
            base.TriggerNode();

            Event.Invoke(_parameterCache);

            TraverseToNextNode();
        }

        /// <inheritdoc />
        public override void TraverseToNextNode()
        {
            TraversableGraph traversableGraph = (TraversableGraph)graph;

            TraversableNode exit = GetConnectedNode("Exit");

            if(exit != null)
                exit.TriggerNode();
            else
                traversableGraph.SignalEndReached();
        }
    }
}