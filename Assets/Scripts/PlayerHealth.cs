using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [MinValue(0)]
    [SerializeField] private int health = 3;

    private void OnEnable()
    {
        EventManager.onCollision += TakeDamage;
    }

    private void OnDisable()
    {
        EventManager.onCollision -= TakeDamage;
    }

    private void TakeDamage()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            EventManager.OnGameOver();
        }
    }
}