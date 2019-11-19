// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Data.Events;
using PurpleMuffin.Utilities;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CreateNodeMenu("Purple Muffin/Logic/Event")]
    public class EventNode : TraversableNode
    {
        /// <summary>
        ///     The parameter cache for this event.
        /// </summary>
        private readonly PMDynamic _parameterCache = new PMDynamic();

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