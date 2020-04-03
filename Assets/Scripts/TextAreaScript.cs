using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;
using Button = UnityEngine.UI.Button;

public class TextAreaScript : MonoBehaviour
{
    private class UnityEventOptionSet: UnityEvent<OptionSet>{}
    
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private VerticalLayoutGroup _buttonContainerPrefab;
    [SerializeField] private VerticalLayoutGroup _textContainerPrefab;

    [SerializeField] private VerticalLayoutGroup _instantiationTarget;

    [SerializeField] private DialogueUI _dialogueUi;
    [SerializeField] private DialogueRunner _dialogueRunner;
    
    private TextMeshProUGUI _currentActiveText;

    public TextMeshProUGUI CurrentActiveText => _currentActiveText;

    private float _defaultTextSpeed;

    public bool WaitingForContinue { get; set; }

    private void Awake()
    {
        _dialogueUi.onOptionsStart = new UnityEventOptionSet();
        _dialogueUi.onOptionsStart.AddListener(CreateRequiredOptions);
        _dialogueUi.getProcessedTextMaxCharacters += () => 
            CurrentActiveText.textInfo.characterCount;

        _dialogueUi.onLineUpdateCharCount = new DialogueUI.IntUnityEvent();
        _dialogueUi.onLineUpdateCharCount.AddListener((int maxCharacters) => 
            CurrentActiveText.maxVisibleCharacters = maxCharacters);

        _defaultTextSpeed = _dialogueUi.textSpeed;
    }

    private void Update()
    {
        int keyPressed = -1;
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) keyPressed = 1;
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) keyPressed = 2;
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) keyPressed = 3;
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) keyPressed = 4;
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)) keyPressed = 5;
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)) keyPressed = 6;
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7)) keyPressed = 7;
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8)) keyPressed = 8;
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9)) keyPressed = 9;

        if (keyPressed > 0 && keyPressed <= _dialogueUi.optionButtons.Count)
        {
            _dialogueUi.optionButtons[keyPressed - 1].onClick?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Continue();
        }
    }

    public void ShrinkText()
    {
        foreach (Transform layoutElementTransform in _instantiationTarget.transform)
        {
            Destroy(layoutElementTransform.GetComponent<LayoutElement>());
        }
    }

    public void AppendText()
    {
        var textContainer = Instantiate(_textContainerPrefab, _instantiationTarget.transform);
        _currentActiveText = Instantiate(_textPrefab, textContainer.transform);
        _currentActiveText.maxVisibleCharacters = 0;

        var textContainerLayoutElement = textContainer.GetComponent<LayoutElement>();
        var instantiationTargetLayoutElement = _instantiationTarget.GetComponent<LayoutElement>();
        
        textContainerLayoutElement.minHeight = instantiationTargetLayoutElement.minHeight - (_instantiationTarget.padding.bottom);
    }

    [YarnCommand("clear_history")]
    public void ClearText()
    {
        foreach (Transform o in _instantiationTarget.transform)
        {
            Destroy(o.gameObject);
        }
    }

    public void FillActiveText(string value)
    {
        _currentActiveText.text = value;
    }

    public void CreateRequiredOptions(Yarn.OptionSet optionSet)
    {
        _dialogueUi.optionButtons = new List<Button>(optionSet.Options.Length);
        
        // Get the preferred height of the container before we instantiated the buttons
        var preferredHeight = _currentActiveText.transform.parent.GetComponent<VerticalLayoutGroup>().preferredHeight;
        var startingContainerHeight = Mathf.Clamp(_instantiationTarget.GetComponent<LayoutElement>().minHeight - (preferredHeight + _instantiationTarget.padding.bottom), 0, Mathf.Infinity);
        
        var container = Instantiate(_buttonContainerPrefab, _currentActiveText.transform.parent);
        container.GetComponent<LayoutElement>().preferredHeight = startingContainerHeight;

        for (var i = 0; i < optionSet.Options.Length; i++)
        {
            var button = Instantiate(_buttonPrefab, container.transform);
            _dialogueUi.optionButtons.Add(button);

            button.onClick.AddListener(() =>
            {
                // When we click the button, we should display the response in the history
                ShrinkText();
                AppendText();
                FillActiveText($"You: {button.GetComponentsInChildren<TextMeshProUGUI>()[1].text}");
                _currentActiveText.maxVisibleCharacters = 300;
                _currentActiveText.color = new Color(0xFF / 255f, 0xE8 / 255f, 0x23 / 255f);
            });
        }

        // Scroll to the bottom of the view
        GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

    public void RemoveOptions()
    {
        if (_dialogueUi.optionButtons.Count > 0)
            Destroy(_dialogueUi.optionButtons[0].transform.parent.gameObject);

        _dialogueUi.optionButtons.Clear();
    }

    public void Continue()
    {
        // Check if we're done with the current line
        _dialogueUi.MarkLineComplete();
    }

    public void ResetTextSpeed()
    {
        _dialogueUi.textSpeed = _defaultTextSpeed;
    }
}
