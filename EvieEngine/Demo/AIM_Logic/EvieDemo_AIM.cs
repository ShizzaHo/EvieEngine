using UnityEngine;

public class EvieDemo_AIM : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject prefab;
    
    public void OnAim()
    {
        Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
