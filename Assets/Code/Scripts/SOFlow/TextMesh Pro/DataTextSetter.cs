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

#if UNITY_EDITOR
        /// <summary>
        ///     Adds a Data Text Setter to the scene.
        /// </summary>
        [UnityEditor.MenuItem("GameObject/SOFlow/TextMesh Pro/Add Data Text Setter", false, 10)]
        public static void AddComponentToScene()
        {
	        TMP_Text text = UnityEditor.Selection.activeGameObject?.GetComponent<TMP_Text>();

	        if(text != null)
	        {
		        DataTextSetter dataTextSetter = text.gameObject.AddComponent<DataTextSetter>();
		        dataTextSetter.TextElement = text;

		        return;
	        }

	        GameObject _gameObject = new GameObject("Data Text Setter", typeof(DataTextSetter));

	        if(UnityEditor.Selection.activeTransform != null)
	        {
		        _gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
	        }

	        UnityEditor.Selection.activeGameObject = _gameObject;
        }
#endif
    }
}