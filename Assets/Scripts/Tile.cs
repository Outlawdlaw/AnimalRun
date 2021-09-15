using UnityEngine;

public class Tile : MonoBehaviour
{
    public float movementSpeed { get; set; }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * (Time.deltaTime * movementSpeed));
    }
}
