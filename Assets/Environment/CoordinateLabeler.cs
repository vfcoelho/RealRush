using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]//Ensure the specified component's attachment to game object when this script is attached
[ExecuteAlways]//Will execute in edit mode (unity) and play mode (game)
public class CoordinateLabeler : MonoBehaviour
{
    [Header("Debug Configurations")]
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);
    GridManager gridManager;

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        DisplayCoordinates();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)//Check if we are in play mode
        {
            //only execute in unity (edit mode)
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;
        }
        SetLabelColor();
        ToggleLabels();
    }
    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }
    void SetLabelColor()
    {
        // label.color = Color.red;
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if (node == null) { return; }

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }
    void DisplayCoordinates()
    {
        //UnityEditor can't be used on finished built games. To ignore this script during build process, drag it to the "Editor" folder
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        label.text = $"{coordinates.x},{coordinates.y}";
    }
    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
