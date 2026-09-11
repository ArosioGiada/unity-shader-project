using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;

    Vector3 velocity;
    float MAX_VELOCITY = 0.1f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z);
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -MAX_VELOCITY, MAX_VELOCITY);

        controller.Move(velocity);
    }
}
