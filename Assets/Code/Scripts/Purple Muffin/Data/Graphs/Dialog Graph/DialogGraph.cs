// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Data.Primitives;
using UnityEngine;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CreateAssetMenu(menuName = "PurpleMuffin/Data/Graphs/Dialog Graph")]
    [SupportedNodes(typeof(DialogNode))]
    public class DialogGraph : TraversableGraph
    {
	    /// <summary>
	    ///     The index of the choice made during a dialog.
	    /// </summary>
	    public IntField DialogChoice;

	    /// <summary>
	    ///     The dialog string data reference. Any dialog triggered by the graph will be within this reference.
	    /// </summary>
	    public StringField DialogString;
    }
}