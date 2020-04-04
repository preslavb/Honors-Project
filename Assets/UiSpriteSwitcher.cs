using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

/// Attach SpriteSwitcher to game object
public class UiSpriteSwitcher : MonoBehaviour
{
    private const float TRANSITION_TIME = 1.2f;
    
    [SerializeField] private Image _currentImage;
    [SerializeField] private Image _targetImage;

    /// Create a command to use on a sprite
    [YarnCommand("set_sprite")]
    public void UseSprite(string spriteName) {

        // Load the sprite
        var targetImage = spriteName == "none" ? null : Resources.Load<Sprite>(spriteName);

        StartCoroutine(SwitchSpritesCoroutine(targetImage));
    }

    private IEnumerator SwitchSpritesCoroutine(Sprite targetSprite)
    {
        _targetImage.sprite = targetSprite;

        var timer = 0f;

        while (timer < TRANSITION_TIME)
        {
            timer += Time.deltaTime;

            var color = _targetImage.color;

            _currentImage.color = Color.Lerp(Color.clear, _currentImage.sprite ? Color.white : Color.clear, 1 - (timer / TRANSITION_TIME));
            _targetImage.color = Color.Lerp(Color.clear, _targetImage.sprite ? Color.white : Color.clear, timer / TRANSITION_TIME);

            yield return new WaitForEndOfFrame();
        }

        _currentImage.sprite = _targetImage.sprite;
        _targetImage.color = Color.clear;
        _currentImage.color = _currentImage.sprite ? Color.white : Color.clear;
    }
}