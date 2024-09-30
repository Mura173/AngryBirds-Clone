using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private int limit;
    private SpriteRenderer spriteR;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private ParticleSystem bomb;
    // Start is called before the first frame update
    void Start()
    {
        limit = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Relative velocity = velocidade linear relativa de dois objetos colidindo
        if (other.relativeVelocity.magnitude > 4 && other.relativeVelocity.magnitude < 10)
        {
            if (limit < sprites.Length - 1)
            {
                limit++;
                spriteR.sprite = sprites[limit];
            }
            else if (limit == sprites.Length - 1)
            {
                Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else if (other.relativeVelocity.magnitude > 12 && other.gameObject.CompareTag("Player"))
        {
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
