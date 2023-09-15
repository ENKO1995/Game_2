using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for a box to display text in the scene
/// </summary>
public class TextBox : MonoBehaviour
{
    //The transform to scale the whole text box
    [SerializeField]
    private Transform scalerTransform;

    //The text
    [SerializeField]
    private TextMeshPro text;

    //current scale of the text box
    private float textboxScale = 0;

    //text box opening speed
    private static float openSpeed = 5;

    //text box size when closed
    private static float textBoxMinScale = 0.3f;

    //text box is open
    private bool open = false;

    //Update, handles scaling
    void Update()
    {
        textboxScale = Mathf.Clamp01(textboxScale + Time.deltaTime * openSpeed * (open ? 1 : -1));

        //text box is closed, reset text
        if (textboxScale == 0)
        {
            text.text = "...";
        }

        scalerTransform.localScale = Vector3.one * (textboxScale * (1 - textBoxMinScale) + textBoxMinScale);
    }

    /// <summary>
    /// Opens the text box and displays the text
    /// </summary>
    /// <param name="_text">text to display</param>
    public void displayText(string _text)
    {
        text.text = _text;
        open = true;
    }

    /// <summary>
    /// Closes the text box
    /// </summary>
    public void close()
    {
        open = false;
    }
}
