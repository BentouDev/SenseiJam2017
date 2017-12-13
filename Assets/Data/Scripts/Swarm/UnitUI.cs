using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    public TextMeshProUGUI Id;
    public TextMeshProUGUI Hash;
    public TextMeshProUGUI State;
    public Image Background;

    private int lastId = -1;
    public void SetId(int id)
    {
        if (id != lastId)
        {
            lastId = id;
            Id.text = $"id: {lastId}";
        }
    }
    
    private int lastHash = -1;
    public void SetHash(int hash)
    {
        if (hash != lastHash)
        {
            lastHash = hash;
            Hash.text = $"0<size=6>x</size>{(uint)lastHash}";
        }
    }
    
    private string lastState;
    public void SetState(string state)
    {
        if (state != lastState)
        {
            lastState = state;
            State.text = lastState;
        }
    }
}
