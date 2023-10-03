using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    Dictionary<string, UnityAction> _actionsNames = new Dictionary<string, UnityAction>();
    private List<Button> _randomSkills;
    private GameManager _gameManager;
    private Tower _tower;
    private Camera _camera;
    private ShootTrigger _shotTrigger;
    public List<Button> RandomSkills => _randomSkills;
    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _tower = _gameManager.Tower;
        _shotTrigger = _tower.GetComponentInChildren<ShootTrigger>();
        _camera = Camera.main;
        LoadDictionary();
        _randomSkills = new List<Button>(3);
    }

    private void LoadDictionary()
    {
        _actionsNames.Add("AttackSpeed(Clone)", AttackSpeed);
        _actionsNames.Add("TowerSize(Clone)", TowerSize);
        _actionsNames.Add("Damage(Clone)", Damage);
        _actionsNames.Add("TowerRepair(Clone)", TowerRepair);
        _actionsNames.Add("ViewRadius(Clone)", ViewRadius);
        _actionsNames.Add("AttackRadius(Clone)", AttackRadius);
        Debug.Log($"Все действия для кнопок загружены.");
    }
    public void LoadAvailableSkills() //при создании кнопок через справочник ищет их функции и добавляет
    {
        for (int i = 0; i < _randomSkills.Count; i++)
        {
            if (_actionsNames.ContainsKey(_randomSkills[i].name))
            {
                _randomSkills[i].onClick.AddListener(_actionsNames[_randomSkills[i].name]);
                Debug.Log($"Добавлен {_actionsNames[_randomSkills[i].name]} в кнопку {_randomSkills[i].name} ");
            }
            else
                Debug.Log($"Имена кнопки и действия не совпадают");
        }
    }
    //ниже идут методы которые надо закинуть на кнопки
    public void AttackSpeed()
    {
        _tower.AttackSpeed -= 0.1f;
        DestroyAll();
        _gameManager.Continue();
    }
    public void TowerSize()
    {
        _tower.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        _tower.MaxHealth += 10;
        DestroyAll();
        _gameManager.Continue();
    }
    public void Damage()
    {
        _tower.CurrentDamage += 10;
        DestroyAll();
        _gameManager.Continue();
    }
    public void TowerRepair()
    {
        _tower.Health += 20;
        if (_tower.Health > _tower.MaxHealth)
            _tower.Health = _tower.MaxHealth;
        DestroyAll();
        _gameManager.Continue();
    }
    public void ViewRadius()
    {
        Debug.Log(_camera);
        _camera.orthographicSize++;
        DestroyAll();
        _gameManager.Continue();
    }
    public void AttackRadius()
    {
        _shotTrigger.SphereCollider.radius += 0.1f;
        DestroyAll();
        _gameManager.Continue();
    }
    private void DestroyAll()
    {
        foreach(var skill in _randomSkills)
        {
            Destroy(skill);
        }
        _randomSkills.Clear();
    }
}
