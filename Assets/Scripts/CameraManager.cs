using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void CameraShake()
    {
        transform.DOShakePosition(0.4f, 0.8f, 20, 1, false, true);
    }
}
