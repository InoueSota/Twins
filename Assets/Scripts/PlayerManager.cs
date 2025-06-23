using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Other Objects")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;

    enum Status
    {
        SINGLE,
        DOUBLE
    }
    [SerializeField] private Status status = Status.SINGLE;

    [Header("Single Player")]
    [SerializeField] private GameObject singlePlayerObj;
    private PlayerController singleController;

    [Header("Double Players")]
    [SerializeField] private GameObject leftPlayerObj;
    [SerializeField] private ParticleSystem leftParticleSystem;
    private PlayerController leftController;
    [SerializeField] private GameObject rightPlayerObj;
    [SerializeField] private ParticleSystem rightParticleSystem;
    private PlayerController rightController;

    [Header("Road Materials")]
    [SerializeField] private Material brightMaterial;
    [SerializeField] private Material darkMaterial;
    
    [Header("Road Objects")]
    [SerializeField] private MeshRenderer leftRoadMeshRenderer;
    [SerializeField] private MeshRenderer centerRoadMeshRenderer;
    [SerializeField] private MeshRenderer rightRoadMeshRenderer;

    [Header("DOMove Parameter")]
    [SerializeField] private float doMoveTime;

    [Header("SE")]
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        // Set Component - Other
        inputManager = gameManagerObj.GetComponent<InputManager>();
        singleController = singlePlayerObj.GetComponent<PlayerController>();
        leftController = leftPlayerObj.GetComponent<PlayerController>();
        rightController = rightPlayerObj.GetComponent<PlayerController>();

        // PlayerController - Initialize
        singleController.SetInputManager(inputManager);
        leftController.SetInputManager(inputManager);
        rightController.SetInputManager(inputManager);
    }

    void Update()
    {
        inputManager.GetAllInput();

        // Player更新
        if (singlePlayerObj.activeSelf) { singleController.ManualUpdate(); }
        if (leftPlayerObj.activeSelf)   { leftController.ManualUpdate(); }
        if (rightPlayerObj.activeSelf)  { rightController.ManualUpdate(); }

        switch (status)
        {
            case Status.SINGLE:

                if (inputManager.IsRelease(inputManager.action))
                {
                    // 道のMaterialを変更する
                    leftRoadMeshRenderer.material = brightMaterial;
                    centerRoadMeshRenderer.material = darkMaterial;
                    rightRoadMeshRenderer.material = brightMaterial;

                    // 攻撃フラグをfalseにする
                    singleController.SetAttackActive(false);

                    // 非表示にする
                    singlePlayerObj.SetActive(false);

                    // 表示する
                    leftParticleSystem.Play();
                    rightParticleSystem.Play();

                    // DOTweenで二体を動かす
                    leftPlayerObj.transform.DOKill();
                    rightPlayerObj.transform.DOKill();
                    leftPlayerObj.transform.DOMoveX(-2f, doMoveTime).SetEase(Ease.OutCirc);
                    rightPlayerObj.transform.DOMoveX(2f, doMoveTime).SetEase(Ease.OutCirc).OnComplete(EndDOMoveX);

                    // SEを再生する
                    audioSource.Play();

                    status = Status.DOUBLE;
                }

                break;
            case Status.DOUBLE:

                if (inputManager.IsRelease(inputManager.action))
                {
                    // 道のMaterialを変更する
                    leftRoadMeshRenderer.material = darkMaterial;
                    centerRoadMeshRenderer.material = brightMaterial;
                    rightRoadMeshRenderer.material = darkMaterial;

                    // 攻撃フラグをfalseにする
                    leftController.SetAttackActive(false);
                    rightController.SetAttackActive(false);

                    // DOTweenで二体を動かす
                    leftPlayerObj.transform.DOKill();
                    rightPlayerObj.transform.DOKill();
                    leftPlayerObj.transform.DOMoveX(0f, doMoveTime).SetEase(Ease.OutCirc);
                    rightPlayerObj.transform.DOMoveX(0f, doMoveTime).SetEase(Ease.OutCirc).OnComplete(EndDOMoveX);

                    // SEを再生する
                    audioSource.Play();

                    status = Status.SINGLE;
                }

                break;
        }
    }
    void EndDOMoveX()
    {
        switch (status)
        {
            case Status.SINGLE:

                // 攻撃フラグをtrueにする
                singleController.SetAttackActive(true);

                // 攻撃フラグをfalseにする
                leftController.SetAttackActive(false);
                rightController.SetAttackActive(false);

                // 非表示にする
                leftParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                rightParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                // 表示する
                singlePlayerObj.SetActive(true);

                break;
            case Status.DOUBLE:

                // 攻撃フラグをtrueにする
                leftController.SetAttackActive(true);
                rightController.SetAttackActive(true);

                // 攻撃フラグをfalseにする
                singleController.SetAttackActive(false);

                break;
        }

        // PlayerControllerの初期化を行う
        singleController.SwitchInitialize();
        leftController.SwitchInitialize();
        rightController.SwitchInitialize();
    }

    // Getter
    public InputManager GetInputManager() { return inputManager; }
}
