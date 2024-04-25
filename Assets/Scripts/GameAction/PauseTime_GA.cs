using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime_GA : GameAction
{
    public override void Action()
    {
        Time.timeScale = 0;
    }
    public override void DeAction()
    {
        Time.timeScale = 1;
    }
    public override void ResetToDefault()
    {
        //nothing
    }
}
