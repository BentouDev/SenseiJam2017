using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwarmGUI : MonoBehaviour
{
    public SwarmController Swarm;
    public TextMeshProUGUI CurrentState;

    public GameObject UnitPrefab;
    public RectTransform ArrayPanel;
    private List<UnitData> Units = new List<UnitData>();

    struct UnitData
    {
        public SwarmController.PawnInfo Unit;
        public UnitUI UI;
    }

    private void Update()
    {
        if (!Swarm)
            return;

        CurrentState.text = Swarm.CurrentState.ToString();
        
        foreach (UnitData unit in Units)
        {
            unit.UI.Id.text = "id: " + unit.Unit.Pawn.GetInstanceID().ToString();
            unit.UI.Hash.text = "offset: " + unit.Unit.FormationOffset;
            unit.UI.State.text = "state:" + unit.Unit.Pawn.CurrentState.name;
        }
    }

    public void UpdatePositioning()
    {
        Units.Clear();

        foreach (SwarmController.PawnInfo info in Swarm.Pawns)
        {
            var go = Instantiate(UnitPrefab);
            go.transform.SetParent(ArrayPanel);
            var ui = go.GetComponent<UnitUI>();
            Units.Add(new UnitData()
            {
                Unit = info,
                UI = ui
            });
        }
    }
}
