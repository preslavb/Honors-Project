using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FadeInDialogueContainerScript : MonoBehaviour
{
    private const float FADE_IN_TIME = 1.2f;
    
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private DialogueRunner _dialogueRunner;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        float timer = 0;

        while (timer < FADE_IN_TIME)
        {
            timer += Time.deltaTime;

            _canvasGroup.alpha = timer / FADE_IN_TIME;
            
            yield return new WaitForEndOfFrame();
        }

        _canvasGroup.alpha = 1;
        _dialogueRunner.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
