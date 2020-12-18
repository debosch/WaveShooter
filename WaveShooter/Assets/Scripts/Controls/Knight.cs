using UnityEngine;

namespace Controls
{
    public class Knight : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float jumpForce = 800f;
        [SerializeField] private Vector2 boxSize = new Vector2(1f, 0.3f);
        [SerializeField] private Animator animator;
        
        private bool _isGrounded;
        private bool _jump;
        private bool _isFalling = false;
        private bool _facingRight = true;

        private float _direction;
        
        private Rigidbody2D _rb;
        private Vector2 _movement = Vector2.zero;
        
        private static readonly int JumpState = Animator.StringToHash("Jump");
        private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int AnimState = Animator.StringToHash("AnimState");

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            HandleInput();
            HandleAnimatorStates();
            Move(_direction);
            
            if (_isGrounded)
            {
                _jump = false;
            }
        }

        private void FixedUpdate()
        {
            _isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize,90,whatIsGround);
            _isFalling = _rb.velocity.y < 0;
        }

        private void HandleInput()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _jump = true;
                Jump();
            }

            if (Input.GetButton("Horizontal"))
            {
                _direction = Input.GetAxisRaw("Horizontal");
            }
            else if (Input.GetButtonUp("Horizontal"))
            {
                _direction = 0;
            }
        }

        private void HandleAnimatorStates()
        {
            if (_isGrounded && (int)_movement.x != 0)
            {
                animator.SetInteger(AnimState, 1);
            }
            else if (_isGrounded && (int) _movement.x == 0)
            {
                animator.SetInteger(AnimState, 0);
            }
            
            if (_jump)
            {
                animator.SetTrigger(JumpState);
            }

            if (_isFalling)
            {
                animator.SetFloat(AirSpeedY, _rb.velocity.y);
                animator.SetBool(Grounded, false);
            }
            else
            {
                animator.SetBool(Grounded, true);
            }
            
        }

        private void Jump()
        {
            _movement.y = jumpForce;
            _rb.AddForce(_movement);
            _movement.y = 0;
        }
        

        private void Flip()
        {
            _facingRight = !_facingRight;

            var playerTransform = transform;
            var targetScale = playerTransform.localScale;
            targetScale.x *= -1;
            playerTransform.localScale = targetScale;
        }

        private void Move(float dir)
        {
            _movement.x = dir * speed;
            _movement.y = _rb.velocity.y;
            _rb.velocity = _movement;

            if ((int)dir == 0)
            {
                _movement.x = 0;
                _movement.y = _rb.velocity.y;
                _rb.velocity = _movement;
            }

            if (!_facingRight && _movement.x > 0)
            {
                Flip();
            }
            else if (_facingRight && _movement.x < 0)
            {
                Flip();
            }
        }
    }
}
