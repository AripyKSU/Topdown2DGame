using UnityEngine;

public class MoneyItem : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Visual")]
    [SerializeField] private GameObject pickupEffect;

    private void Update()
    {
        // ������ ȸ�� �ִϸ��̼�
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameScorer.I.AddMoney();
            if (GameAudio.I != null)
            {
                GameAudio.I.PlayMoneyPickup();
            }

            // ��ƼŬ ȿ�� (���� ����)
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // ������ ����
            Destroy(gameObject);
        }
    }
}
