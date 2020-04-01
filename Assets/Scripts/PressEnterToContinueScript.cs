using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using Doozy.Engine.Utils;
using UnityEngine;

public class PressEnterToContinueScript : MonoBehaviour
{
    [SerializeField]
    private GameEventListener _transitionToGameEvent;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            //_transitionToGameEvent.Invoke(_transitionToGameEvent.GameEvent, 0f);
            GameEventMessage.SendEvent(_transitionToGameEvent.GameEvent);
        }
    }
}
