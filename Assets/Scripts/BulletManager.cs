using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [Header("Life Parameter")]
    [SerializeField] private float lifeTime;
    private float lifeTimer;

    [Header("Move Parameter")]
    [SerializeField] private float moveSpeed;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitPrefab;

    public void Initialize()
    {
        // Variables - Initialize
        lifeTimer = lifeTime;
    }

    void Update()
    {
        // �����\���Ԃ̍X�V
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) { Destroy(gameObject); }

        // �ړ�
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        transform.position = new(transform.position.x, transform.position.y, transform.position.z + deltaMoveSpeed);
    }

    private void OnTriggerEnter(Collider other) { OnCollision(other); }
    void OnCollision(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            // HitEffect�쐬
            Instantiate(hitPrefab, transform.position, Quaternion.identity);

            // Score
            ScoreManager scoreManager = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
            scoreManager.SetBulletScore();

            // Spike��Knockback������
            other.GetComponent<SpikeManager>().SetKnockback();

            // �폜
            Destroy(gameObject);
        }
    }
}
