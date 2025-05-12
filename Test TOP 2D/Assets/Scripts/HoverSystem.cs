using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class HoverSystem : MonoBehaviour
{

    [SerializeField] private GameObject player;

    [SerializeField] private Tilemap groundTilemap;

    [Header("Farmland")]
    [SerializeField] private GameObject farmlandObjectPrefab;
    private readonly Dictionary<Vector3Int, GameObject> farmlandDictionnary = new();

    [Header("Crop")]
    [SerializeField] private GameObject cropObjectPrefab;
    private readonly Dictionary<Vector3Int, GameObject> cropDictionnary = new();

    [SerializeField] private GameObject cursor;
    

    private Vector3Int lastHoveredTilePos;

    void OnEnable()
    {
        InputPolling.OnAttackEvent += OnClick;
    }

    void OnDisable()
    {
        InputPolling.OnAttackEvent -= OnClick;
    }

    void Update()
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

    private void highlightTile()
    {
        // Get the mouse position in world coordinates
        Vector3Int hoverPosition = getMousePosOnGrid();

        

        cursor.transform.DOMove(groundTilemap.GetCellCenterWorld(hoverPosition), 0.1f);

        // Debug.Log("Hover Position: " + hoverPosition);
        
    }

    private void OnClick()
    {
        Debug.Log("Clicked on tilemap");
        Vector3Int clickedPosition = getMousePosOnGrid();

        // Check if the clicked tile is farmland
        if (!farmlandDictionnary.ContainsKey(clickedPosition))
        {
            GameObject farmlandObject = Instantiate(farmlandObjectPrefab, groundTilemap.GetCellCenterWorld(clickedPosition), Quaternion.identity);
            farmlandDictionnary.Add(clickedPosition, farmlandObject);
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
}