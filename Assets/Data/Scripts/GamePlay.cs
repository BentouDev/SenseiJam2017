using UnityEngine;

public class GamePlay : Framework.GameState
{
    protected override void OnStart()
    {
        MainGame.Instance.Controllers.Enable();
    }
}