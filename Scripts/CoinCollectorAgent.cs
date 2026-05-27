using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CoinCollectorAgent : Agent
{
    public float moveSpeed = 5f;
    public Transform[] coins;

    private Rigidbody rb;
    private Vector3 startPosition;
    private float previousDistanceToCoin;
    private int collectedCoins;
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        MaxStep = 720;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        collectedCoins = 0;

        foreach (Transform coin in coins)
        {
            coin.gameObject.SetActive(true);
        }

        Transform closestCoin = GetClosestCoin();
        if (closestCoin != null)
            previousDistanceToCoin = Vector3.Distance(transform.position, closestCoin.position);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);

        Transform closestCoin = GetClosestCoin();

        if (closestCoin != null)
        {
            Vector3 relativeCoinPosition = closestCoin.position - transform.position;
            sensor.AddObservation(relativeCoinPosition);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
        }

        sensor.AddObservation(rb.linearVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveAction = actions.DiscreteActions[0];

        Vector3 moveDirection = Vector3.zero;

        switch (moveAction)
        {
            case 0: moveDirection = Vector3.forward; break;
            case 1: moveDirection = Vector3.back; break;
            case 2: moveDirection = Vector3.left; break;
            case 3: moveDirection = Vector3.right; break;
        }

        rb.linearVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.linearVelocity.y,
            moveDirection.z * moveSpeed
        );

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateSteps(StepCount);
        }

        AddReward(-0.0005f);

        Transform closestCoin = GetClosestCoin();
        if (closestCoin != null)
        {
            float currentDistance = Vector3.Distance(transform.position, closestCoin.position);

            if (currentDistance < previousDistanceToCoin)
            {
                AddReward(0.02f);
            }

            previousDistanceToCoin = currentDistance;
        }

        if (AllCoinsCollected())
        {
            AddReward(8.0f);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(200);

                int efficiencyBonus = Mathf.Max(0, (MaxStep - StepCount) / 5);
                GameManager.Instance.AddScore(efficiencyBonus);

                GameManager.Instance.EndEpisode(StepCount);
            }

            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;

        if (Input.GetKey(KeyCode.W)) discreteActions[0] = 0;
        if (Input.GetKey(KeyCode.S)) discreteActions[0] = 1;
        if (Input.GetKey(KeyCode.A)) discreteActions[0] = 2;
        if (Input.GetKey(KeyCode.D)) discreteActions[0] = 3;
    }

    private Transform GetClosestCoin()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform coin in coins)
        {
            if (!coin.gameObject.activeSelf) continue;

            float distance = Vector3.Distance(transform.position, coin.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = coin;
            }
        }

        return closest;
    }

    public void UpdateClosestCoinDistance()
    {
        Transform closestCoin = GetClosestCoin();
        if (closestCoin != null)
        {
            previousDistanceToCoin = Vector3.Distance(transform.position, closestCoin.position);
        }
    }

    public void OnCoinCollected()
    {
        collectedCoins++;

        if (collectedCoins == 2)
        {
            AddReward(2.0f);
        }
        if (collectedCoins == 3)
        {
            AddReward(4.0f);
        }
        else if (collectedCoins == 4)
        {
            AddReward(8.0f);
        }
    }

    private bool AllCoinsCollected()
    {
        foreach (Transform coin in coins)
        {
            if (coin.gameObject.activeSelf)
                return false;
        }
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Wall") || collision.gameObject.name.Contains("Obstacle"))
        {
            AddReward(-0.1f);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCollisionPenalty(10);
            }
        }
    }
}