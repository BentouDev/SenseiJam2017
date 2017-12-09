using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : Framework.Game<MainGame>
{
    public GUIController GUI;
    
    public override bool IsPlaying()
    {
        return CurrentState is GamePlay;
    }
}
