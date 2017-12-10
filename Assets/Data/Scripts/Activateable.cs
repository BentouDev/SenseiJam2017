using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Activateable : MonoBehaviour
{
    private List<ActionActivator> ActiveActivators = new List<ActionActivator>();
    public TextMeshProUGUI ActivatorMessage;
    public string ActivatorUITag = "Activator";

    public void OnLevelLoaded()
    {
        if (!ActivatorMessage)
        {
            var go = GameObject.FindGameObjectWithTag(ActivatorUITag);
            if (go)
            {
                ActivatorMessage = go.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    void Update()
    {
        if (!ActivatorMessage)
            return;

        ActivatorMessage.text = string.Empty;

        if (!MainGame.Instance.IsPlaying())
            return;
        
        if (!ActiveActivators.Any())
            return;

        ActivatorMessage.text = ActiveActivators.Last().Message;

        if (Input.GetButtonDown("Submit"))
        {
            ActiveActivators.Last().Activate(this);
        }
    }

    public void PushActivator(ActionActivator activator)
    {
        if (!ActiveActivators.Contains(activator))
        {
            // Debug.LogFormat("{0} :: New Activator {1}", Time.realtimeSinceStartup, activator);
            ActiveActivators.Add(activator);
        }
    }

    public void PopActivator(ActionActivator activator)
    {
        if (ActiveActivators.Remove(activator))
        {
            // Debug.LogFormat("{0} :: Lost Activator {1}", Time.realtimeSinceStartup, activator);
        }
    }
}
