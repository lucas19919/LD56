using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float Health = 99;
    public float duration = 3f;

    public static int shipsSunkCount;
    
    public void cannonHit(float damage)
    {
        Health -= damage;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            if (this.gameObject.tag == "Enemy")
            {
                gameObject.GetComponent<EnemyController>().enabled = false;
                gameObject.GetComponent<EnemyShipFloat>().enabled = false;
                onDeath();
            }    

            if (this.gameObject.tag == "Player")
            {
                SceneController.DeathScreen();
            }
        }
    }

    void onDeath()
    {
        StartCoroutine(Lower());
    }
    IEnumerator Lower()
    {
            float startTime = Time.time;
            Vector3 initialPosition = this.transform.position;
            Vector3 targetPosition = initialPosition + (Vector3.down);

            while (Time.time - startTime < duration)
            {
                float t = (Time.time - startTime) / duration;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                yield return null;
            }

            transform.position = targetPosition;
            shipsSunkCount++;
            checkWinCondition();
            Destroy(this.gameObject);
    }

    private void checkWinCondition()
    {
        if (shipsSunkCount == 3)
        {
            SceneController.WinScreen();
        }

        Debug.Log(shipsSunkCount);
    }
}