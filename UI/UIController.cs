using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle in game UI
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider playerHealthBar;

    [SerializeField]
    private TextMeshProUGUI coinCounter;

    [SerializeField]
    private TextMeshProUGUI crystalCounter;

    private int crystals = 0;

    [SerializeField]
    private TextMeshProUGUI crystalTotal;

    [SerializeField]
    private GameObject crystalDisplay;

    [SerializeField]
    private TextMeshProUGUI textDisplay;

    [SerializeField]
    private Image blackScreen;

    //timer for text display
    private float textTimer = 0;
    private float textAlpha = 0;

    //timer for black screen
    private float transitionTimer = 0;
    private float transitionAlpha = 0;

    //black screen fade time
    private static float fadeTime = 0.5f;

    public static Action<string, float> OnGlobalMessage;

    public static Action<float> OnTransition;

    void Start()
    {
        PlayerController.OnPlayerHurt += UpdateHealthBar;
        GameController.OnCoinCountChanged += UpdateCoinCount;
        OnGlobalMessage += DisplayGlobalMessage;
        OnTransition += FadeToBlack;

        //update coins
        UpdateCoinCount(GameController.gameController.GetPlayerCoins());

        int totalCrystals = 0;
        foreach(PickupSwitch crystal in FindObjectsOfType<PickupSwitch>())
        {
            if (crystal.ShowInUI())
            {
                totalCrystals++;
            }
        }

        if (totalCrystals == 0)
        {
            crystalDisplay.SetActive(false);
        } else {
            crystalTotal.text = totalCrystals.ToString();
        }

        PickupSwitch.OnSwitchPickup += IncreaseCrystalCount;
    }

    private void IncreaseCrystalCount()
    {
        crystals++;
        crystalCounter.text = crystals.ToString();
    }

    /// <summary>
    /// Displays a text on screen for a certain time.
    /// </summary>
    /// <param name="_text">text</param>
    /// <param name="_time">time</param>
    private void DisplayGlobalMessage(string _text, float _time)
    {
        textDisplay.text = _text;
        textTimer = _time;
    }

    /// <summary>
    /// Fades the screen to black for a certain time.
    /// </summary>
    /// <param name="_time">time</param>
    private void FadeToBlack(float _time)
    {
        transitionTimer = _time;
    }

    /// <summary>
    /// Sets the health bar slider to a percentage.
    /// </summary>
    /// <param name="_healthPercent">health percentage</param>
    private void UpdateHealthBar(float _healthPercent)
    {
        playerHealthBar.value = _healthPercent;
    }

    /// <summary>
    /// Sets the coin counter to a certain value.
    /// </summary>
    /// <param name="_value">coin count</param>
    private void UpdateCoinCount(int _value)
    {
        coinCounter.text = _value.ToString();
    }

    private void Update()
    {
        //updates text color
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
            textAlpha = Mathf.Min(1, textAlpha + Time.deltaTime / fadeTime);
            textDisplay.color = new Color(1, 1, 1, textAlpha);
        } else if (textAlpha > 0)
        {
            textAlpha = Mathf.Max(0, textAlpha - Time.deltaTime / fadeTime);
            textDisplay.color = new Color(1, 1, 1, textAlpha);
        }

        //updates black screen color
        if (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            transitionAlpha = Mathf.Min(1, transitionAlpha + Time.deltaTime / fadeTime);
            blackScreen.color = new Color(0, 0, 0, transitionAlpha);
        }
        else if (transitionAlpha > 0)
        {
            transitionAlpha = Mathf.Max(0, transitionAlpha - Time.deltaTime / fadeTime);
            blackScreen.color = new Color(0, 0, 0, transitionAlpha);
        }
    }
}
