using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour, IHitable{
    public delegate void EnableButtons();
    public delegate void HeroLoaded(Hero hero, Action<int, Action> attackClicked);
    public delegate void HeroDied();
    public delegate void AttackedEnemy(string attackText);

    public event EnableButtons EnableButtonsEvent;
    public event HeroLoaded HeroLoadedEvent;
    public event AttackedEnemy AttackedEnemyEvent;
    public event HeroDied HeroDiedEvent;

    private Animator _animator;
    private GameObject _target;

    private float _timer = 0;
    private bool _buttonsEnabled = true;

    private Hero Hero;

    public HeroTypes HeroType;

    // Start is called before the first frame update
    void Start(){
        Hero = new Hero(GameState.Hero);

        if(HeroLoadedEvent != null) {
            HeroLoadedEvent(Hero, AttackClicked);
        }

        _animator = this.GetComponentInParent<Animator>();
        _timer = Hero.TimeBetweenAttacks;
    }

    // Update is called once per frame
    void Update(){
        if(_timer >= Hero.TimeBetweenAttacks && !_buttonsEnabled) {
            if(EnableButtonsEvent != null) {
                EnableButtonsEvent();
                _buttonsEnabled = true;
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Mouse is down");

            var hit = Physics2D.GetRayIntersection(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition));

            if (hit.collider != null) {
                var hitInfo = hit.collider;
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                SetTarget(hitInfo.transform.gameObject);
            }
            else {
                Debug.Log("No hit");
            }

            Debug.Log("Mouse is down");
        }

        if(Hero != null && Hero.HitPoints <= 0) {
            HeroDiedEvent();
        }

        _timer += Time.deltaTime;
    }

    public void AttackClicked(int number, Action DisableButtons) {
        if(_target != null) {
            var attackText = Hero.DoAttack(number, _target);

            _animator.SetTrigger("Attack");

            DisableButtons();
            _buttonsEnabled = false;

            AttackedEnemyEvent(attackText);

            _timer = 0;
        }
    }

    public void TakeDamage(int damage) {
        Hero.HitPoints -= damage;
    }

    public void SaveXP(int xp) {
        Hero.UpdateXP(xp);

        GameState.Hero = Hero;
    }

    private void RemoveTarget(GameObject target) {
        _target = null;
    }

    private void SetTarget(GameObject target) {
        if (target.tag == "Enemy") {
            if (_target != null) {
                _target.GetComponent<ISelectable>().Unselected();
            }

            _target = target;
            target.GetComponent<ISelectable>().Selected();

            //remove the previous handler and readd for the new target
            target.GetComponent<EnemyController>().EnemyDiedEvent -= RemoveTarget;
            target.GetComponent<EnemyController>().EnemyDiedEvent += RemoveTarget;
        }
        else {
            Debug.Log("nopz");
        }
    }
}
