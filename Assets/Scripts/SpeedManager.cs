using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float fastSpeed;
    [SerializeField] private float moveSpeed;

    [Header("Fast Parameter")]
    [SerializeField] private int fastCountMax;
    private int fastCount;
    [SerializeField] private float fastTime;
    private float fastTimer;
    private bool isFast;

    [Header("Moving Object")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform fieldTransform;
    [SerializeField] private Transform singlePlayerTransform;
    [SerializeField] private Transform leftPlayerTransform;
    [SerializeField] private Transform rightPlayerTransform;
    [SerializeField] private Transform bulletTransform;

    [Header("Moving Particle System")]
    [SerializeField] private ParticleSystem buildingParticle;
    [SerializeField] private ParticleSystem noiseParticle;

    void Update()
    {
        Fast();

        float deltaMoveSpeed = moveSpeed * Time.deltaTime;

        cameraTransform.position = new(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z + deltaMoveSpeed);
        fieldTransform.position = new(fieldTransform.position.x, fieldTransform.position.y, fieldTransform.position.z + deltaMoveSpeed);
        singlePlayerTransform.position = new(singlePlayerTransform.position.x, singlePlayerTransform.position.y, singlePlayerTransform.position.z + deltaMoveSpeed);
        leftPlayerTransform.position = new(leftPlayerTransform.position.x, leftPlayerTransform.position.y, leftPlayerTransform.position.z + deltaMoveSpeed);
        rightPlayerTransform.position = new(rightPlayerTransform.position.x, rightPlayerTransform.position.y, rightPlayerTransform.position.z + deltaMoveSpeed);
        bulletTransform.position = new(bulletTransform.position.x, bulletTransform.position.y, bulletTransform.position.z + deltaMoveSpeed);

        // Particle System の生成範囲をカメラのスクロールに合わせる
        var shape = buildingParticle.shape;
        shape.position = new(shape.position.x, shape.position.y - deltaMoveSpeed, shape.position.z);

        var shape2 = noiseParticle.shape;
        shape2.position = new(shape2.position.x, shape2.position.y - deltaMoveSpeed, shape2.position.z);
    }
    void Fast()
    {
        if (isFast)
        {
            fastTimer -= Time.deltaTime;
            moveSpeed = fastSpeed;

            if (fastTimer <= 0) { isFast = false; }
        }
        else { moveSpeed = defaultSpeed; }
    }

    // Setter
    public void AddFastCount()
    {
        if (!isFast)
        {
            fastCount++;

            if (fastCount >= fastCountMax)
            {
                fastCount = 0;
                fastTimer = fastTime;
                isFast = true;
            }
        }
    }

    // Getter
    public float GetMoveSpeed() { return moveSpeed; }
}
