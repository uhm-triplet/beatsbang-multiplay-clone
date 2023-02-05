using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerState playerState;
    public Enemy enemy;
    public float playTime = 754f;

    public GameObject gamePanel;

    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI killTxt;
    public TextMeshProUGUI deathTxt;

    public TextMeshProUGUI playerHealthTxt;
    public TextMeshProUGUI playerAmmoTxt;
    // public TextMeshProUGUI playerCoinTxt;

    public Image hammerImg;
    public Image handGunImg;
    public Image subMachineGunImg;
    public TextMeshProUGUI weaponAmmoTxt;
    public TextMeshProUGUI grenadeCountTxt;

    public RectTransform healthGroup;
    public RectTransform healthBar;

    void Update()
    {
        playTime -= Time.deltaTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int minute = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        timerTxt.text = string.Format("{0:00}", minute) + ":" + string.Format("{0:00}", second);

        playerHealthTxt.text = playerState.health + " / " + playerState.maxHealth;
        // playerAmmoTxt.text = playerState.ammo + " / " + playerState.maxAmmo;
        // killTxt.text = playerState.kill.ToString();
        // deathTxt.text = playerState.death.ToString();
        grenadeCountTxt.text = playerState.hasGrenades + " / " + playerState.maxHasGrenades;

        // if (playerState.hasWeapon == -1)
        // {
        //     weaponAmmoTxt.text = "- / -";
        //     hammerImg.color = new Color(1, 1, 1, 0);
        //     handGunImg.color = new Color(1, 1, 1, 0);
        //     subMachineGunImg.color = new Color(1, 1, 1, 0);
        // }
        // else if (playerState.hasWeapon == 0)
        // {
        //     weaponAmmoTxt.text = "- / -";
        //     hammerImg.color = new Color(1, 1, 1, 1);
        //     handGunImg.color = new Color(1, 1, 1, 0);
        //     subMachineGunImg.color = new Color(1, 1, 1, 0);
        // }
        // else if (playerState.hasWeapon == 1)
        // {
        //     weaponAmmoTxt.text = playerState.equipWeapon.curAmmo + " / " + playerState.equipWeapon.maxAmmo;
        //     hammerImg.color = new Color(1, 1, 1, 0);
        //     handGunImg.color = new Color(1, 1, 1, 1);
        //     subMachineGunImg.color = new Color(1, 1, 1, 0);
        // }
        // else if (playerState.hasWeapon == 2)
        // {
        //     weaponAmmoTxt.text = playerState.equipWeapon.curAmmo + " / " + playerState.equipWeapon.maxAmmo;
        //     hammerImg.color = new Color(1, 1, 1, 0);
        //     handGunImg.color = new Color(1, 1, 1, 0);
        //     subMachineGunImg.color = new Color(1, 1, 1, 1);
        // }


    }
}
