using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : MonoBehaviour
{
    public Slider towerHealthSlider;
    public int health;
    public TMP_Text timerText;
    public TMP_Text towerHealth;

    private void Start()
    {
        towerHealthSlider = UIManager.instance.enemyHealthBar;
        timerText = UIManager.instance.time;
        towerHealth = UIManager.instance.towerHealth;
        health = GameManager.instance.enemyTowerHealth;
        towerHealth.text = "TowerHealth : " + health.ToString();
        towerHealthSlider.maxValue = health;
        towerHealthSlider.value = health;
    }

    private void Update()
    {
        timerText.text = "Timer : " + Mathf.FloorToInt(GameManager.instance.timer);
    }

    public void TakingDamage()
    {
        StartCoroutine(TowerHealthDeplete());
    }

    private IEnumerator TowerHealthDeplete()
    {
        health = health - 1;
        transform.DOShakePosition(0.1f, 0.2f).SetEase(Ease.InOutSine);
        towerHealthSlider.value = health;
        towerHealth.text = "TowerHealth : " + health.ToString();
        yield return new WaitForSeconds(0.5f);
        if (health <= 0)
        {
            health = 0;
            GameManager.instance.GoingToResultScreen();
        }
    }
}
