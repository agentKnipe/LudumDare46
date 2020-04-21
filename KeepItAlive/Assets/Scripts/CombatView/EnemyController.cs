using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IHitable{
    private Animator _animator { get; set; }
    private Enemy Enemy;

    private float _timer = 0;

    public delegate void AttackText(string text);
    public event AttackText AttackTextEvent;

    public delegate void EnemyDied(GameObject enemy);
    public event EnemyDied EnemyDiedEvent;

    public GameObject Target;

    public EnemyTypes EnemyType;

    public void TakeDamage(int damage) {
        Enemy.TakeDamage(damage);

        if(Enemy.HitPoints <= 0) {
            if(EnemyDiedEvent != null) {
                EnemyDiedEvent(gameObject);
            }

            //figure out death animation

            gameObject.SetActive(false);
            Destroy(gameObject, 5f);
        }
    }

    // Start is called before the first frame update
    void Start(){
        Enemy = new Enemy(EnemyType, GameState.Level);

        _animator = this.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        if(_timer >= Enemy.TimeBetweenAttacks) {
            var attack = Enemy.DoAttack(Target);

            AttackTextEvent(attack);

            _timer = 0;
        }
        else {
            _timer += Time.deltaTime;
        }

    }

    public int GetXP() {
        return Enemy.baseXP;
    }

    public void SetTarget(GameObject target) {
        Target = target;
    }
}
