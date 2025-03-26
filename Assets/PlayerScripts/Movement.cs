using System;
using System.Collections;
using UnityEngine;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("LAYERS")]
        public LayerMask PlayerLayer;

        [Header("INPUT")]
        public bool SnapInput = true;
        public float VerticalDeadZoneThreshold = 0.3f;
        public float HorizontalDeadZoneThreshold = 0.1f;

        [Header("MOVEMENT")]
        public float MaxSpeed = 14;
        public float Acceleration = 120;
        public float GroundDeceleration = 60;
        public float AirDeceleration = 30;
        public float GroundingForce = -1.5f;
        public float GrounderDistance = 0.05f;

        [Header("JUMP")]
        public float JumpPower = 36;
        public float MaxFallSpeed = 40;
        public float FallAcceleration = 110;
        public float JumpEndEarlyGravityModifier = 3;
        public float CoyoteTime = .15f;  // Time window after leaving the ground to still jump
        public float JumpBuffer = .2f;   // Time window to buffer a jump after pressing the button

        [Header("DASH")]
        public float DashSpeed = 30f;
        public float DashTime = 0.2f;
        public float DashCooldown = 0.5f;
        public TrailRenderer DashTrail;

        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private bool _grounded;
        private bool _isDashing;
        private bool _canDash = true;
        private bool _isJumping;

        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;

        // Implementing the IPlayerController interface members
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public Vector2 FrameInput => _frameInput.Move;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            GatherInput();

            // Dash input check (dash only when not dashing already)
            if (!_isDashing && _frameInput.DashPressed && _canDash)
            {
                StartCoroutine(PerformDash());
            }

            // Buffer the jump if the player presses the button within the jump buffer window
            if (_frameInput.JumpDown)
            {
                _jumpBufferCounter = JumpBuffer;
            }

            // Handle Coyote Time (allows jumping after leaving the ground for a short time)
            if (!_grounded)
            {
                _coyoteTimeCounter -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (!_isDashing)
            {
                CheckCollisions();
                HandleJump();
                HandleDirection();
                HandleGravity();
            }
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump"),
                JumpHeld = Input.GetButton("Jump"),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
                DashPressed = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Z)
            };
        }

        private IEnumerator PerformDash()
        {
            _isDashing = true;
            _canDash = false;
            DashTrail.emitting = true;

            // Dash in the direction the player is facing or moving
            float dashDirection = Mathf.Sign(_rb.linearVelocity.x) != 0 ? Mathf.Sign(_rb.linearVelocity.x) : Mathf.Sign(_frameInput.Move.x);
            _rb.linearVelocity = new Vector2(dashDirection * DashSpeed, 0);
            yield return new WaitForSeconds(DashTime);

            _isDashing = false;
            DashTrail.emitting = false;
            yield return new WaitForSeconds(DashCooldown);
            _canDash = true;
        }

        private void CheckCollisions()
        {
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, GrounderDistance, ~PlayerLayer);

            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteTimeCounter = CoyoteTime;  // Reset coyote time when hitting the ground
                GroundedChanged?.Invoke(true, _rb.linearVelocity.y);  // Trigger event when grounded
            }
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                GroundedChanged?.Invoke(false, _rb.linearVelocity.y);  // Trigger event when not grounded
            }
        }

        private void HandleJump()
        {
            // Jump logic
            if (_jumpBufferCounter > 0)
            {
                _jumpBufferCounter -= Time.deltaTime;
                if (_coyoteTimeCounter > 0 || _grounded)  // Allow jump if coyote time or grounded
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, JumpPower);  // Apply jump velocity only in Y axis
                    _isJumping = true;
                    Jumped?.Invoke();  // Trigger Jumped event
                }
            }

            // Apply gravity and smooth falling behavior (Celeste-style)
            if (!_grounded && !_isJumping)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -MaxFallSpeed)); // Cap fall speed
            }
            else
            {
                _isJumping = false;  // Reset jumping state when grounded
            }
        }

        private void HandleDirection()
        {
            // Apply horizontal movement
            if (_frameInput.Move.x != 0)
            {
                float targetSpeed = _frameInput.Move.x * MaxSpeed;
                float speedDifference = targetSpeed - _rb.linearVelocity.x;
                float movement = Mathf.Sign(speedDifference) * Mathf.Min(Mathf.Abs(speedDifference), Acceleration * Time.fixedDeltaTime);
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x + movement, _rb.linearVelocity.y);
            }
            else
            {
                // Apply deceleration when not moving horizontally
                if (_grounded)
                {
                    _rb.linearVelocity = new Vector2(Mathf.MoveTowards(_rb.linearVelocity.x, 0, GroundDeceleration * Time.fixedDeltaTime), _rb.linearVelocity.y);
                }
            }
        }

        private void HandleGravity()
        {
            if (!_grounded) _rb.linearVelocity += Vector2.down * FallAcceleration * Time.fixedDeltaTime;
        }
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
        public bool DashPressed;
    }

    public interface IPlayerController
    {
        event Action<bool, float> GroundedChanged;
        event Action Jumped;
        Vector2 FrameInput { get; }
    }
}

