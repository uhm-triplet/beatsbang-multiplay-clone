using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerState playerState;
    public Enemy bossA;
    public Enemy bossB;
    public Enemy bossC;
    public Boss bossD;
    public float playTime = 754f;
    public int stage = 0;
    public int enemyACnt = 0;
    public int enemyBCnt = 0;
    public int enemyCCnt = 0;

    public GameObject gamePanel;

    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI stageTxt;
    // public TextMeshProUGUI killTxt;
    // public TextMeshProUGUI deathTxt;
    public TextMeshProUGUI scoreTxt;

    public TextMeshProUGUI playerHealthTxt;
    public TextMeshProUGUI playerAmmoTxt;
    // public TextMeshProUGUI playerCoinTxt;

    public TextMeshProUGUI EnemyATxt;
    public TextMeshProUGUI EnemyBTxt;
    public TextMeshProUGUI EnemyCTxt;

    public Image hammerImg;
    public Image handGunImg;
    public Image subMachineGunImg;
    public TextMeshProUGUI weaponAmmoTxt;
    public TextMeshProUGUI grenadeCountTxt;


    public Image bossAImg;
    public Image bossBImg;
    public Image bossCImg;
    public Image bossDImg;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    public Transform playerPosition;
    public CharacterController controller;

    void Awake()
    {
        weaponAmmoTxt.text = "   - / -";
        hammerImg.color = new Color(1, 1, 1, 0);
        handGunImg.color = new Color(1, 1, 1, 0);
        subMachineGunImg.color = new Color(1, 1, 1, 0);
        bossAImg.color = new Color(1, 1, 1, 0);
        bossBImg.color = new Color(1, 1, 1, 0);
        bossCImg.color = new Color(1, 1, 1, 0);
        bossDImg.color = new Color(1, 1, 1, 0);
    }
    void Update()
    {
        playTime -= Time.deltaTime;

    }

    public void NextStage()
    {
        stage++;
        switch (stage)
        {
            case 1:
                controller.enabled = false;
                playerPosition.position = new Vector3(98, 1, 34);
                controller.enabled = true;
                break;
            case 2:

                break;
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        int minute = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        timerTxt.text = string.Format("{0:00}", minute) + ":" + string.Format("{0:00}", second);
        stageTxt.text = "Stage : " + stage;

        playerHealthTxt.text = playerState.health + " / " + playerState.maxHealth;
        playerAmmoTxt.text = playerState.ammo + " / " + playerState.maxAmmo;
        scoreTxt.text = playerState.score.ToString();

        EnemyATxt.text = enemyACnt.ToString();
        EnemyBTxt.text = enemyBCnt.ToString();
        EnemyCTxt.text = enemyCCnt.ToString();

        // killTxt.text = playerState.kill.ToString();
        // deathTxt.text = playerState.death.ToString();
        grenadeCountTxt.text = playerState.hasGrenades + " / " + playerState.maxHasGrenades;

        if (playerState.hasWeapon == 0)
        {
            weaponAmmoTxt.text = "   - / -";
            hammerImg.color = new Color(1, 1, 1, 1);
            handGunImg.color = new Color(1, 1, 1, 0);
            subMachineGunImg.color = new Color(1, 1, 1, 0);
        }
        else if (playerState.hasWeapon == 1)
        {
            weaponAmmoTxt.text = "  " + playerState.equipWeapon.curAmmo + " / " + playerState.equipWeapon.maxAmmo;
            hammerImg.color = new Color(1, 1, 1, 0);
            handGunImg.color = new Color(1, 1, 1, 1);
            subMachineGunImg.color = new Color(1, 1, 1, 0);
        }
        else if (playerState.hasWeapon == 2)
        {
            weaponAmmoTxt.text = playerState.equipWeapon.curAmmo + " / " + playerState.equipWeapon.maxAmmo;
            hammerImg.color = new Color(1, 1, 1, 0);
            handGunImg.color = new Color(1, 1, 1, 0);
            subMachineGunImg.color = new Color(1, 1, 1, 1);
        }


        if (stage == 1)
        {
            bossHealthBar.localScale = new Vector3((float)bossA.currentHealth / bossA.maxHealth, 1, 1);
            bossAImg.color = new Color(1, 1, 1, 1);
            bossBImg.color = new Color(1, 1, 1, 0);
            bossCImg.color = new Color(1, 1, 1, 0);
            bossDImg.color = new Color(1, 1, 1, 0);
        }
        else if (stage == 2)
        {
            bossHealthBar.localScale = new Vector3((float)bossB.currentHealth / bossB.maxHealth, 1, 1);
            bossAImg.color = new Color(1, 1, 1, 0);
            bossBImg.color = new Color(1, 1, 1, 1);
            bossCImg.color = new Color(1, 1, 1, 0);
            bossDImg.color = new Color(1, 1, 1, 0);
        }
        else if (stage == 3)
        {
            bossHealthBar.localScale = new Vector3((float)bossC.currentHealth / bossC.maxHealth, 1, 1);
            bossAImg.color = new Color(1, 1, 1, 0);
            bossBImg.color = new Color(1, 1, 1, 0);
            bossCImg.color = new Color(1, 1, 1, 1);
            bossDImg.color = new Color(1, 1, 1, 0);
        }
        else if (stage == 4)
        {
            bossHealthBar.localScale = new Vector3((float)bossD.currentHealth / bossD.maxHealth, 1, 1);
            bossAImg.color = new Color(1, 1, 1, 0);
            bossBImg.color = new Color(1, 1, 1, 0);
            bossCImg.color = new Color(1, 1, 1, 0);
            bossDImg.color = new Color(1, 1, 1, 1);
        }

    }
}
