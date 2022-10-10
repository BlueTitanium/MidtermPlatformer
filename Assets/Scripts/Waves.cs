using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Attack()
    {
        base.Attack();
        print("MAGIC BLAST");
    }
    public override void Special()
    {
        base.Special();
        print("HYPER MAGIC BEAM CANNON ULTRA MOVE!");

    }
}
