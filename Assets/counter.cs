using UnityEngine;
using TMPro;
using System.Collections;

public class Counter : MonoBehaviour
{
    public int count = 0;
    private TextMeshProUGUI textComponent;
    
    [Header("Counter Settings")]
    public int baseIncrement = 1;       // Базовое значение увеличения
    public int multiplier = 1;          // Текущий множитель
    public int multiplierThreshold = 10; // Каждые 10 очков увеличиваем множитель
    
    [Header("Visual Effects")]
    public float pulseScale = 1.3f;
    public float pulseDuration = 0.3f;
    public Color[] colorCycle = new Color[] { 
        Color.white, 
        Color.yellow, 
        Color.green, 
        Color.cyan, 
        Color.magenta
    };
    
    private Coroutine effectCoroutine;
    private Vector3 originalScale;
    private bool isAnimating = false;
    
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        originalScale = transform.localScale;
        UpdateCounterText();
    }
    
    public void IncreaseCount()
    {
        // Вычисляем, сколько добавить к счетчику с учетом множителя
        int increment = baseIncrement * multiplier;
        count += increment;
        
        // Проверяем, не пора ли увеличить множитель
        UpdateMultiplier();
        
        // Обновляем текст и эффекты
        UpdateCounterText();
        
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
            transform.localScale = originalScale;
        }
        
        effectCoroutine = StartCoroutine(PlayCountEffect());
    }
    
    // Обновляет множитель на основе текущего значения счетчика
    private void UpdateMultiplier()
    {
        // Вычисляем новый множитель на основе текущего значения
        // Например: каждые multiplierThreshold очков увеличиваем множитель на 1
        int newMultiplier = 1 + (count / multiplierThreshold);
        
        // Если множитель изменился, показываем сообщение
        if (newMultiplier > multiplier)
        {
            multiplier = newMultiplier;
            Debug.Log("Множитель увеличен до " + multiplier + "!");
            // Здесь можно добавить дополнительные эффекты при увеличении множителя
        }
    }
    
    private void UpdateCounterText()
    {
        if (textComponent != null)
        {
            // Показываем и значение счетчика, и текущий множитель
            textComponent.text = count.ToString() + "\nх" + multiplier;
        }
    }
    
    private IEnumerator PlayCountEffect()
    {
        isAnimating = true;
        
        textComponent.color = colorCycle[count % colorCycle.Length];
        
        transform.localScale = originalScale;
        Vector3 targetScale = originalScale * pulseScale;
        
        float elapsed = 0;
        while (elapsed < pulseDuration/2)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed/(pulseDuration/2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        elapsed = 0;
        while (elapsed < pulseDuration/2)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed/(pulseDuration/2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = originalScale;
        isAnimating = false;
    }
}