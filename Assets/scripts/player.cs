using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 50;
    [SerializeField] private int money = 0;
    [SerializeField] private int maxArmor = 100;
    [SerializeField] private int currentArmor = 0;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider armorSlider;

    // Getter for current money amount
    public int Money => money;
    
    // Getter for current armor amount
    public int Armor => currentArmor;

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
    
    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: {money}";
        }
    }
    
    private void UpdateArmorUI()
    {
        if (armorText != null)
        {
            armorText.text = $"{currentArmor}/{maxArmor}";
        }
        
        if (armorSlider != null)
        {
            armorSlider.maxValue = maxArmor;
            armorSlider.value = currentArmor;
        }
    }
    
    // Add money to the player
    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            money += amount;
            Debug.Log($"Received {amount} money. Current balance: {money}");
            UpdateMoneyUI();
        }
    }
    
    // Spend money. Returns true if successful, false if not enough money
    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
            return true;
            
        if (money >= amount)
        {
            money -= amount;
            Debug.Log($"Spent {amount} money. Current balance: {money}");
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log($"Not enough money! Required: {amount}, available: {money}");
            return false;
        }
    }
    
    // Add armor to the player
    public void AddArmor(int amount)
    {
        if (amount > 0)
        {
            // Add armor amount
            currentArmor += amount;
            
            // Make sure we don't exceed max armor
            if (currentArmor > maxArmor)
            {
                currentArmor = maxArmor;
            }
            
            Debug.Log($"Added {amount} armor. Current armor: {currentArmor}/{maxArmor}");
            UpdateArmorUI();
        }
    }
    
    // Remove armor from the player, returns the amount of armor removed
    public int RemoveArmor(int amount)
    {
        if (amount <= 0)
            return 0;
            
        int armorRemoved = Mathf.Min(amount, currentArmor);
        currentArmor -= armorRemoved;
        
        Debug.Log($"Removed {armorRemoved} armor. Current armor: {currentArmor}/{maxArmor}");
        UpdateArmorUI();
        
        return armorRemoved;
    }
    
    public void Heal(int healAmount)
    {
        // Check if player needs healing
        if (currentHealth < maxHealth)
        {
            // Add healing amount
            currentHealth += healAmount;
            
            // Make sure we don't exceed max health
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            Debug.Log($"Healed for {healAmount}. Current health: {currentHealth}/{maxHealth}");
            UpdateHealthUI();
        }
        else
        {
            Debug.Log("Already at full health!");
        }
    }
    
    public void TakeDamage(int damageAmount)
    {
        // First try to absorb damage with armor
        int remainingDamage = damageAmount;
        
        if (currentArmor > 0)
        {
            int armorAbsorbed = RemoveArmor(remainingDamage);
            remainingDamage -= armorAbsorbed;
            Debug.Log($"Armor absorbed {armorAbsorbed} damage.");
        }
        
        // Apply remaining damage to health if there's any
        if (remainingDamage > 0)
        {
            // Reduce health by damage amount
            currentHealth -= remainingDamage;
            
            // Make sure health doesn't go below 0
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            
            Debug.Log($"Took {remainingDamage} damage. Current health: {currentHealth}/{maxHealth}");
            
            // Check if player is dead
            if (currentHealth <= 0)
            {
                Debug.Log("Player died!");
                // Add death logic here if needed
            }
            
            UpdateHealthUI();
        }
    }
    
    private void Start()
    {
        UpdateHealthUI();
        UpdateMoneyUI();
        UpdateArmorUI();
    }
}