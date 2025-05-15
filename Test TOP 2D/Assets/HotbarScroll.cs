using UnityEngine;
using DG.Tweening;

public class HotbarScroll : MonoBehaviour
{

    [SerializeField] private GameObject hotbarContainer;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int padding = 10;

    public void getSlots()
    {
        // Get all the slots in the hotbar container
        Transform[] slots = hotbarContainer.GetComponentsInChildren<Transform>();

        Debug.Log("Slots found: " + slots.Length);

        // Loop through each slot and do something with it
        // foreach (Transform slot in slots)
        // {
        //     // Check if the slot is a child of the hotbar container
        //     if (slot != hotbarContainer.transform)
        //     {
        //         // Do something with the slot
        //         Debug.Log("Slot: " + slot.name);
        //     }
        // }
    }

    public void Next()
    {
        // Check if the current index is less than the number of slots
        if (currentIndex < hotbarContainer.transform.childCount - 1)
        {
            currentIndex++;
            MoveHotbar();
        }
    }

    public void Previous()
    {
        // Check if the current index is greater than 0
        if (currentIndex > 0)
        {
            currentIndex--;
            MoveHotbar();
        }
    }

    private void MoveHotbar()
    {
        Vector3 previousPosition = hotbarContainer.transform.position;
        Vector3 targetPosition = new Vector3(previousPosition.x - (currentIndex * (hotbarContainer.transform.localScale.x + padding)), previousPosition.y, previousPosition.z);

        // Move the hotbar container to the target position
        hotbarContainer.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutBack);
    }
}
