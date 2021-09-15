using Sirenix.OdinInspector;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _startTime;
    private float _endTime;

    [MinValue(0.01)] [SerializeField] private float minimumDistance = 0.2f;
    [SerializeField] private float maximumTime = 1f;

    [Range(0, 1)]
    [Tooltip("The accuracy required to detect whether a swipe was performed in a particular direction")]
    [SerializeField]
    private float directionThreshold = 0.9f;

    private void Start()
    {
        EventManager.onStartTouch += SwipeStart;
        EventManager.onEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        EventManager.onStartTouch -= SwipeStart;
        EventManager.onEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(_startPosition, _endPosition) >= minimumDistance && _endTime - _startTime <= maximumTime)
        {
            Vector3 direction = _endPosition - _startPosition;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            DetermineSwipeDirection(direction2D);
        }
    }

    private void DetermineSwipeDirection(Vector2 direction2D)
    {
        if (Vector2.Dot(Vector2.up, direction2D) > directionThreshold)
            EventManager.OnSwipeUp();
        else if (Vector2.Dot(Vector2.down, direction2D) > directionThreshold)
            EventManager.OnSwipeDown();
        else if (Vector2.Dot(Vector2.right, direction2D) > directionThreshold)
            EventManager.OnSwipeRight();
        else if (Vector2.Dot(Vector2.left, direction2D) > directionThreshold) EventManager.OnSwipeLeft();
    }
}