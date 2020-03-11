using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

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

    private float _defaultTextSpeed;

    public bool WaitingForContinue { get; set; }

    private void Awake()
    {
        _dialogueUi.onOptionsStart = new UnityEventOptionSet();
        _dialogueUi.onOptionsStart.AddListener(CreateRequiredOptions);

        _defaultTextSpeed = _dialogueUi.textSpeed;
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
            _dialogueUi.optionButtons.Add(Instantiate(_buttonPrefab, container.transform));
        }
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
        if (WaitingForContinue)
        {
            _dialogueUi.MarkLineComplete();
        }

        // Skip to the end of the line
        else
        {
            _dialogueUi.textSpeed = 0;
        }
    }

    public void ResetTextSpeed()
    {
        _dialogueUi.textSpeed = _defaultTextSpeed;
    }
}
