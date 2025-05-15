using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using Unity.VisualScripting;

public class HoverSystem : MonoBehaviour
{

    [SerializeField] private GameObject player;

    [SerializeField] private Tilemap groundTilemap;

    [Header("Farmland")]
    [SerializeField] private Tilemap farmlandTilemap;
    [SerializeField] private TileBase farmlandTile;
    private readonly Dictionary<Vector3Int, FarmlandData> farmlandDictionnary = new();

    [Header("Crop")]
    [SerializeField] private GameObject cropObjectPrefab;
    private readonly Dictionary<Vector3Int, GameObject> cropDictionnary = new();

    [Header("Cursor")]
    [SerializeField] private GameObject cursor;
    [SerializeField] private int cursorReach = 15;
    [SerializeField] private int cursorReachDisplay = 18;

    void OnEnable()
    {
        InputPolling.OnAttackEvent += OnClick;
    }

    void OnDisable()
    {
        InputPolling.OnAttackEvent -= OnClick;
    }

    void FixedUpdate()
    {
        highlightTile();
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked on tilemap");
    }

    private Vector3Int getMousePosOnGrid() 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = groundTilemap.WorldToCell(mousePos);
        cellPosition.z = 0; // Ensure z is set to 0
        return cellPosition;
    }

    private DistanceType GetDistanceType() 
    {
        Vector3Int hoverPosition = getMousePosOnGrid();
        Vector3Int playerPosition = groundTilemap.WorldToCell(player.transform.position);
        playerPosition.z = 0; // Ensure z is set to 0

        float distance = Vector3Int.Distance(playerPosition, hoverPosition);

        if (distance < cursorReach)
            return DistanceType.Interactable;
        else if (distance < cursorReachDisplay)
            return DistanceType.Examinable;
        else
            return DistanceType.Far;
    }

    private void highlightTile()
    {
        Vector3Int hoverPosition = getMousePosOnGrid();
        DistanceType distanceType = GetDistanceType();

        // Check if the cursor is at Interactable distance
        cursor.GetComponent<SpriteRenderer>().color = (distanceType == DistanceType.Interactable) ? Color.white : Color.grey;

        // Check if the cursor is at Examinable distance
        cursor.GetComponent<SpriteRenderer>().enabled = (distanceType == DistanceType.Interactable || distanceType == DistanceType.Examinable);
        
        if(cursor.activeSelf)
            cursor.transform.DOMove(groundTilemap.GetCellCenterWorld(hoverPosition), 0.1f);
        

        // Debug.Log("Hover Position: " + hoverPosition);
        
    }

    private void OnClick()
    {
        Debug.Log("Clicked on tilemap");
        Vector3Int clickedPosition = getMousePosOnGrid();

        if(GetDistanceType() != DistanceType.Interactable)
            return;

        // Check if the clicked tile is farmland
        if (!farmlandDictionnary.ContainsKey(clickedPosition))
        {
            // Check if the clicked tile is farmland
            if (farmlandTilemap.GetTile(clickedPosition) != null)
                return;
            
            farmlandTilemap.SetTile(clickedPosition, farmlandTile);
            farmlandDictionnary.Add(clickedPosition, new FarmlandData(clickedPosition));
            return;
        } else  
        {
            if(cropDictionnary.ContainsKey(clickedPosition))
                return;
            
            GameObject cropObject = Instantiate(cropObjectPrefab, groundTilemap.GetCellCenterWorld(clickedPosition), Quaternion.identity);
            cropDictionnary.Add(clickedPosition, cropObject);
            return;
        }
    }

    private enum DistanceType
    {
        Interactable,
        Examinable,
        Far
    }

    private class FarmlandData
    {
        public Vector3Int Position { get; }

        public FarmlandData(Vector3Int position)
        {
            Position = position;
        }
    }
}