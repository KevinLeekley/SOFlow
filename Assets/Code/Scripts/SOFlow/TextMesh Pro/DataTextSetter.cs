// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using SOFlow.Data.Primitives;
using SOFlow.Internal;
using TMPro;
using UnityEngine;

namespace SOFlow
{
    [ExecuteInEditMode]
    public class DataTextSetter : MonoBehaviour
    {
	    /// <summary>
	    ///     The data element
	    /// </summary>
	    public PrimitiveData Data;

	    /// <summary>
	    ///     The text to display in the text element.
	    /// </summary>
	    [TextArea(3, 20)]
        public string Text;

	    /// <summary>
	    ///     The text element.
	    /// </summary>
	    [Info("Add {data} within the text block to add the value of the given data.\n" +
              "Example:\n\n"                                                           +
              "This is the current value of the data: {data}")]
        public TMP_Text TextElement;

        private void Start()
        {
            if(!Application.isPlaying) TextElement = GetComponent<TMP_Text>();
        }

        /// <summary>
        ///     Sets the text element text to the specified text and data values.
        /// </summary>
        public void SetText()
        {
            try
            {
                TextElement.SetText(Text.Replace("{data}", Data.GetValueData().ToString()));
            }
            catch
            {
                // Ignore
            }
        }
    }
}