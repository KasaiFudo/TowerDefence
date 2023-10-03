using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float _speedBullet = 10f;
    private Transform _target;
    private int _damage;
    public int Damage { set { _damage = value; } }
    public Transform Target { set { _target = value; } }

    private void Update()
    {
        ChasingTarget();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.EnemyHP -= _damage;
            Destroy(gameObject);
        }
    }

    private void ChasingTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speedBullet * Time.deltaTime); ;
    }
}
