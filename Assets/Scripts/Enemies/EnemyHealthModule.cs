using TMPro;
using UnityEngine;


public class EnemyHealthModule : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public TMP_Text healthText;


    public int currentBlock = 0;
    public int maxBlock = 100;
    public TMP_Text blockText;
    public GameObject statusEffectPanel;
    public GameObject healthPanel;
    public HitFlash hitFlash;

    private void Start()
    {
        currentHealth = maxHealth;
        currentBlock = 0;

        UpdateUI();
        HideUI();
    }

    public void TakeDamage(int damage)
    {
        hitFlash.Flash(GlobalDataStore.instance.hitFlashMaterial_Base, .1f);

        if (currentBlock > 0)
        {
            currentBlock -= damage;
            if (currentBlock < 0)
            {
                currentHealth += currentBlock;
                currentBlock = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        healthText.text = currentHealth.ToString();
        blockText.text = currentBlock.ToString();
    }

    public void ShowUI()
    {
        healthPanel.SetActive(true);
        statusEffectPanel.SetActive(true);
    }

    public void HideUI()
    {
        healthPanel.SetActive(false);
        statusEffectPanel.SetActive(false);
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}