using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGUI : MonoBehaviour
{
    public GameObject InfoPrefab;
    public RectTransform Panel;

    private RectTransform GUI;

    void Start()
    {
        var go = Instantiate(InfoPrefab, Panel);
        GUI = go.GetComponent<RectTransform>();
    }
    
    void Update()
    {
        var screemPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 anchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Panel, screemPos, Camera.main, out anchPos);
        GUI.anchoredPosition = anchPos;
    }
}
