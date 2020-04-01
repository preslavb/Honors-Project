using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeIconScript : MonoBehaviour
{
    private UIButton _button;

    [SerializeField]
    private AudioMixer _audioMixer;

    private void Awake()
    {
        _button = GetComponent<UIButton>();
    }

    public void UpdateButton(float value)
    {
        if (value > -0.05f)
            _button.TextMeshProLabel.text = "\uf028";
        else if (value < -79f) 
            _button.TextMeshProLabel.text = "\uf026";
        else 
            _button.TextMeshProLabel.text = "\uf027";

        _audioMixer.SetFloat("MasterVolume", value);
    }
}
