
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Transform _target;
    private Rigidbody _rb;
    private Tower _tower;
    private float _enemyHP = 10;
    private float _enemyExp = 20;
    private float _enemyDamage = 10;

    public Tower Tower { get { return Tower; } }
    public float EnemyHP{ get { return _enemyHP; } set { _enemyHP = value; } }
    public float EnemyExp { get { return _enemyExp; } set { _enemyExp = value; } }
    public float EnemyDamage { get { return _enemyDamage; } set { _enemyDamage = value; } }

    private void Start()
    {
        _target = GameObject.FindWithTag("Tower").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _tower = _target.GetComponent<Tower>();
    }
    private void Update()
    {
        HPControl();
    }
    private void FixedUpdate()
    {
        //Attraction();
    }
    private void HPControl() 
    {
        if (_enemyHP <= 0)
        {
            _tower.Experience += _enemyExp;
            Kill();
        }
    }

    private void Attraction()
    {
        if (_target != null)
        {
            Vector3 randomVector = new Vector3(Random.Range(-30,30), 0, Random.Range(-30, 30));
            _rb.AddForce((_target.transform.position - gameObject.transform.position + randomVector).normalized * _speed);
        }
        else
            Destroy(gameObject);
    }
    
    public void Kill()
    {
        Destroy(gameObject);
    }


}
