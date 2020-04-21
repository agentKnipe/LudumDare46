using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hero {
    private int _minHPRange;
    private int _maxHPRange;
    private int _maxHP;

    public HeroTypes HeroType;
    public string Name;

    public int HitPoints;
    public int AttackDamage;
    public float TimeBetweenAttacks;
    public int Level = 1;
    public int XP;

    public Attack[] Attacks { get; set; }

    public Hero() { }

    public Hero(Hero hero) {
        this.HeroType = hero.HeroType;
        this.Name = hero.Name;
        this.HitPoints = hero.HitPoints;
        this.AttackDamage = hero.AttackDamage;
        this.TimeBetweenAttacks = hero.TimeBetweenAttacks;
        this.Attacks = hero.Attacks;
    }

    public string DoAttack(int attackIndex, GameObject target) {
        var attack = Attacks[attackIndex];
        var damage = attack.AttackModifier + AttackDamage;

        if (attack.IsRegen) {
            HitPoints += damage;
        }
        else {
            if(target.GetComponent<IHitable>() != null) {
                target.GetComponent<IHitable>().TakeDamage(damage);

                return $"You attacked {target.name} for {damage} damage";
            }
        }

        return string.Empty;
    }

    public Hero GenerateArcher() {
        _minHPRange = 8;
        _maxHPRange = 20;

        this.HeroType = HeroTypes.archer;
        this.Name = HeroType.ToString();
        this.HitPoints = Random.Range(8, 20);
        this.AttackDamage = Random.Range(6, 15);
        this.TimeBetweenAttacks = 20f;

        SetHP();

        return this;
    }

    public Hero GenerateKnight() {
        _minHPRange = 10;
        _maxHPRange = 25;

        this.HeroType = HeroTypes.knight;
        this.Name = HeroType.ToString();
        this.AttackDamage = Random.Range(6, 18);
        this.TimeBetweenAttacks = 3f;

        this.Attacks = new Attack[] { 
            new Attack("Low Swipe", 0, 0, "Attack"),
            new Attack("Quick Attack", 1, 0, "Attack2"),
            new Attack("Spin Attack", 2, 1, "Attack3"),
            new Attack("Power Spin", 5, 2, "Attack4"),
            new Attack("Battle Cry", 2, 1, "BattleCry", true)
        };


        SetHP();

        return this;
    }

    public Hero GenerateWizard() {
        _minHPRange = 4;
        _maxHPRange = 8;

        this.HeroType = HeroTypes.wizard;
        this.Name = HeroType.ToString();
        this.HitPoints = Random.Range(4, 8);
        this.AttackDamage = Random.Range(10, 25);
        this.TimeBetweenAttacks = 30f;

        SetHP();

        return this;
    }

    public void TakeDamage(int damage) {
        HitPoints -= damage;
    }

    public void UpdateXP(int xp) {
        XP += xp;

        var newLevel = new Levels().GetLevel(XP);

        var levelDiff = newLevel - Level;

        if(levelDiff > 0) {
            for(int i = 0; i < levelDiff; i++) {
                this.AttackDamage = this.AttackDamage + (2 * Level);

                SetHP();
            }
        }
    }

    private void SetHP() {
        _maxHP += Random.Range(_minHPRange, _maxHPRange);

        HitPoints = _maxHP;
    }
}