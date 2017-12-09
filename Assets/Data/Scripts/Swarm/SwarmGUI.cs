using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwarmGUI : MonoBehaviour
{
    public SwarmController Swarm;
    public TextMeshProUGUI CurrentState;

    private void Update()
    {
        if (!Swarm)
            return;

        CurrentState.text = Swarm.CurrentState.ToString();
    }
}
