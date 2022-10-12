using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
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
        print("SWORD STRIKE");
    }
    public override void Special()
    {
        base.Special();
        print("SWORD OBLITERATION ATTACK SPECIAL HIT!");

    }
}
