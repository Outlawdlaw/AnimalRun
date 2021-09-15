using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [ChildGameObjectsOnly] [SerializeField]
    private Animator animatorController;

    [MinValue(0)] [SerializeField] private float moveSpeed;
    [MinValue(0)] [SerializeField] private float laneWidth;
    [MinValue(0)] [SerializeField] private float jumpVelocity;
    [MinValue(1)] [SerializeField] private float fallMultiplier = 2.5f;
    [MinValue(0)] [SerializeField] private float groundCheckOffset = 0.1f;

    [Range(-1, 1)] private int _currentLane;
    private Rigidbody _rigidbody;
    private float _groundCheckDistance;
    private bool _shouldRoll = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheckDistance = GetComponent<Collider>().bounds.extents.y + groundCheckOffset;

        EventManager.onSwipeUp += Jump;
        EventManager.onSwipeDown += Roll;
        EventManager.onSwipeRight += MoveRight;
        EventManager.onSwipeLeft += MoveLeft;
    }

    private void MoveLeft()
    {
        if (_currentLane > -1)
        {   
            _currentLane--;
            
            var endPosition = _currentLane * laneWidth;
            string coroutineName = "ChangeLane";
            StopCoroutine(coroutineName);
            
            animatorController.SetBool("MoveLeft", true);

            StartCoroutine(ChangeLane(endPosition));
        }
    }

    private void MoveRight()
    {
        if (_currentLane < 1)
        {
            _currentLane++;

            var endPosition = _currentLane * laneWidth;
            string coroutineName = "ChangeLane";
            StopCoroutine(coroutineName);
            
            animatorController.SetBool("MoveRight", true);
            
            StartCoroutine(ChangeLane(endPosition));
        }
    }

    private IEnumerator ChangeLane(float endPosition)
    {
        while (Math.Abs(transform.position.x - endPosition) > 0.1)
        {
            var position = transform.position;
            var newPosition = Mathf.Lerp(position.x, endPosition, moveSpeed * Time.deltaTime);

            position = new Vector3(newPosition, position.y, 0);
            transform.position = position;
            
            yield return new WaitForEndOfFrame();
        }

        var currentPosition = transform;
        currentPosition.position = new Vector3(endPosition, currentPosition.position.y, 0);
        animatorController.SetBool("MoveLeft", false);
        animatorController.SetBool("MoveRight", false);
    }

    private void Roll()
    {
        if (!IsGrounded())
        {
            _rigidbody.velocity = new Vector3(0, -jumpVelocity, 0);
            _shouldRoll = true;
        }
        else
        {
            animatorController.SetTrigger("Roll");
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _rigidbody.velocity = new Vector3(0, jumpVelocity, 0);

            StartCoroutine(IsInAir());
        } 
    }

    private IEnumerator IsInAir()
    {
        animatorController.SetTrigger("Jump");

        yield return new WaitForSeconds(.2f);
        
        while (!IsGrounded())
        {
            yield return new WaitForEndOfFrame();
        }

        if (!_shouldRoll)
        {
            animatorController.SetTrigger("Land");
        }
        else
        {
            animatorController.SetTrigger("Roll");
            _shouldRoll = false;
        }
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.y < 0)
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        var layerMask = 1 << 3;

        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundCheckDistance, layerMask);
    }
}