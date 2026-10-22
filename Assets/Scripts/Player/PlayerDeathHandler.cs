using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour, IOnDeath
{
    private GameManager gameManager;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    public void OnDeath()
    {
        if(gameManager != null)
        {
            gameManager.EndRun();
        }
        else
        {
            Debug.LogError("GameManager missing");
        }
    }
}
