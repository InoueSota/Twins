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

        // Player�X�V
        if (singlePlayerObj.activeSelf) { singleController.ManualUpdate(); }
        if (leftPlayerObj.activeSelf)   { leftController.ManualUpdate(); }
        if (rightPlayerObj.activeSelf)  { rightController.ManualUpdate(); }

        switch (status)
        {
            case Status.SINGLE:

                if (inputManager.IsRelease(inputManager.action))
                {
                    // ����Material��ύX����
                    leftRoadMeshRenderer.material = brightMaterial;
                    centerRoadMeshRenderer.material = darkMaterial;
                    rightRoadMeshRenderer.material = brightMaterial;

                    // �U���t���O��false�ɂ���
                    singleController.SetAttackActive(false);

                    // ��\���ɂ���
                    singlePlayerObj.SetActive(false);

                    // �\������
                    leftParticleSystem.Play();
                    rightParticleSystem.Play();

                    // DOTween�œ�̂𓮂���
                    leftPlayerObj.transform.DOKill();
                    rightPlayerObj.transform.DOKill();
                    leftPlayerObj.transform.DOMoveX(-2f, doMoveTime).SetEase(Ease.OutCirc);
                    rightPlayerObj.transform.DOMoveX(2f, doMoveTime).SetEase(Ease.OutCirc).OnComplete(EndDOMoveX);

                    // SE���Đ�����
                    audioSource.Play();

                    status = Status.DOUBLE;
                }

                break;
            case Status.DOUBLE:

                if (inputManager.IsRelease(inputManager.action))
                {
                    // ����Material��ύX����
                    leftRoadMeshRenderer.material = darkMaterial;
                    centerRoadMeshRenderer.material = brightMaterial;
                    rightRoadMeshRenderer.material = darkMaterial;

                    // �U���t���O��false�ɂ���
                    leftController.SetAttackActive(false);
                    rightController.SetAttackActive(false);

                    // DOTween�œ�̂𓮂���
                    leftPlayerObj.transform.DOKill();
                    rightPlayerObj.transform.DOKill();
                    leftPlayerObj.transform.DOMoveX(0f, doMoveTime).SetEase(Ease.OutCirc);
                    rightPlayerObj.transform.DOMoveX(0f, doMoveTime).SetEase(Ease.OutCirc).OnComplete(EndDOMoveX);

                    // SE���Đ�����
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

                // �U���t���O��true�ɂ���
                singleController.SetAttackActive(true);

                // �U���t���O��false�ɂ���
                leftController.SetAttackActive(false);
                rightController.SetAttackActive(false);

                // ��\���ɂ���
                leftParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                rightParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                // �\������
                singlePlayerObj.SetActive(true);

                break;
            case Status.DOUBLE:

                // �U���t���O��true�ɂ���
                leftController.SetAttackActive(true);
                rightController.SetAttackActive(true);

                // �U���t���O��false�ɂ���
                singleController.SetAttackActive(false);

                break;
        }

        // PlayerController�̏��������s��
        singleController.SwitchInitialize();
        leftController.SwitchInitialize();
        rightController.SwitchInitialize();
    }

    // Getter
    public InputManager GetInputManager() { return inputManager; }
}
