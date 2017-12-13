using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwarmGUI : MonoBehaviour
{
    public Gradient BackgroundColor;
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
            unit.UI.SetId(unit.Unit.Pawn.GetInstanceID());
            unit.UI.SetHash(unit.Unit.FormationOffset.GetHashCode());
            unit.UI.SetState(unit.Unit.Pawn.CurrentState.GetType().Name.ToLower());
            
            unit.UI.Background.color = BackgroundColor.Evaluate
            (
                1 - ((float) unit.Unit.Pawn.Damageable.CurrentHealth / (float) unit.Unit.Pawn.Damageable.MaxHealth)
            );
        }
    }

    public void OnUpdatePositioning()
    {
        foreach (UnitData data in Units)
        {
            DestroyObject(data.UI.gameObject);
        }
        
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
