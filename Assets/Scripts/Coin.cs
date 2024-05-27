using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    [SerializeField] 
    private LayerMask finishMask;
    
    [SerializeField] 
    private LayerMask playerMask;
    
    [SerializeField] 
    private float coinVelocity = 1f;

    private Rigidbody _rig;
    private Rigidbody Rig => _rig ??= GetComponent<Rigidbody>();

    private IObjectPool<Coin> _objectPool;

    public IObjectPool<Coin> ObjectPool
    {
        set => _objectPool = value;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (finishMask == (finishMask | (1 <<other.gameObject.layer)) || playerMask == (playerMask | (1 <<other.gameObject.layer)))
        {
            _objectPool.Release(this);    
        }
    }

    public void Initialize()
    {
        var tr = transform;
        
        Rig.velocity = tr.forward * coinVelocity;
    }
}
