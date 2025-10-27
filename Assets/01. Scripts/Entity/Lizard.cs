using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    public static Lizard Instance;

    Animator animator;
    Rigidbody2D _rigidbody;

    [SerializeField] private float forwardSpeed = 3f;
    [SerializeField] private float JumpForce = 3f;

    public bool isGodMode = false;

    private const string ObstacleTag = "Obstacle";

    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        // handle input
        if (IsGround())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump(JumpForce);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Jump_Down(JumpForce);
            }
        }
    }

    public void EnableControl()
    {
        isPlaying = true;
    }

    public void DisableControl()
    {
        isPlaying = false;
    }

    private void Jump(float force)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void Jump_Down(float force)
    {
        force *= -1f;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private bool IsGround()
    {
        return Mathf.Abs(_rigidbody.velocity.y) < 0.01f;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rigidbody.velocity;
        velocity.x = forwardSpeed;

        _rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGodMode)
            return;

        if (collision.gameObject.CompareTag(ObstacleTag))
        {
            Debug.Log("game over");
            animator.SetBool("IsHit", true);
            Time.timeScale = 0f;
        }
    }
}
