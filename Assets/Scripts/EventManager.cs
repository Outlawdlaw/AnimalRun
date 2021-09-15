using UnityEngine;

public static class EventManager
{
    public delegate void TouchEvent(Vector2 position, float time);
    public delegate void VoidEvent();
    

    public static event TouchEvent onStartTouch;
    public static event TouchEvent onEndTouch;
    
    public static event VoidEvent onSwipeUp;
    public static event VoidEvent onSwipeDown;
    public static event VoidEvent onSwipeRight;
    public static event VoidEvent onSwipeLeft;

    public static event VoidEvent onCollision;
    public static event VoidEvent onGameOver; 

    public static void OnStartTouch(Vector2 position, float time)
    {
        onStartTouch?.Invoke(position, time);
    }

    public static void OnEndTouch(Vector2 position, float time)
    {
        onEndTouch?.Invoke(position, time);
    }

    public static void OnSwipeUp()
    {
        onSwipeUp?.Invoke();
    }

    public static void OnSwipeDown()
    {
        onSwipeDown?.Invoke();
    }

    public static void OnSwipeRight()
    {
        onSwipeRight?.Invoke();
    }

    public static void OnSwipeLeft()
    {
        onSwipeLeft?.Invoke();
    }

    public static void OnCollision()
    {
        onCollision?.Invoke();
    }

    public static void OnGameOver()
    {
        onGameOver?.Invoke();
    }
}