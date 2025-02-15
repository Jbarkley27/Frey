using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;

    public int maxHealth;
    public int currentHealth;
    public int currentBlock;

    public TMP_Text healthText;
    public TMP_Text blockText;
    public TMP_Text maxHealthText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Player Health Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set health to max
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();

        // Set block to 0
        currentBlock = 0;
        blockText.text = currentBlock.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        healthText.text = currentHealth.ToString();
        blockText.text = currentBlock.ToString();
    }

    public void TakeDamage(int damage)
    {
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

    public void Die()
    {
        Debug.Log("Player died");
    }
}
