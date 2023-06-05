using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Image image;
    public Color newColorGrey;
    public Color newColorBlue;
    public Color newColorRed;

    public void SetColorGrey()
    {
        image.color = newColorGrey;
    }
    public void SetColorBlue()
    {
        image.color = newColorBlue;
    }
    public void SetColorRed()
    {
        image.color = newColorRed;
    }
}
