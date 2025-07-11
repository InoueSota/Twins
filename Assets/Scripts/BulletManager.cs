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
        // 生存可能時間の更新
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) { Destroy(gameObject); }

        // 移動
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        transform.position = new(transform.position.x, transform.position.y, transform.position.z + deltaMoveSpeed);
    }

    private void OnTriggerEnter(Collider other) { OnCollision(other); }
    void OnCollision(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            // HitEffect作成
            Instantiate(hitPrefab, transform.position, Quaternion.identity);

            // Score
            ScoreManager scoreManager = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
            scoreManager.SetBulletScore();

            // SpikeをKnockbackさせる
            other.GetComponent<SpikeManager>().SetKnockback();

            // 削除
            Destroy(gameObject);
        }
    }
}
