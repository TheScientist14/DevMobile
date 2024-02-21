using System.Collections;
using UnityEngine;

public class PoolDebug : MonoBehaviour
{
    private float _timeBeforeDestroy = 1.5f;

    private Touch theTouch;
    private float timeTouchEnded;

    void Update()
    {
        // To debug in classic editor
        HandleSpaceInput();
        
        // For Mobile Device Simulator
        HandleTouchInput();
    }

    private void HandleSpaceInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBullet();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Stationary)
            {
                SpawnBullet();
                timeTouchEnded = Time.time;
            }
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = BulletsPool.SharedInstance.GetPooledObject().gameObject;
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
    }

    private IEnumerator DeactivateBullet(GameObject gameObject) 
    {
        yield return new WaitForSeconds(_timeBeforeDestroy);

         gameObject.SetActive(false);
    }

}
