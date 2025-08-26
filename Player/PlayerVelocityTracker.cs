using UnityEngine;

public class PlayerVelocityTracker : MonoBehaviour
{
    //발사체에 붙이기 위한 플레이어 속도 추적기
    public Vector3 Velocity { get; private set; }
    
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
}