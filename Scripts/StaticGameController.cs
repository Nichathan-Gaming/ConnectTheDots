using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A typical StaticGameController holds the values for the game
/// </summary>
public class StaticGameController : MonoBehaviour
{
    internal static int width = 6;
    internal static int height = 6;
    internal static bool hasRandomSeed = false;
    internal static int randomSeed = 42;
    int baseRandomSeed = 42;

    [Header("Set the Width")]
    [SerializeField] Slider widthSlider;
    [SerializeField] TMP_Text widthValue;
    [Header("Set the Height")]
    [SerializeField] Slider heightSlider;
    [SerializeField] TMP_Text heightValue;
    [Header("Controls the random seed")]
    [SerializeField] Image image;
    [SerializeField] TMP_InputField randomSeedInputField;

    /// <summary>
    /// Sets the display values
    /// </summary>
    private void Start()
    {
        widthSlider.value = width;
        widthValue.text = "" + width;
        heightSlider.value = height;
        heightValue.text = "" + height;

        image.color = hasRandomSeed ? Color.green : Color.red;
        randomSeedInputField.text = ""+randomSeed;
    }

    /// <summary>
    /// Runs when the slider is dragged
    /// </summary>
    /// <param name="f">The new width value</param>
    public void OnWidthChange(float f)
    {
        width = (int)f;
        widthValue.text = "" + width;
    }

    /// <summary>
    /// Runs when the slider is dragged
    /// </summary>
    /// <param name="f">The new height value</param>
    public void OnHeightChange(float f)
    {
        height = (int)f;
        heightValue.text = "" + height;
    }

    /// <summary>
    /// Toggle the hasRandomSeed checkbox
    /// </summary>
    public void ToggleRandomSeed()
    {
        hasRandomSeed = !hasRandomSeed;

        image.color = hasRandomSeed ? Color.green : Color.red;
    }

    /// <summary>
    /// Changes the random seed value when the random seed input field is changed
    /// 
    /// if value is in error, set as blank and return value to base
    /// </summary>
    /// <param name="value">the new random seed</param>
    public void OnRandomSeedChange(string value)
    {
        try
        {
            randomSeed = int.Parse(value);
        }
        catch (System.Exception)
        {
            randomSeedInputField.text = "";
            randomSeed = baseRandomSeed;
        }
    }
}
