using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour
{
    [SerializeField]
    GameObject cannonBall;

    public Transform cannonPosR;
    public Transform cannonPosL;

    private bool cooldown;

    public void FireCannon()
    {
        if (!cooldown)
        {
            Vector3 launchDirection = transform.right;
            Transform CannonBallR = Instantiate(cannonBall.transform, cannonPosR.transform.position, Quaternion.Euler(0, 0, 0));
            CannonBallR.GetComponent<CannonBallBehavior>().Launch(-launchDirection);
            Transform CannonBallL = Instantiate(cannonBall.transform, cannonPosL.transform.position, Quaternion.Euler(0, 0, 0));
            CannonBallL.GetComponent<CannonBallBehavior>().Launch(launchDirection);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(2.5f);
        cooldown = false;
    }
}
