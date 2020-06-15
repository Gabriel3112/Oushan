using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponStats : MonoBehaviour
{
    public Weapon weapon;
    public GameObject canvas;
    public bool isActivate;
    public Text textNameWeapon;
    public Image imagebulletsInTheComb;
    public Text textfiringRate;
    public Image imageDamage;
    // Start is called before the first frame update
    void Start()
    {
        textNameWeapon = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        imagebulletsInTheComb = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>();
        textfiringRate = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        imageDamage = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Image>();
        textNameWeapon.text = weapon.nameWeapon;
        imagebulletsInTheComb.fillAmount = weapon.bulletsInTheInitialComb / 100;
        imageDamage.fillAmount = weapon.damage / 100;
        textfiringRate.text = weapon.firingRate.ToString() + " /s";
        isActivate = false;
    }

    // Update is called once per frame
    void Update()
    {
        canvas.SetActive(isActivate);
    }
}
