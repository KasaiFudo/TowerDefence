using UnityEngine;

public class ShootTrigger : MonoBehaviour
{
    private Tower _tower;
    private SphereCollider _sphereCollider;
    public SphereCollider SphereCollider => _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _tower = gameObject.GetComponentInParent<Tower>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (_tower.enemyes.IndexOf(other.transform) == -1)
            {
                _tower.enemyes.Add(other.transform);
            }
        }
    }


}
