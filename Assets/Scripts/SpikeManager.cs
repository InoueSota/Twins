using DG.Tweening;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    // Coordinate
    private float initialZ;

    // Other Script
    private SpeedManager speedManager;

    // Other Transform
    private Transform playerTransform;

    // Parameter
    private int hp;

    [Header("Knockback Parameter")]
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackRange;

    [Header("Spikes")]
    [SerializeField] private GameObject[] spikeObjs;

    public void Initialize(SpeedManager _speedManager, Transform _playerTransform)
    {
        // Get
        speedManager = _speedManager;
        playerTransform = _playerTransform;

        // Coordinate - Initialize
        initialZ = transform.position.z;

        // Parameter - Initialize
        hp = 3;
    }

    void Update()
    {
        Reach();
    }
    void Reach()
    {
        // プレイヤーとどれくらい離れているか取得
        float zDistance = playerTransform.position.z - transform.position.z;

        // プレイヤーよりも一定距離下がっていたら削除する
        if (zDistance >= 8f) { Destroy(gameObject); }
    }
    void SubtractSpike()
    {
        int subtractCount = 0;

        // トゲを減らす処理
        while (subtractCount < 9f)
        {
            int randomSpikeNumber = Random.Range(0, 19);

            if (spikeObjs[randomSpikeNumber].activeSelf) { spikeObjs[randomSpikeNumber].SetActive(false); }
            else { continue; }

            subtractCount++;
        }
    }

    // Setter
    public void SetKnockback()
    {
        // 体力を減らす
        hp--;

        // 体力がなくなったら削除する
        if (hp <= 0)
        {
            speedManager.AddFastCount();
            Destroy(gameObject);
        }
        else
        {
            // トゲの数を減らす
            SubtractSpike();

            // 目標座標をずらす
            initialZ += knockbackRange;

            // Knockbackする
            transform.DOMoveZ(initialZ, knockbackTime).SetEase(Ease.OutExpo);
        }
    }
}
