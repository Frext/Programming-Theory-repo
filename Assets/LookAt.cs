using UnityEngine;

public class LookAt : MonoBehaviour
{
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
