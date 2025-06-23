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
        // 入力状況を取得する
        inputManager.GetAllInput();

        // ゲーム開始
        if (!canStart)
        {
            if (inputManager.IsPush(inputManager.action))
            {
                startCoolTimer += Time.deltaTime;

                if (startCoolTimer >= startCoolTime)
                {
                    // UIの表示／非表示を切り替える
                    groupTitle.SetActive(false);
                    groupIngame.SetActive(true);
                    groupOnlyIngame.SetActive(true);

                    spikeCoolTimer = spikeCoolTime;

                    // GameStartFadeを再生
                    gameStartAnimator.SetTrigger("Start");

                    // 開始SEを再生する
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

            // トゲ生成を開始する
            if (hp > 0 && !spawnSpikeManager.GetIsActive() && spikeCoolTimer <= 0f) { spawnSpikeManager.SetIsActive(true); }
        }

        // ゲーム終了フラグ
        Finish();

        // Damageのクールタイムを更新する
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
                    // CoolTimerを初期化
                    startCoolTimer = 0f;

                    // Scoreを戻す
                    scoreManager.Initialize();

                    // HPを戻す
                    hp = 3;
                    hp1Obj.SetActive(true);
                    hp2Obj.SetActive(true);
                    hp3Obj.SetActive(true);

                    // UIの表示／非表示を切り替える
                    groupTitle.SetActive(true);
                    groupIngame.SetActive(false);
                    groupResult.SetActive(false);

                    // Particleを再生する
                    buildingParticle.Play();

                    // Flag
                    canStart = false;
                }
            }
        }
    }
    void LateUpdate()
    {
        // 入力状況をリセットする
        inputManager.SetIsGetInput();
    }

    public void SubtractHp()
    {
        if (damageCoolTimer <= 0f)
        {
            // 体力を減らす
            hp--;

            // 体力がなくなったら
            if (hp == 0)
            {
                // Resultのクールタイムを設定
                resultCoolTimer = resultCoolTime;

                // UIの表示／非表示を切り替える
                groupOnlyIngame.SetActive(false);

                // Particleを止める
                buildingParticle.Stop();

                // Spike生成フラグをfalseにする
                spawnSpikeManager.SetIsActive(false);

                // Score
                scoreManager.SetFinalScore();

                // GameOverFadeを再生
                gameOverAnimator.SetTrigger("Start");
            }
            else if (hp > 0)
            {
                // Vignetteを再生
                vignetteAnimator.SetTrigger("Start");

                AudioSource.PlayClipAtPoint(subtractHpClip, cameraManager.transform.position);
            }

            // UIを適応させる
            if (hp >= 2) { hp3Obj.SetActive(false); }
            else if (hp >= 1) { hp2Obj.SetActive(false); }
            else { hp1Obj.SetActive(false); }

            // CameraShakeを行う
            cameraManager.CameraShake();

            // Damageのクールタイムを再設定する
            damageCoolTimer = damageCoolTime;
        }
    }
}
