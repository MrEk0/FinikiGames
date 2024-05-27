using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    
    [SerializeField] 
    private Transform road;
    
    [SerializeField] 
    private LayerMask coinMask;

    private bool _isDead;

    public event Action CoinCollectEvent = delegate { };
    public event Action DeadEvent = delegate { };

    private void OnCollisionEnter(Collision other)
    {
        if (coinMask == (coinMask | (1 <<other.gameObject.layer)))
        {
            CoinCollectEvent();
        }
    }

    private void Update()
    {
        if (_isDead)
            return;

        if (transform.position.y > road.transform.position.y)
            return;
        
        joystick.Deactivate();

        _isDead = true;

        DeadEvent();
    }

    public void Restart()
    {
        _isDead = false;
        
        joystick.Activate();
    }
}
