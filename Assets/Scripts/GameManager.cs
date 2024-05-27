using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField] 
    private CoinCounter coinCounter;

    [SerializeField] 
    private CoinSpawner coinSpawner;

    [SerializeField]
    private Transform startPos;

    [SerializeField] 
    private int coinReward;

    private int _coinCount;

    private void OnEnable()
    {
        playerController.CoinCollectEvent += OnCoinCollected;
        playerController.DeadEvent += OnDead;
    }

    private void OnDisable()
    {
        playerController.CoinCollectEvent -= OnCoinCollected;
        playerController.DeadEvent -= OnDead;
    }

    private void OnCoinCollected()
    {
        _coinCount += coinReward;

        coinCounter.UpdateText(_coinCount);
    }

    private void OnDead()
    {
        _coinCount = 0;
        coinCounter.UpdateText(_coinCount);

        playerController.transform.position = startPos.position;
        playerController.Restart();
        
        coinSpawner.Restart();
    }
}
