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
    [SerializeField] private GameObject _gameInterface;
    [SerializeField] private GameObject _deadMenu;
    [SerializeField] private GameObject _levelUpMenu;
    [SerializeField] private TMP_Text _gameTimer;
    [SerializeField] private List<Transform> _skillsPos;
    [SerializeField] private List<Button> _availableSkills;
    private Slider _sliderLVL;
    private TMP_Text _textHealth;
    private TMP_Text _textCooldown;
    private Tower _tower;
    private Skills _skills;
    private int _minunes;
    private int _seconds;
    private int _wave;
    private int _enemyCount = 3;
    public Tower Tower { get { return _tower; } set { _tower = value; } }
    private void Start()
    {
        _skills = GetComponent<Skills>();
        _textCooldown = GameObject.Find("Cooldown").GetComponent<TMP_Text>();
        _textHealth = GameObject.Find("HP").GetComponent<TMP_Text>();
        Debug.Log(_textHealth);
        _tower = GameObject.Find("Tower").GetComponent<Tower>();
        _sliderLVL = GameObject.Find("SliderLVL").GetComponent<Slider>();

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
        _availableSkills = _availableSkills.OrderBy(x => Random.value).ToList<Button>();
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

    private void GenerateEnemyes()
    {
        Vector3[] spawnPosition = new Vector3[_enemyCount];
        float buffer = 1000f;  // adjust this based on the size of your objects
        Vector3 worldMax = Camera.main.ScreenToWorldPoint(new Vector2(-buffer, -buffer));
        Vector3 worldMin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + buffer, Screen.height + buffer));
        for (int i = 0; i < _enemyCount; i++)
        {
            spawnPosition[i] = new Vector3(Random.Range(worldMin.x, worldMax.x), 0, Random.Range(worldMin.z, worldMax.z));
        }
        Vector3 CameraDownLeftPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector3 CameraUpRightPoint = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Create(_enemy, spawnPosition);

    }
    private void Create(GameObject obj, Vector3[] position)
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            var enemyStats = Instantiate(obj, position[i], Quaternion.identity).GetComponent<Enemy>();
            enemyStats.EnemyHP += _wave * 1.05f;
            enemyStats.EnemyExp += _wave * 1.1f;
            enemyStats.EnemyDamage += _wave * 1.01f;
        }
    }


    private void InterfaceConroll()
    {
        _textHealth.text = $"Health: {System.Math.Round(_tower.Health, 1)}/{System.Math.Round(_tower.MaxHealth, 1)}"; //Control tower's HP
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
    private IEnumerator IGameTimer()
    {
        //SpawnEnemy();
        while (true)
        {
            if (_seconds == 59)
            {
                _minunes++;
                _seconds = -1;
                _wave++;
                _enemyCount++;
            }
            if (_seconds % 20 == 0)
            {
                GenerateEnemyes();
            }
            _seconds += 1;
            _gameTimer.text = _minunes.ToString("D2") + ":" + _seconds.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }
}
