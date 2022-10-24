using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRespawn : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
      var playerController = GameObject.FindObjectOfType<PlayerController>();
      var lm = GameObject.FindObjectOfType<LevelManager>();
      if (lm != null)
      {
        transform.position = new Vector3(lm.checkPoint.x, lm.checkPoint.y, 0);
      }
      playerController.curIndex = lm.weaponEquipped;
      playerController.curLength = lm.weaponLength;
      if(lm.weaponEquipped == 2)
      {
        StartCoroutine(startTrail());
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator startTrail()
    {
      yield return new WaitForSeconds(0.05f);
      GetComponent<Guitar>().sprintTrail.mbEnabled = true;
    }
}
