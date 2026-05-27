using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        CoinCollectorAgent agent = other.GetComponent<CoinCollectorAgent>();

        if (agent != null && gameObject.activeSelf)
        {
            agent.AddReward(3.0f);
            agent.OnCoinCollected();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(100);
                GameManager.Instance.AddCoin();
            }

            gameObject.SetActive(false);
            agent.UpdateClosestCoinDistance();
        }
    }
}