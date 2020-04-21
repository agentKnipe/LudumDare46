using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour{
    private static List<GameObject> EnemyObjects = new List<GameObject>();
    private bool _enemiesLoaded = false;
    private int _xp = 0;

    private GameObject Hero;

    public EnemyPrefabEnumMapper[] EnemyPrefabs;
    public HeroPrefabEnumMapper[] HeroPrefabs;

    public GameObject[] EnemySpawns;
    public GameObject[] HeroSpawns;


    public GameObject Princess;

    public GameObject[] AttackButtons;
    public Button RunButton;

    public GameObject Vicory;
    public GameObject Lose;
    public GameObject PlaceholderText;

    void Start() {
        var hero = SpawnHeros();
        var heroController = hero.GetComponent<HeroController>();
        heroController.EnableButtonsEvent += EnableButtons;
        heroController.HeroLoadedEvent += MapButtons;
        heroController.HeroDiedEvent += GameOver;
        heroController.AttackedEnemyEvent += LogAttackText;

        Princess.GetComponent<PrincessController>().Load(GameState.Princess);

        RunButton.onClick.AddListener(Run);

        SpawnEnemies(hero);
    }

        // Update is called once per frame
    void Update(){
        if (_enemiesLoaded && !EnemyObjects.Any()) {
            Vicory.SetActive(true);

            Hero.GetComponent<HeroController>().SaveXP(_xp);

            Run();
        }
    }

    private void GameOver() {
        Lose.SetActive(false);

        //return to main menu
    }

    public void EnemyDestroyed(GameObject enemy) {
        EnemyObjects.Remove(enemy);

        _xp += enemy.GetComponent<EnemyController>().GetXP();

        LogAttackText($"You killed {enemy.name}");
    }

    private void SpawnEnemies(GameObject hero) {
        //determine how many enemies
        var numEnemies = UnityEngine.Random.Range(1, 4); // we have 3 potential spawn points
        for(int i = 0; i < numEnemies; i++) {
            //determine which enemies will spawn
            var randEnemy = EnemyPrefabs[UnityEngine.Random.Range(0, EnemyPrefabs.Length)];

            var enemy = new Enemy(randEnemy.EnemyType, GameState.Level);

            var newEnemy = Instantiate(randEnemy.Prefab, EnemySpawns[i].transform.position, Quaternion.identity);
            EnemyObjects.Add(newEnemy);

            var enemyController = newEnemy.GetComponent<EnemyController>();

            enemyController.EnemyDiedEvent += EnemyDestroyed;
            enemyController.AttackTextEvent += LogAttackText;
            enemyController.SetTarget(hero);
        }

        //spawn enemies

        _enemiesLoaded = true;
    }

    private GameObject SpawnHeros() {
        var hero = HeroPrefabs.Where(w => w.HeroType == GameState.Hero.HeroType).SingleOrDefault();

        if(hero != null) {
            var newHero = Instantiate(hero.Prefab, HeroSpawns[0].transform.position, Quaternion.identity);

            Hero = newHero;

            return newHero;
        }

        return null;
    }

    private void DisableButtons() {
        for (int i = 0; i < AttackButtons.Length; i++) {

            AttackButtons[i].GetComponent<Button>().enabled = false;
            var colors = AttackButtons[i].GetComponent<Button>().colors;
            colors.normalColor = Color.gray;

            AttackButtons[i].GetComponent<Button>().colors = colors;
        }

    }

    private void OnDestroy() {
        Destroy(Princess);
        Destroy(Hero);

        foreach (var obj in EnemyObjects) {
            Destroy(obj);
        }
    }

    private void Run() {
        SceneLoader.Instance.UnloadScene("CombatView");
    }

    private void EnableButtons() {
        for (int i = 0; i < AttackButtons.Length; i++) {
            AttackButtons[i].GetComponent<Button>().enabled = true;

            var colors = AttackButtons[i].GetComponent<Button>().colors;
            colors.normalColor = Color.white;

            AttackButtons[i].GetComponent<Button>().colors = colors;
        }
    }

    private void MapButtons(Hero hero, Action<int, Action> attackClicked) {
        for (int i = 0; i < AttackButtons.Length; i++) {
            if (hero.Attacks != null && i < hero.Attacks.Length) {
                var index = i;

                AttackButtons[index].GetComponentInChildren<Text>().text = hero.Attacks[index].AttackName;
                AttackButtons[index].GetComponent<Button>().onClick.AddListener(() => attackClicked(index, DisableButtons));
            }
            else {
                AttackButtons[i].SetActive(false);
            }
        }
    }

    private void LogAttackText(string attackText) {
        PlaceholderText.GetComponent<Text>().text = attackText;
    }
}
