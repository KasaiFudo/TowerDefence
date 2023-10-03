using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _secondsUntilSpawn = 3f;
    [SerializeField] private GameObject _gameInterface;
    [SerializeField] private GameObject _deadMenu;
    [SerializeField] private GameObject _levelUpMenu;
    [SerializeField] private TMP_Text _gameTimer;
    [SerializeField] private List<Transform> _skillsPos;
    [SerializeField] private List<Button> _availableSkills;
    private Camera _camera;
    private Slider _sliderLVL; 
    private TMP_Text _textHealth;
    private TMP_Text _textCooldown;
    private Tower _tower;
    private Skills _skills;
    private int _minunes;
    private int _seconds;
    private int _delta = 1;
    private int _wave;
    private int _enemyCount = 3;
    public Tower Tower { get { return _tower; } set { _tower = value; } }
    private void Start()
    {
        _skills = GetComponent<Skills>();
        _camera = Camera.main;
        _textCooldown = GameObject.Find("Cooldown").GetComponent<TMP_Text>();
        _textHealth = GameObject.Find("HP").GetComponent<TMP_Text>();
        _tower = GameObject.Find("Tower").GetComponent<Tower>();
        _sliderLVL = GameObject.Find("SliderLVL").GetComponent<Slider>();

        if(_tower != null )
            StartCoroutine(ISpawnTimer());
        StartCoroutine(IGameTimer());
    }
    private void Update()
    {
        InterfaceConroll();
        GameOver();
    }

    public void LevelUp()
    {
        Pause();
        _levelUpMenu.SetActive(true);
        _availableSkills.OrderBy(x => Random.Range(0, _availableSkills.Count)).ToArray();
        short i = 0;
        foreach (Transform buttonPos in _skillsPos)
        {
            var skill = Instantiate(_availableSkills[i]);
            _skills.RandomSkills.Add(skill);
            skill.transform.SetParent(buttonPos.transform, false);
            skill.transform.localPosition = Vector3.zero;
            Debug.Log($"Создана карта в позитиции {buttonPos}. Всего карт: {_skillsPos.Count}");
            i++;
        }
        _skills.LoadAvailableSkills();     
    }
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        _levelUpMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void SpawnEnemy()
    {
        Vector3[] spawnPosition = new Vector3[_enemyCount];
        float buffer = 1000f;  // adjust this based on the size of your objects
        Vector3 worldMax = Camera.main.ScreenToWorldPoint(new Vector2(-buffer, -buffer));
        Vector3 worldMin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + buffer, Screen.height + buffer));
        for(int i = 0; i > _enemyCount; i++)
        {
            spawnPosition[i] = new Vector3(Random.Range(worldMin.x, worldMax.x), 0, Random.Range(worldMin.z, worldMax.z));
        }

        Vector3 CameraDownLeftPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector3 CameraUpRightPoint = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //if (spawnPosition.x < CameraDownLeftPoint.x || spawnPosition.x > CameraUpRightPoint.x && spawnPosition.z < CameraDownLeftPoint.z || spawnPosition.z > CameraUpRightPoint.z) //No spawn enemyes in camera
        //{
        //    Create(_enemy, spawnPosition);
        //}
        Create(_enemy, spawnPosition);

    }
    private void Create(GameObject obj, Vector3[] position)
    {
        if (_wave % 2 != 0)
        {
            for(int i = 0; i < _enemyCount; i++)
            {
                var enemyStats = Instantiate(obj, position[i], Quaternion.identity).GetComponent<Enemy>();
                enemyStats.EnemyHP += _tower.Level * 3;
                enemyStats.EnemyExp += _tower.Level * 2;
                enemyStats.EnemyDamage += 3;
            }
        }
        else 
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                Instantiate(obj, position[i], Quaternion.identity);
            }
        }
    }


    private void InterfaceConroll()
    {
        _textHealth.text = $"Health: {_tower.Health}/{_tower.MaxHealth}"; //Control tower's HP
        _textCooldown.text = "Attack Speed:" + System.Math.Round(_tower.Cooldown, 2);
        _sliderLVL.maxValue = _tower.MaxExperience;
        _sliderLVL.value = _tower.Experience;
    }
    private void GameOver()
    {
        if (_tower.Health <= 0)
        {
            _gameInterface.SetActive(false);
            _deadMenu.SetActive(true);
            StopAllCoroutines();
        }
    }
    private IEnumerator ISpawnTimer()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_secondsUntilSpawn);
        }
    }
    private IEnumerator IGameTimer()
    {
        while (true)
        {
            if (_seconds == 59)
            {

                _minunes++;
                _seconds = -1;
            }
            if(_seconds == 30)
            {
                _wave++;
            }
            _seconds += _delta;
            _gameTimer.text = _minunes.ToString("D2") + ":" + _seconds.ToString("D2");
            yield return new WaitForSeconds(1);
        } 
    }
}
