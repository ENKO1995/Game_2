using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that displays text in a text box
/// </summary>
public class ActionDisplayText : SwitchActivation
{
    [SerializeField]
    private TextBox textBox;

    [SerializeField]
    private string[] displayText;

    [SerializeField]
    private float displayTime;

    //if text should display in order or randomly
    [SerializeField]
    private bool dialogue;

    private int lastText = -1;

    private float openTimer;

    public override void SwitchFunction(bool _activated)
    {
        if (dialogue)
        {
            lastText++;
            if (lastText < displayText.Length)
            {
                textBox.displayText(displayText[lastText]);
            } else
            {
                lastText = -1;
                openTimer = 0;
                textBox.close();
            }
        } else
        {
            int randomText = 0;
            if (lastText == -1)
            {
                randomText = Random.Range(0, displayText.Length);
            } else
            {
                //don't pick the same random text again
                randomText = Random.Range(0, displayText.Length - 1);
                if (randomText >= lastText)
                {
                    randomText++;
                }
            }

            if (displayText.Length > 1)
            {
                lastText = randomText;
            }

            textBox.displayText(displayText[randomText]);
        }

        openTimer = displayTime;
    }

    private void Update()
    {
        if (openTimer > 0)
        {
            openTimer -= Time.deltaTime;
            if (openTimer <= 0)
            {
                textBox.close();
                lastText = -1;
            }
        }
    }

}
