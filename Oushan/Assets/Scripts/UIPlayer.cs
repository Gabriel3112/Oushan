using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{

    Image lifeBar;
    Image staminaBar;

    private void Start()
    {
        lifeBar = GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Image>();
        staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Image>();
    }

    public void LifeBar(float life)
    {
        lifeBar.fillAmount = life / 100;
    }

    public void StaminaBar(float stamina)
    {
        staminaBar.fillAmount = stamina / 100;
    }
}
