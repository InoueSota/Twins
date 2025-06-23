using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Variables
    private int score;
    private int finalScore;
    private int highScore;

    // Flag
    private bool isStart;
    private bool isFinishScoreAnimation;

    [Header("Score Parameter")]
    [SerializeField] private int bulletValue;
    [SerializeField] private int bodyBlowValue;

    [Header("Combo Parameter")]
    [SerializeField] private float comboAddValue;
    private float comboMag;
    [SerializeField] private float comboTime;
    private float comboTimer;

    [Header("UI")]
    [SerializeField] private Slider comboSlider;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboMagText;

    [Header("UI Animator")]
    [SerializeField] private Animator scoreAnimator;

    [Header("Result UI")]
    [SerializeField] private Text resultScoreText;
    [SerializeField] private Text highScoreText;

    [Header("Result Parameter")]
    [SerializeField] private float resultTime;
    private float resultTimer;

    public void Initialize()
    {
        comboMag = 1f;
        score = 0;
        scoreText.text = string.Format("{0:00000000}", score);
        isFinishScoreAnimation = false;
        isStart = false;
    }

    void Start()
    {
        comboMag = 1f;
        isFinishScoreAnimation = false;
        isStart = false;
    }

    void Update()
    {
        // Score
        scoreText.text = string.Format("{0:00000000}", score);
        comboMagText.text = "x" + comboMag.ToString();

        // Combo
        comboTimer -= Time.deltaTime;
        comboTimer = Mathf.Clamp(comboTimer, 0, comboTime);
        if (comboTimer <= 0f) { comboMag = 1f; }
        comboSlider.value = comboTimer / comboTime;

        if (isStart)
        {
            resultTimer -= Time.deltaTime;
            resultTimer = Mathf.Clamp(resultTimer, 0f, resultTime);
            float t = resultTimer / resultTime;

            int finishScoreNum = (int)Mathf.Lerp(finalScore, 0, t * t);
            resultScoreText.text = string.Format("{0:00000000}", finishScoreNum);

            if (resultTimer <= 0f)
            {
                if (highScore <= finalScore)
                {
                    highScore = finalScore;
                    highScoreText.text = string.Format("{0:00000000}", highScore);
                }

                isFinishScoreAnimation = true;
            }
        }
    }

    // Setter
    public void SetBulletScore()
    {
        // Scoreを加算する
        score += (int)(bulletValue * comboMag);

        // ScoreのAnimatorを起動する
        scoreAnimator.SetTrigger("Start");

        // ComboMagを加算する
        comboMag += comboAddValue;

        // Comboタイマーを初期化する
        comboTimer = comboTime;
    }
    public void SetFinalScore() { finalScore = score; }
    public void SetStart()
    {
        resultTimer = resultTime;
        isFinishScoreAnimation = false;
        isStart = true;
    }

    // Getter
    public bool GetIsFinishScoreAnimation() { return isFinishScoreAnimation; }
}
