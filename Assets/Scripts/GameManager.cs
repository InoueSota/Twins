using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    [Header("Other Component")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Start Cool Time")]
    [SerializeField] private Slider startCoolTimeSlider;
    [SerializeField] private float startCoolTime;
    private float startCoolTimer;

    [Header("Spike")]
    [SerializeField] private SpawnSpikeManager spawnSpikeManager;
    [SerializeField] private float spikeCoolTime;
    private float spikeCoolTimer;

    [Header("Group UI")]
    [SerializeField] private GameObject groupTitle;
    [SerializeField] private GameObject groupIngame;
    [SerializeField] private GameObject groupOnlyIngame;
    [SerializeField] private GameObject groupResult;

    [Header("Life Point")]
    [SerializeField] private Animator vignetteAnimator;
    [SerializeField] private GameObject hp1Obj;
    [SerializeField] private GameObject hp2Obj;
    [SerializeField] private GameObject hp3Obj;
    [SerializeField] private int hp;

    [Header("Fade UI")]
    [SerializeField] private Animator gameStartAnimator;
    [SerializeField] private Animator gameOverAnimator;

    [Header("Damage Cool Time")]
    [SerializeField] private float damageCoolTime;
    private float damageCoolTimer;

    [Header("Effect")]
    [SerializeField] private ParticleSystem buildingParticle;

    [Header("Result Cool Time")]
    [SerializeField] private float resultCoolTime;
    private float resultCoolTimer;

    // Flag
    private bool canStart;

    [Header("SE")]
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip subtractHpClip;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �Q�[���J�n
        if (!canStart)
        {
            if (inputManager.IsPush(inputManager.action))
            {
                startCoolTimer += Time.deltaTime;

                if (startCoolTimer >= startCoolTime)
                {
                    // UI�̕\���^��\����؂�ւ���
                    groupTitle.SetActive(false);
                    groupIngame.SetActive(true);
                    groupOnlyIngame.SetActive(true);

                    spikeCoolTimer = spikeCoolTime;

                    // GameStartFade���Đ�
                    gameStartAnimator.SetTrigger("Start");

                    // �J�nSE���Đ�����
                    AudioSource.PlayClipAtPoint(startClip, cameraManager.transform.position);

                    // Flag
                    canStart = true;
                }
            }
            else
            {
                startCoolTimer = 0f;
            }

            startCoolTimeSlider.value = startCoolTimer / startCoolTime;
        }
        else
        {
            spikeCoolTimer -= Time.deltaTime;

            // �g�Q�������J�n����
            if (hp > 0 && !spawnSpikeManager.GetIsActive() && spikeCoolTimer <= 0f) { spawnSpikeManager.SetIsActive(true); }
        }

        // �Q�[���I���t���O
        Finish();

        // Damage�̃N�[���^�C�����X�V����
        damageCoolTimer -= Time.deltaTime;
    }
    void Finish()
    {
        if (hp <= 0)
        {
            resultCoolTimer -= Time.deltaTime;

            if (resultCoolTimer <= 0f)
            {
                if (!groupResult.activeSelf)
                {
                    groupResult.SetActive(true);
                    scoreManager.SetStart();
                }

                if (scoreManager.GetIsFinishScoreAnimation() && inputManager.IsTrgger(inputManager.action))
                {
                    // CoolTimer��������
                    startCoolTimer = 0f;

                    // Score��߂�
                    scoreManager.Initialize();

                    // HP��߂�
                    hp = 3;
                    hp1Obj.SetActive(true);
                    hp2Obj.SetActive(true);
                    hp3Obj.SetActive(true);

                    // UI�̕\���^��\����؂�ւ���
                    groupTitle.SetActive(true);
                    groupIngame.SetActive(false);
                    groupResult.SetActive(false);

                    // Particle���Đ�����
                    buildingParticle.Play();

                    // Flag
                    canStart = false;
                }
            }
        }
    }
    void LateUpdate()
    {
        // ���͏󋵂����Z�b�g����
        inputManager.SetIsGetInput();
    }

    public void SubtractHp()
    {
        if (damageCoolTimer <= 0f)
        {
            // �̗͂����炷
            hp--;

            // �̗͂��Ȃ��Ȃ�����
            if (hp == 0)
            {
                // Result�̃N�[���^�C����ݒ�
                resultCoolTimer = resultCoolTime;

                // UI�̕\���^��\����؂�ւ���
                groupOnlyIngame.SetActive(false);

                // Particle���~�߂�
                buildingParticle.Stop();

                // Spike�����t���O��false�ɂ���
                spawnSpikeManager.SetIsActive(false);

                // Score
                scoreManager.SetFinalScore();

                // GameOverFade���Đ�
                gameOverAnimator.SetTrigger("Start");
            }
            else if (hp > 0)
            {
                // Vignette���Đ�
                vignetteAnimator.SetTrigger("Start");

                AudioSource.PlayClipAtPoint(subtractHpClip, cameraManager.transform.position);
            }

            // UI��K��������
            if (hp >= 2) { hp3Obj.SetActive(false); }
            else if (hp >= 1) { hp2Obj.SetActive(false); }
            else { hp1Obj.SetActive(false); }

            // CameraShake���s��
            cameraManager.CameraShake();

            // Damage�̃N�[���^�C�����Đݒ肷��
            damageCoolTimer = damageCoolTime;
        }
    }
}
