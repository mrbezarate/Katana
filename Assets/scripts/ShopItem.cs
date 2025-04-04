using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public enum ItemType
    {
        Health,
        Armor
    }
    
    [Header("Item Properties")]
    [SerializeField] private string itemName = "Item";
    [SerializeField] private int price = 10;
    [SerializeField] private ItemType type = ItemType.Health;
    [SerializeField] private int effectAmount = 20;
    [SerializeField] private int quantity = 1;
    [SerializeField] private bool hasUnlimitedQuantity = false;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Button buyButton;
    
    private Player playerReference;
    private PlayerInventory inventoryReference;
    
    private void Start()
    {
        // Find the player in the scene
        playerReference = FindObjectOfType<Player>();
        
        if (playerReference == null)
        {
            Debug.LogError("Player not found in the scene! Make sure there's a GameObject with Player component.");
        }
        
        // Find inventory if we're selling armor
        if (type == ItemType.Armor)
        {
            inventoryReference = FindObjectOfType<PlayerInventory>();
            
            if (inventoryReference == null)
            {
                Debug.LogWarning("PlayerInventory not found in the scene! Armor items will apply directly instead of being added to inventory.");
            }
        }
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        // Setup UI elements
        if (nameText != null)
        {
            nameText.text = itemName;
        }
        
        if (priceText != null)
        {
            priceText.text = $"${price}";
        }
        
        if (quantityText != null)
        {
            quantityText.text = hasUnlimitedQuantity ? "∞" : quantity.ToString();
            quantityText.gameObject.SetActive(true);
        }
        
        // Add click listener to the buy button if it exists
        if (buyButton != null && buyButton.onClick.GetPersistentEventCount() == 0)
        {
            buyButton.onClick.AddListener(TryPurchase);
        }
        
        // Enable/disable buy button based on quantity
        if (buyButton != null)
        {
            buyButton.interactable = hasUnlimitedQuantity || quantity > 0;
        }
    }
    
    // This method is called when the shop item is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        TryPurchase();
    }
    
    // Try to purchase the item
    public void TryPurchase()
    {
        if (playerReference == null)
        {
            Debug.LogError("Cannot purchase item: Player reference not found!");
            return;
        }
        
        // Check if we have stock
        if (!hasUnlimitedQuantity && quantity <= 0)
        {
            Debug.Log($"Out of stock: {itemName}");
            return;
        }
        
        // Check if player has enough money
        if (playerReference.SpendMoney(price))
        {
            // Purchase successful, give the item effect to player
            ApplyItemEffect();
            
            // Reduce quantity if not unlimited
            if (!hasUnlimitedQuantity)
            {
                quantity--;
                UpdateUI();
                
                // Hide item if out of stock
                if (quantity <= 0)
                {
                    Debug.Log($"{itemName} is now out of stock!");
                    gameObject.SetActive(false);
                }
            }
            
            Debug.Log($"Successfully purchased {itemName} for ${price}!");
        }
        else
        {
            Debug.Log($"Not enough money to buy {itemName}. It costs ${price}.");
        }
    }
    
    private void ApplyItemEffect()
    {
        switch (type)
        {
            case ItemType.Health:
                playerReference.Heal(effectAmount);
                break;
                
            case ItemType.Armor:
                // If we have inventory reference, add to inventory
                if (inventoryReference != null)
                {
                    inventoryReference.AddArmorItem(effectAmount, itemName);
                    Debug.Log($"Added {itemName} to inventory with {effectAmount} armor value.");
                }
                else
                {
                    // Otherwise, just add armor directly to player
                    playerReference.AddArmor(effectAmount);
                }
                break;
                
            default:
                Debug.LogWarning($"Unknown item type for {itemName}");
                break;
        }
    }
} 