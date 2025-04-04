using UnityEngine;
using UnityEngine.EventSystems;

public class MoneyPickup : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int moneyAmount = 10;
    [SerializeField] private bool destroyOnPickup = true;
    
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
    
    // This method is called when the money pickup is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        PickupMoney();
    }
    
    public void PickupMoney()
    {
        if (playerReference != null)
        {
            playerReference.AddMoney(moneyAmount);
            Debug.Log($"Picked up {moneyAmount} money!");
            
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogError("Cannot pickup money: Player reference not found!");
        }
    }
} 