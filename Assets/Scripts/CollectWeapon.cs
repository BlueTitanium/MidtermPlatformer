using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    public int potentialNextWeaponLength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var p = collision.gameObject.GetComponent<PlayerController>();
            if(p.curLength < potentialNextWeaponLength)
            {
                p.curLength = potentialNextWeaponLength;
                p.imageBackgrounds[potentialNextWeaponLength - 1].color = p.colors[0];
                p.playerUIAnim.Play();
            }
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var p = collision.gameObject.GetComponent<PlayerController>();
            if (p.curLength < potentialNextWeaponLength)
            {
                p.curLength = potentialNextWeaponLength;
                p.imageBackgrounds[potentialNextWeaponLength - 1].color = p.colors[0];
                p.playerUIAnim.Play();
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var p = collision.gameObject.GetComponent<PlayerController>();
            if (p.curLength < potentialNextWeaponLength)
            {
                p.curLength = potentialNextWeaponLength;
                p.imageBackgrounds[potentialNextWeaponLength-1].color = p.colors[0];
                p.playerUIAnim.Play();
            }
            Destroy(gameObject);
        }
    }
}
