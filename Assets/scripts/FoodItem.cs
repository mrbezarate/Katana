using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FoodItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int healAmount = 20;
    private Player playerReference;
    
    private void Start()
    {
        // Find the player in the scene
        playerReference = FindObjectOfType<Player>();
        
        if (playerReference == null)
        {
            Debug.LogError("Player not found in the scene! Make sure there's a GameObject with Player component.");
        }
    }
    
    // This method is called when the food item is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        UseItem();
    }
    
    // Can also be called from a button
    public void UseItem()
    {
        if (playerReference != null)
        {
            playerReference.Heal(healAmount);
        }
        else
        {
            Debug.LogError("Cannot use item: Player reference not found!");
        }
    }
}