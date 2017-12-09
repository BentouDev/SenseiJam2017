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

    void OnGUI()
    {
        GUI.Label(new Rect(10,10,200,30), "JB0 " + Input.GetKey(KeyCode.Joystick1Button0));
        GUI.Label(new Rect(10,30,200,30), "JB1 " + Input.GetKey(KeyCode.Joystick1Button1));
        GUI.Label(new Rect(10,50,200,30), "JB2 " + Input.GetKey(KeyCode.Joystick1Button2));
        GUI.Label(new Rect(10,70,200,30), "JB3 " + Input.GetKey(KeyCode.Joystick1Button3));
    }
}
