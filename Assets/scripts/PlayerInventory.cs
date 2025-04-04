using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Player playerReference;
    [SerializeField] private Transform armorSlotsParent;
    [SerializeField] private GameObject armorSlotPrefab;
    
    private List<ArmorItem> equippedArmorItems = new List<ArmorItem>();
    
    private void Start()
    {
        if (playerReference == null)
        {
            playerReference = FindObjectOfType<Player>();
            
            if (playerReference == null)
            {
                Debug.LogError("Player not found in the scene!");
            }
        }
    }
    
    // Add armor item to player's inventory
    public void AddArmorItem(int armorValue, string armorName)
    {
        if (armorSlotsParent == null || armorSlotPrefab == null)
        {
            Debug.LogError("Cannot add armor: armor slot parent or prefab is not assigned!");
            return;
        }
        
        // Create new armor slot
        GameObject armorSlotObj = Instantiate(armorSlotPrefab, armorSlotsParent);
        ArmorItem armorItem = armorSlotObj.GetComponent<ArmorItem>();
        
        if (armorItem == null)
        {
            Debug.LogError("ArmorItem component not found on the instantiated prefab!");
            Destroy(armorSlotObj);
            return;
        }
        
        // Initialize armor item
        armorItem.Initialize(armorValue, armorName, playerReference, this);
        equippedArmorItems.Add(armorItem);
        
        // Add armor to player
        playerReference.AddArmor(armorValue);
    }
    
    // Remove armor item from inventory
    public void RemoveArmorItem(ArmorItem armorItem)
    {
        if (equippedArmorItems.Contains(armorItem))
        {
            // Remove armor from player
            playerReference.RemoveArmor(armorItem.ArmorValue);
            
            // Remove from list
            equippedArmorItems.Remove(armorItem);
            
            // Destroy the UI element
            Destroy(armorItem.gameObject);
        }
    }
}

// This class should be attached to the armor slot prefab
public class ArmorItem : MonoBehaviour
{
    [SerializeField] private Button dropButton;
    [SerializeField] private TMPro.TextMeshProUGUI armorNameText;
    [SerializeField] private TMPro.TextMeshProUGUI armorValueText;
    
    private int armorValue;
    private Player playerRef;
    private PlayerInventory inventoryRef;
    
    public int ArmorValue => armorValue;
    
    public void Initialize(int value, string name, Player player, PlayerInventory inventory)
    {
        armorValue = value;
        playerRef = player;
        inventoryRef = inventory;
        
        // Set UI
        if (armorNameText != null)
        {
            armorNameText.text = name;
        }
        
        if (armorValueText != null)
        {
            armorValueText.text = armorValue.ToString();
        }
        
        // Setup drop button
        if (dropButton != null)
        {
            dropButton.onClick.AddListener(DropArmor);
        }
    }
    
    public void DropArmor()
    {
        if (inventoryRef != null)
        {
            inventoryRef.RemoveArmorItem(this);
        }
    }
} 