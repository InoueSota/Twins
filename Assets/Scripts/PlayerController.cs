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
            // �C���^�[�o���̍X�V
            attackIntervalTimer -= Time.deltaTime;

            if (attackIntervalTimer <= 0f)
            {
                // �e�𐶐�����
                GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
                bullet.GetComponent<BulletManager>().Initialize();
                bullet.transform.parent = bulletParent;

                // �C���^�[�o���̍Đݒ�
                attackIntervalTimer = attackIntervalTime;
            }
        }
        else
        {
            // �C���^�[�o���̍Đݒ�
            attackIntervalTimer = attackIntervalTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            float xDistance = other.transform.position.x - transform.position.x;
            float zDistance = other.transform.position.z - transform.position.z;

            if (Mathf.Abs(xDistance) >= Mathf.Abs(zDistance))
            {
                audioSource.Play();

                // Score
                ScoreManager scoreManager = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
                scoreManager.SetBodyBlowScore();

                // Spike������
                Destroy(other.gameObject);
            }
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
