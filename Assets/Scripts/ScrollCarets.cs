using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ScrollCarets : MonoBehaviour
{
    [SerializeField] private float step = 0.25f;
    
    [SerializeField] private List<GameObject> _upCaret;
    [SerializeField] private List<GameObject> _downCaret;

    [SerializeField] private ScrollRect _scrollView;

    public void UpdateCarets()
    {
        _upCaret.ForEach(go => go.SetActive(true));
        _downCaret.ForEach(go => go.SetActive(true));

        var scrollValue = _scrollView.normalizedPosition;
        
        if (!_scrollView.vertical)
        {
            _upCaret.ForEach(go => go.SetActive(false));
            _downCaret.ForEach(go => go.SetActive(false));
        }
        
        if (scrollValue.y <= 0.05f) _downCaret.ForEach(go => go.SetActive(false));
        if (scrollValue.y >= 0.95f) _upCaret.ForEach(go => go.SetActive(false));
    }

    public void UpdateWithInput(float value)
    {
        var currentPositionNormal = _scrollView.normalizedPosition.y * _scrollView.content.rect.height;
        var newPositionNormal = currentPositionNormal + (step * value);
        var newPositionNormalized = Mathf.Clamp(newPositionNormal / _scrollView.content.rect.height, 0f, 1f);
        
        _scrollView.normalizedPosition = new Vector2(0, newPositionNormalized);
    }
}
