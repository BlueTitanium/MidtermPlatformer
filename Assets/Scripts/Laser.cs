using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100f;
    public LineRenderer line;
    public float damage = 6f;
    public Transform rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = (transform.position - rotationPoint.position).normalized;
        if (Physics2D.Raycast(transform.position, dir))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
            Draw2DRay(transform.position, hit.point);
            if (hit.transform.gameObject.CompareTag("Enemy"))
            { 
               hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                FindObjectOfType<CameraShaker>().ShakeCamera(.7f, .3f);
            }
        } else
        {
            Draw2DRay(transform.position, dir * defDistanceRay);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
