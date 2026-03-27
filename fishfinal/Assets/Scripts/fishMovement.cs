using UnityEngine;

public class fishMovement : MonoBehaviour
{

    public float speed;
   
    public void move(Vector3 direction)
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}
