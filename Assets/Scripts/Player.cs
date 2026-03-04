using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float tilt = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private int spriteIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[0];
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;

        rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        // Input (works for both WebGL + Editor)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * strength;
        }

        // Tilt based on velocity
        Vector3 rotation = transform.eulerAngles;
        rotation.z = rb.velocity.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }

        if (spriteIndex >= 0 && spriteIndex < sprites.Length) {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) {
            GameManager.Instance.GameOver();
        } 
        else if (other.CompareTag("Scoring")) {
            GameManager.Instance.IncreaseScore();
        }
    }
}