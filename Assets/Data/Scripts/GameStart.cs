using UnityEngine;

public class GameStart : Framework.GameState
{
    protected override void OnStart()
    {
        MainGame.Instance.Controllers.Init();
        
        MainGame.Instance.SwitchState<GamePlay>();
    }
}