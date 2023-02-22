using UnityEngine;

public class ScoringTrigger : MonoBehaviour
{
    private bool _isTriggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isTriggered)
        {
            GameManager.instance.IncreaseScore();
            Spawner.instance.DeletePipe();
            _isTriggered = true;
        }
    }
}