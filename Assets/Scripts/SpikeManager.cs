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
        float zDistance = playerTransform.position.z - transform.position.z;

        if (zDistance >= 8f)
        {
            Destroy(gameObject);
        }
    }
    void SubtractSpike()
    {
        int subtractCount = 0;

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
        // ‘Ì—Í‚ðŒ¸‚ç‚·
        hp--;

        // ‘Ì—Í‚ª‚È‚­‚È‚Á‚½‚çíœ‚·‚é
        if (hp <= 0)
        {
            speedManager.AddFastCount();
            Destroy(gameObject);
        }
        else
        {
            // ƒgƒQ‚Ì”‚ðŒ¸‚ç‚·
            SubtractSpike();

            // –Ú•WÀ•W‚ð‚¸‚ç‚·
            initialZ += knockbackRange;

            // Knockback‚·‚é
            transform.DOMoveZ(initialZ, knockbackTime).SetEase(Ease.OutExpo);
        }
    }
}
