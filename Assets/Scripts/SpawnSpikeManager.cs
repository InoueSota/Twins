using UnityEngine;

public class SpawnSpikeManager : MonoBehaviour
{
    // Flag
    private bool isActive;

    [Header("Other Sctipt")]
    [SerializeField] private SpeedManager speedManager;

    [Header("Other Transform")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Object")]
    [SerializeField] private GameObject spikePrefab;

    [Header("Spawn Parameter")]
    [SerializeField] private float spawnIntervalMax;
    [SerializeField] private float spawnIntervalMin;
    private float spawnIntervalTimer;

    void Update()
    {
        if (isActive)
        {
            // インターバルの更新
            spawnIntervalTimer -= Time.deltaTime;

            if (spawnIntervalTimer <= 0f)
            {
                // 乱数の取得
                int randomValue = Random.Range(0, 99);

                // 中央に生成
                if (randomValue % 6 >= 3)
                {
                    GameObject spike = Instantiate(spikePrefab, new(0f, 0f, playerTransform.position.z + 22f + speedManager.GetMoveSpeed()), Quaternion.identity);
                    spike.GetComponent<SpikeManager>().Initialize(speedManager, playerTransform);
                }
                // 左端に生成
                else if (randomValue % 4 == 2)
                {
                    GameObject spike = Instantiate(spikePrefab, new(-2f, 0f, playerTransform.position.z + 22f + speedManager.GetMoveSpeed()), Quaternion.identity);
                    spike.GetComponent<SpikeManager>().Initialize(speedManager, playerTransform);
                }
                // 右端に生成
                else if (randomValue % 4 == 1)
                {
                    GameObject spike = Instantiate(spikePrefab, new(2f, 0f, playerTransform.position.z + 22f + speedManager.GetMoveSpeed()), Quaternion.identity);
                    spike.GetComponent<SpikeManager>().Initialize(speedManager, playerTransform);
                }
                // 両端に生成
                else
                {
                    GameObject spike1 = Instantiate(spikePrefab, new(-2f, 0f, playerTransform.position.z + 22f + speedManager.GetMoveSpeed()), Quaternion.identity);
                    GameObject spike2 = Instantiate(spikePrefab, new(2f, 0f, playerTransform.position.z + 22f + speedManager.GetMoveSpeed()), Quaternion.identity);
                    spike1.GetComponent<SpikeManager>().Initialize(speedManager, playerTransform);
                    spike2.GetComponent<SpikeManager>().Initialize(speedManager, playerTransform);
                }

                // インターバルの再設定
                spawnIntervalTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
            }
        }
    }

    // Setter
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;

        if (isActive)
        {
            // Variables - Initialize
            spawnIntervalTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
    }

    // Getter
    public bool GetIsActive() { return isActive; }
}
