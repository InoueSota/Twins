using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input
    private InputManager inputManager;

    [Header("Parent Transform")]
    [SerializeField] private Transform bulletParent;

    [Header("Attack Active")]
    [SerializeField] private bool isActive;

    [Header("Attack Object")]
    [SerializeField] private GameObject bulletObj;

    [Header("Attack Parameter")]
    [SerializeField] private float attackIntervalTime;
    private float attackIntervalTimer;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;

    public void ManualUpdate()
    {
        if (isActive)
        {
            Attack();
        }
    }
    void Attack()
    {
        if (inputManager.IsPush(inputManager.action))
        {
            // インターバルの更新
            attackIntervalTimer -= Time.deltaTime;

            if (attackIntervalTimer <= 0f)
            {
                // 弾を生成する
                GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
                bullet.GetComponent<BulletManager>().Initialize();
                bullet.transform.parent = bulletParent;

                // インターバルの再設定
                attackIntervalTimer = attackIntervalTime;
            }
        }
        else
        {
            // インターバルの再設定
            attackIntervalTimer = attackIntervalTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            audioSource.Play();

            // ダメージを受ける
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            gameManager.SubtractHp();

            // Spikeを消去
            Destroy(other.gameObject);
        }
    }

    // Setter
    public void SwitchInitialize()
    {
        attackIntervalTimer = attackIntervalTime;
    }
    public void SetInputManager(InputManager _inputManager) { inputManager = _inputManager; }
    public void SetAttackActive(bool _isActive) { isActive = _isActive; }
}
