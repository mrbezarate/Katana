using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ShopController : MonoBehaviour, IPointerClickHandler
{
    [Header("Primary Shop")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private RectTransform shopPanelRect;
    
    [Header("Secondary Shop")]
    [SerializeField] private GameObject secondaryShopPanel;
    [SerializeField] private RectTransform secondaryShopPanelRect;
    [SerializeField] private bool useSecondaryShop = false;
    [SerializeField] private KeyCode secondaryShopKey = KeyCode.Y;
    
    [Header("Settings")]
    [SerializeField] private bool openShopOnClick = true;
    [SerializeField] private bool enableKeyboardToggle = true;
    [SerializeField] private KeyCode openShopKey = KeyCode.Tab;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.25f;
    [SerializeField] private AnimationDirection shopPanelDirection = AnimationDirection.Right;
    [SerializeField] private AnimationDirection secondaryShopDirection = AnimationDirection.Left;
    
    private bool isShopOpen = false;
    private bool isSecondaryShopOpen = false;
    
    // Store original positions
    private Vector2 shopPanelOriginalPos;
    private Vector2 secondaryShopPanelOriginalPos;
    
    public enum AnimationDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }
    
    private void Start()
    {
        // Make sure shops are closed at start
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            
            // If shopPanelRect not assigned, try to get it
            if (shopPanelRect == null)
                shopPanelRect = shopPanel.GetComponent<RectTransform>();
                
            // Store original position
            if (shopPanelRect != null)
                shopPanelOriginalPos = shopPanelRect.anchoredPosition;
        }
        
        if (secondaryShopPanel != null)
        {
            secondaryShopPanel.SetActive(false);
            
            // If secondaryShopPanelRect not assigned, try to get it
            if (secondaryShopPanelRect == null)
                secondaryShopPanelRect = secondaryShopPanel.GetComponent<RectTransform>();
                
            // Store original position
            if (secondaryShopPanelRect != null)
                secondaryShopPanelOriginalPos = secondaryShopPanelRect.anchoredPosition;
        }
    }
    
    private void Update()
    {
        // Toggle primary shop when key is pressed if enabled
        if (enableKeyboardToggle && Input.GetKeyDown(openShopKey))
        {
            ToggleShop();
        }
        
        // Toggle secondary shop when key is pressed if enabled
        if (useSecondaryShop && enableKeyboardToggle && Input.GetKeyDown(secondaryShopKey))
        {
            ToggleSecondaryShop();
        }
    }
    
    // Implement IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        if (openShopOnClick)
        {
            // Toggle both shops when clicked
            if (isShopOpen || isSecondaryShopOpen)
            {
                CloseBothShops();
            }
            else
            {
                OpenBothShops();
            }
        }
    }
    
    // Open both shops at once
    public void OpenBothShops()
    {
        OpenShop();
        if (useSecondaryShop)
        {
            OpenSecondaryShop();
        }
    }
    
    // Close both shops at once
    public void CloseBothShops()
    {
        CloseShop();
        CloseSecondaryShop();
    }
    
    // Primary shop functions
    public void ToggleShop()
    {
        if (shopPanel != null)
        {
            if (isShopOpen)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
    }
    
    public void OpenShop()
    {
        if (shopPanel != null && !isShopOpen)
        {
            isShopOpen = true;
            shopPanel.SetActive(true);
            
            // Start animation
            if (shopPanelRect != null)
            {
                StartCoroutine(AnimatePanel(shopPanelRect, shopPanelDirection, true, shopPanelOriginalPos));
            }
            
            Debug.Log("Shop opened");
        }
    }
    
    public void CloseShop()
    {
        if (shopPanel != null && isShopOpen)
        {
            if (shopPanelRect != null)
            {
                StartCoroutine(AnimatePanel(shopPanelRect, shopPanelDirection, false, shopPanelOriginalPos, () => {
                    isShopOpen = false;
                    shopPanel.SetActive(false);
                }));
            }
            else
            {
                isShopOpen = false;
                shopPanel.SetActive(false);
            }
            
            Debug.Log("Shop closed");
        }
    }
    
    // Secondary shop functions
    public void ToggleSecondaryShop()
    {
        if (secondaryShopPanel != null)
        {
            if (isSecondaryShopOpen)
            {
                CloseSecondaryShop();
            }
            else
            {
                OpenSecondaryShop();
            }
        }
    }
    
    public void OpenSecondaryShop()
    {
        if (secondaryShopPanel != null && !isSecondaryShopOpen)
        {
            isSecondaryShopOpen = true;
            secondaryShopPanel.SetActive(true);
            
            // Start animation
            if (secondaryShopPanelRect != null)
            {
                StartCoroutine(AnimatePanel(secondaryShopPanelRect, secondaryShopDirection, true, secondaryShopPanelOriginalPos));
            }
            
            Debug.Log("Secondary shop opened");
        }
    }
    
    public void CloseSecondaryShop()
    {
        if (secondaryShopPanel != null && isSecondaryShopOpen)
        {
            if (secondaryShopPanelRect != null)
            {
                StartCoroutine(AnimatePanel(secondaryShopPanelRect, secondaryShopDirection, false, secondaryShopPanelOriginalPos, () => {
                    isSecondaryShopOpen = false;
                    secondaryShopPanel.SetActive(false);
                }));
            }
            else
            {
                isSecondaryShopOpen = false;
                secondaryShopPanel.SetActive(false);
            }
            
            Debug.Log("Secondary shop closed");
        }
    }
    
    // Animation coroutine
    private IEnumerator AnimatePanel(RectTransform panelRect, AnimationDirection direction, bool opening, Vector2 originalPos, System.Action onComplete = null)
    {
        // Reset to original position
        panelRect.anchoredPosition = originalPos;
        
        // Determine animation direction
        Vector2 offset = new Vector2(0, 0);
        switch (direction)
        {
            case AnimationDirection.Left:
                offset = new Vector2(-100, 0);
                break;
            case AnimationDirection.Right:
                offset = new Vector2(100, 0);
                break;
            case AnimationDirection.Top:
                offset = new Vector2(0, 100);
                break;
            case AnimationDirection.Bottom:
                offset = new Vector2(0, -100);
                break;
        }
        
        // Animation positions
        Vector2 normalPos = originalPos;
        Vector2 offsetPos = originalPos + offset;
        
        // Set initial and target positions
        Vector2 startPos = opening ? offsetPos : normalPos;
        Vector2 endPos = opening ? normalPos : offsetPos;
        
        // Set initial scale
        Vector3 startScale = opening ? Vector3.zero : Vector3.one;
        Vector3 endScale = opening ? Vector3.one : Vector3.zero;
        
        // Set initial position and scale
        panelRect.anchoredPosition = startPos;
        panelRect.localScale = startScale;
        
        // Animate
        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            
            // Smooth the animation with easing function
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            // Apply position and scale
            panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);
            panelRect.localScale = Vector3.Lerp(startScale, endScale, smoothT);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure final values are set
        panelRect.anchoredPosition = endPos;
        panelRect.localScale = endScale;
        
        // Call completion callback if provided
        if (onComplete != null)
        {
            onComplete();
        }
        else if (!opening)
        {
            // Reset position if closing without callback
            panelRect.anchoredPosition = originalPos;
        }
    }
} 