using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    private static int ammo = 10;
    public Text uiText;

    public static AmmoManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void DecreaseAmmo(int decrease)
    {
        ammo -= decrease;
        uiText.text = ammo.ToString();
    }
}
