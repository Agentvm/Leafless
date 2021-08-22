using UnityEngine;

public class PushRigidbodies : MonoBehaviour
{

    // this script pushes all rigidbodies that the character touches
    [SerializeField] float pushPower = 2.0f;


    void PushRigidbody (Rigidbody body, Vector3 moveDirection = default)
    {
        // Default
        if (moveDirection == default)
            moveDirection = Vector3.up;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3 (moveDirection.x, 2f, moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        PushRigidbody (hit.collider.attachedRigidbody, hit.moveDirection);
    }

    private void OnTriggerEnter (Collider other)
    {
        PushRigidbody (other.attachedRigidbody, this.transform.forward);
    }
}
