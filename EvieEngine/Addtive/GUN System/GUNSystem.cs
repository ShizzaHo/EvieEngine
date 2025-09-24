using UnityEngine;

public class GUNSystem : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletGO = ObjectPool.Instance.Spawn(
                "Bullet",
                cam.transform.position + cam.transform.forward * 0.5f,
                cam.transform.rotation
            );

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Shoot(
                cam.transform.position + cam.transform.forward * 0.5f,
                cam.transform.forward
            );
        }*/
    }
}