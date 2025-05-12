using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Speed")]
    public float maxSpeed = 8.5f; // Speed at which the player moves
    public float acceleration = 50f; // Acceleration rate
    public float deceleration = 65f; // Deceleration rate
}
