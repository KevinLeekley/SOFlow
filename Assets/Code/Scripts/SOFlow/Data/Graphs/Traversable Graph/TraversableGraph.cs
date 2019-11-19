// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using SOFlow.Data.Events;
using SOFlow.Utilities;
using XNode;

namespace SOFlow.Data.Graphs
{
    /// <inheritdoc />
    [SupportedNodes(typeof(BranchNode), typeof(EventNode))]
    public abstract class TraversableGraph : NodeGraph
    {
	    /// <summary>
	    ///     The parameter cache for events.
	    /// </summary>
	    private readonly SOFlowDynamic _parameterCache = new SOFlowDynamic();

	    /// <summary>
	    ///     The current node.
	    /// </summary>
	    public TraversableNode CurrentNode;

	    /// <summary>
	    ///     The entry node for this graph.
	    /// </summary>
	    public EntryNode EntryNode;

	    /// <summary>
	    ///     Event raised when the end of this traversable graph has been reached.
	    /// </summary>
	    public GameEvent OnGraphEndReached;

	    /// <summary>
	    ///     Event raised when a node is triggered within this graph.
	    /// </summary>
	    public DynamicEvent OnNodeTriggered;

	    /// <summary>
	    ///     Begins traversal of this graph.
	    /// </summary>
	    public virtual void Start()
        {
            EntryNode.FirstNode.TriggerNode();
        }

	    /// <summary>
	    ///     Traverses to the next node within the graph.
	    /// </summary>
	    public void TraverseToNextNode()
        {
            if(CurrentNode != null)
                CurrentNode.TraverseToNextNode();
            else
                SignalEndReached();
        }

	    /// <summary>
	    ///     Raises a signal that a node has been triggered within the graph.
	    /// </summary>
	    public virtual void SignalNodeTriggered()
        {
            _parameterCache.Value = CurrentNode;
            OnNodeTriggered.Invoke(_parameterCache);
        }

	    /// <summary>
	    ///     Raises a signal that the end of the graph has been reached.
	    /// </summary>
	    public virtual void SignalEndReached()
        {
            OnGraphEndReached.Raise(CurrentNode);
        }

        /// <inheritdoc />
        public override Node AddNode(Type type)
        {
            if(type == typeof(EntryNode))
            {
                if(EntryNode == null)
                {
                    EntryNode = (EntryNode)base.AddNode(type);

                    return EntryNode;
                }

                return null;
            }

            return base.AddNode(type);
        }
    }
}