using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin();

            Destroy(gameObject);
        }
    }
    private void Update()
    {
        // 코인을 Y축을 기준으로 회전
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
