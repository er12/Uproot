using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public Sprite secondSprite;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "AttackCheck")
        {
            StartCoroutine(AnimateAndFade());
        }

    }

    IEnumerator AnimateAndFade()
    {
        spriteRenderer.sprite = secondSprite;
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(
                    spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                   0.25f);
            yield return new WaitForSeconds(0.15f);

            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
            1f);

            yield return new WaitForSeconds(0.15f);
        }
        Destroy(gameObject);
    }
}
