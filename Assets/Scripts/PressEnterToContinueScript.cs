using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using Doozy.Engine.Utils;
using UnityEngine;

public class PressEnterToContinueScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            GameEventManager.ProcessGameEvent(new GameEventMessage("Test"), true);
        }
    }
}
