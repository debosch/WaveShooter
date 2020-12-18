using System;
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
        
        private bool _isGrounded;
        private Rigidbody2D _rb;
        private Vector2 _movement = Vector2.zero;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _movement.y = jumpForce;
                _rb.AddForce(_movement);
                _movement.y = 0;
            }

            if (Input.GetButton("Horizontal"))
            {
                Move(Input.GetAxisRaw("Horizontal"));
            }
            else
            {
                _movement.x = 0;
                _movement.y = _rb.velocity.y;
                _rb.velocity = _movement;
            }
        }

        private void FixedUpdate()
        {
            _isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize,90,whatIsGround);
        }

        private void Move(float dir)
        {
            _movement.x = dir * speed;
            _movement.y = _rb.velocity.y;
            _rb.velocity = _movement;
        }
    }
}
