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
            audioSource.Play();

            // �_���[�W���󂯂�
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            gameManager.SubtractHp();

            // Spike������
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
