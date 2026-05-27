using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float rotateSpeed = 120f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
