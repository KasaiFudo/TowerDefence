using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Tower : MonoBehaviour
{

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _spawnBulletPoint;
    private static GameManager _gameManager;
    private Transform _closedEnemy;
    private bool _isCooldown = false;
    private float _cooldown;
    private static float _attackSpeed = 1.5f;
    private static float _experience = 0;
    private static int _maxExperience = 100;
    private static int _level = 0;
    private int _currentDamage = 10;
    private float _health = 100;
    private float _maxHealth = 100;
    [NonSerialized] public List<Transform> enemyes = new();
    public float Experience { get { return _experience; } set { _experience = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public int CurrentDamage { get { return _currentDamage; } set { _currentDamage = value; } }
    public float Health { get { return _health; } set { _health = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int MaxExperience => _maxExperience;
    public float Cooldown => _cooldown;
    public Transform ClosedEnemy => _closedEnemy;
    public int Level => _level;
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _cooldown = _attackSpeed; //Do this for escape mistakes
    }

    private void Update()
    {
        Shoot();
        EnemiesClearer();
        LevelControl();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _health -= collision.gameObject.GetComponent<Enemy>().EnemyDamage;
            if(_health <= 0)
                Destroy(gameObject);
        }
    }
    private void Shoot() //если противник рядом и кулдауна нет - создает пулю и говорит куда ей лететь
    {
        if (!_isCooldown && enemyes.Count != 0)
        {
            var bullet = Instantiate(_bullet,_spawnBulletPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Target = FindClosestObject(enemyes);
            bullet.GetComponent<Bullet>().Damage = _currentDamage;
            StartCoroutine(ShootCooldown()); //запускает кулдаун
        }
    }
    private void EnemiesClearer()
    {
        for (int i = enemyes.Count - 1; i >= 0; i--)
        {
            if (enemyes[i] == null)
            {
                enemyes.RemoveAt(i);
            }
        }
        if(gameObject == null)
            enemyes.Clear();
    }

    private void LevelControl()
    {
        if(_experience >= _maxExperience)
        {
            _level++;
            _maxExperience *= 2;
            _experience = 0;
            _gameManager.LevelUp();
        }
    }
    Transform FindClosestObject(List<Transform> objects)
    {
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Transform obj in objects)
        {
            float distanceSqrToObj = (obj.transform.position - transform.position).sqrMagnitude;
            if (distanceSqrToObj < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToObj;
                _closedEnemy = obj;
            }
        }

        return _closedEnemy;
    }
    IEnumerator ShootCooldown() //запускает сам кулдаун
    {
        _isCooldown = true;
        while ((_cooldown -= Time.deltaTime) >= 0.0f)
        {
            yield return null;
        }
        _cooldown = _attackSpeed;
        _isCooldown = false;
    }
}