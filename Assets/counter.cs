using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    public Text counterText;
    public Button actionButton;
    public Button modeButton;

    private float _counter = 100f;
    private CounterMode _currentMode = CounterMode.Subtraction;

    private enum CounterMode { Subtraction, Multiplication, Division }

    void Start()
    {
        UpdateCounterText();
        actionButton.onClick.AddListener(ApplyOperation);
        modeButton.onClick.AddListener(SwitchMode);
    }

    private void ApplyOperation()
    {
        switch (_currentMode)
        {
            case CounterMode.Subtraction:
                _counter -= 10f;
                break;
            case CounterMode.Multiplication:
                _counter *= 1.5f;
                break;
            case CounterMode.Division:
                _counter /= 2f;
                break;
        }
        UpdateCounterText();
    }

    private void SwitchMode()
    {
        _currentMode = (CounterMode)(((int)_currentMode + 1) % 3);
        modeButton.GetComponentInChildren<Text>().text = _currentMode.ToString();
    }

    private void UpdateCounterText()
    {
        counterText.text = "Counter: " + _counter.ToString("F2");
    }
}
