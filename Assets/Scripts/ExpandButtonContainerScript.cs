using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandButtonContainerScript : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
    [SerializeField] private LayoutElement _layoutElement;
    
    [SerializeField] private float _pixelsPerSecond = 150;

    private void Update()
    {
        if (_layoutElement.preferredHeight < _verticalLayoutGroup.preferredHeight)
            _layoutElement.preferredHeight += _pixelsPerSecond * Time.deltaTime;
    }
}
