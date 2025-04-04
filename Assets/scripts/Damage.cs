using UnityEngine;
using UnityEngine.EventSystems;

public class Damage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private bool applyDamageOnClick = true;
    
    private Player playerReference;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the player in the scene
        playerReference = FindObjectOfType<Player>();
        
        if (playerReference == null)
        {
            Debug.LogError("Player not found in the scene! Make sure there's a GameObject with Player component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Вызывается при клике на объект
    public void OnPointerClick(PointerEventData eventData)
    {
        if (applyDamageOnClick)
        {
            ApplyDamage();
        }
    }
    
    // Можно вызвать напрямую из других скриптов
    public void ApplyDamage(Player player)
    {
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
    
    // Можно вызвать напрямую, если playerReference уже найден
    public void ApplyDamage()
    {
        if (playerReference != null)
        {
            playerReference.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogError("Cannot apply damage: Player reference not found!");
        }
    }
}
