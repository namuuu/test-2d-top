using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JetBrains.Annotations;

public class HotbarSystem : MonoBehaviour
{

    [Header("Hotbar")]
    [SerializeField] private GameObject hotbarParent;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject hotbarSlotPrefab;
    [SerializeField] private int hotbarMaxSize = 10;
    private int currentHotbarIndex = 0;
    private int topHotbarIndex = 0;
    private int slideAmount = 50;

    private void OnEnable()
    {
        InputPolling.OnHotbarScrollEvent += OnHotbarScroll;
        InitHotbar();
    }

    private void OnDisable()
    {
        InputPolling.OnHotbarScrollEvent -= OnHotbarScroll;
    }

    private GameObject[] getHotbarSlots()
    {
        // Get the hotbar slots from the hotbar GameObject
        Transform[] hotbarSlots = hotbar.GetComponentsInChildren<Transform>();

        // Create an array to hold the hotbar slots
        GameObject[] slots = new GameObject[hotbarSlots.Length - 1];
        for (int i = 0; i < hotbarSlots.Length - 1; i++)
        {
            // Skip the first element, which is the hotbar itself
            slots[i] = hotbarSlots[i + 1].gameObject;
        }

        return slots;
    }

    private void InitHotbar()
    {
        GameObject[] slots = getHotbarSlots();

        // Move the hotbar to the top
        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();
        float heightDestination = 200 - (slots.Length * 50 + (slots.Length - 1) * 20)/2;
        Debug.Log("Going to " + heightDestination);

        hotbarRect.DOLocalMoveY(heightDestination, 0f);

        

        // Set the current hotbar slot red
        for (int i = 0; i < slots.Length; i++)
        {
            // Debug.Log("Slot " + i + ": " + slots[i].name);

            if (i == currentHotbarIndex)
            {
                slots[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                slots[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void OnHotbarScroll(bool next)
    {
        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();
        Debug.Log("DeltaSize: " + hotbarRect.sizeDelta.y);


        GameObject[] slots = getHotbarSlots();

        // Check if the hotbar spans the entire screen
        if(slots.Length < 6)
            return;

        if(next) {
            // Move to the next slot

            // Don't move if the current index is the last one
            if(currentHotbarIndex >= slots.Length - 1)
                return;

            // Check if we need to slide the hotbar
            if(currentHotbarIndex >= topHotbarIndex + 6)
            {
                // Slide the hotbar to the bottom
                topHotbarIndex++;
                hotbar.transform.DOLocalMoveY(hotbar.transform.localPosition.x - slideAmount, 0.5f);
                
            }

            currentHotbarIndex++;
        } else {
            // Move to the previous slot

            // Don't move if the current index is the first one
            if(currentHotbarIndex <= 0)
                return;

            // Check if we need to slide the hotbar
            if(currentHotbarIndex <= topHotbarIndex)
            {
                // Slide the hotbar to the top
                topHotbarIndex--;
                hotbar.transform.DOLocalMoveY(hotbar.transform.localPosition.x + slideAmount, 0.5f);
                
            }

            currentHotbarIndex--;
        }

        // Set the current hotbar slot red
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentHotbarIndex)
            {
                slots[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                slots[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
