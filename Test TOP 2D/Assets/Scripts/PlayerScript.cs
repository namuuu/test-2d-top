using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerStats stats;

    private GameObject _spriteObject;
    private Rigidbody2D _rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteObject = transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       HandleMovement();
       HandleFacing();

       _rb.linearVelocity = nextVelocity;
    }

    private Vector2 desiredVelocity;
    private Vector2 currentVelocity;
    private Vector2 nextVelocity;

    private void HandleMovement()
    {
        // Handle player movement here
        // For example, you can use InputPolling.inputStruct.move to get the input values
        // and apply them to the player's Rigidbody2D component.

        currentVelocity = _rb.linearVelocity;
        desiredVelocity = new Vector2(InputPolling.inputStruct.fixedMove.x * stats.maxSpeed, InputPolling.inputStruct.fixedMove.y * stats.maxSpeed);

        // Calculate the next velocity based on the desired velocity and current velocity
        nextVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, (desiredVelocity.x != 0 ? stats.acceleration : stats.deceleration) * Time.deltaTime);
        nextVelocity.y = Mathf.MoveTowards(currentVelocity.y, desiredVelocity.y, (desiredVelocity.x != 0 ? stats.acceleration : stats.deceleration) * Time.deltaTime);

        
    }

    private void HandleFacing()
    {
        if(nextVelocity.x > 0) {
            _spriteObject.transform.localScale = new Vector3(1, 1, 1);
        } else if(nextVelocity.x < 0) {
            _spriteObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
