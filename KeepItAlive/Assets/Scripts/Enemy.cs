using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy {
    public EnemyTypes EnemyType;
    public string Name;
    public int HitPoints;
    public int AttackDamage;
    public float TimeBetweenAttacks;
    public int baseXP;

    public Attack[] Attacks { get; set; }

    public string DoAttack(GameObject target) {
        var attack = Attacks[Random.Range(0, Attacks.Length)];

        var damage = attack.AttackModifier + AttackDamage;

        target.GetComponent<IHitable>().TakeDamage(damage);

        return $"{Name} Attacked you with {attack.AnimationName} for {damage} Damaage.";
    }

    public Enemy(EnemyTypes enemyType, int level) {
        switch (enemyType) {
            case EnemyTypes.archer:
                GenerateArcher(level);
                break;
            case EnemyTypes.burningGhoul:
                GenerateGhoul(level);
                break;
            case EnemyTypes.darkAngel:
                GenerateAngel(level);
                break;
            case EnemyTypes.evilWizard:
                GenerateWizard(level);
                break;
            case EnemyTypes.monk:
                GenerateMonk(level);
                break;
            case EnemyTypes.shieldSkeleton:
                GenerateShieldSkeleton(level);
                break;
            case EnemyTypes.spearmanSkeleton:
                GenerateSpearSkeleton(level);
                break;
            case EnemyTypes.warriorSkeleton:
                GenerateWarriorSkeleton(level);
                break;
        }
    }

    public void GenerateArcher(int level) {
        this.EnemyType = EnemyTypes.archer;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(8 + level, 20 + (2 * level));
        this.AttackDamage = Random.Range(6 + (2 * level), 15 + (2 * level));
        this.TimeBetweenAttacks = 60f;

        this.baseXP = 50 * level;

        this.Attacks = new Attack[] {
            new Attack("Arrow", 1, 3, "attack")
        };
    }

    public void GenerateGhoul(int level) {
        this.EnemyType = EnemyTypes.burningGhoul;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(4 + level, 12 + (2 * level));
        this.AttackDamage = Random.Range(6 + (2 * level), 15 + (2 * level));
        this.TimeBetweenAttacks = 120f;

        this.baseXP = 75 * level;

        this.Attacks = new Attack[] {
            new Attack("Slash", 2, 3, "attack")
        };
    }

    public void GenerateAngel(int level) {
        this.EnemyType = EnemyTypes.darkAngel;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(12 + level, 25 + (2 * level));
        this.AttackDamage = Random.Range(6 + (2 * level), 15 + (2 * level));
        this.TimeBetweenAttacks = 60f;

        this.baseXP = 125 * level;

        this.Attacks = new Attack[] {
            new Attack("Hell fire", 5, 10, "attack")
        };
    }

    public void GenerateWizard(int level) {
        this.EnemyType = EnemyTypes.evilWizard;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(4 + level, 12 + (2 * level));
        this.AttackDamage = Random.Range(10 + (2 * level), 20 + (2 * level));
        this.TimeBetweenAttacks = 75f;

        this.baseXP = 100 * level;

        this.Attacks = new Attack[] {
            new Attack("Fireball", 5, 10, "attack")
        };
    }

    public void GenerateMonk(int level) {
        this.EnemyType = EnemyTypes.monk;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(12 + level, 25 + (2 * level));
        this.AttackDamage = Random.Range(5 + (2 * level), 16 + (2 * level));
        this.TimeBetweenAttacks = 45f;

        this.baseXP = 50 * level;

        this.Attacks = new Attack[] {
            new Attack("Punch", 2, 3, "attack")
        };
    }

    public void GenerateShieldSkeleton(int level) {
        this.EnemyType = EnemyTypes.shieldSkeleton;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(12 + level, 25 + (2 * level));
        this.AttackDamage = Random.Range(5 + (2 * level), 16 + (2 * level));
        this.TimeBetweenAttacks = 75f;

        this.baseXP = 100 * level;

        this.Attacks = new Attack[] {
            new Attack("Sword", 3, 3, "attack")
        };
    }

    public void GenerateSpearSkeleton(int level) {
        this.EnemyType = EnemyTypes.spearmanSkeleton;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(8 + level, 18 + (2 * level));
        this.AttackDamage = Random.Range(8 + (2 * level), 16 + (2 * level));
        this.TimeBetweenAttacks = 60f;

        this.baseXP = 100 * level;

        this.Attacks = new Attack[] {
            new Attack("Spear", 2, 3, "attack")
        };
    }

    public void GenerateWarriorSkeleton(int level) {
        this.EnemyType = EnemyTypes.warriorSkeleton;
        this.Name = EnemyType.ToString();
        this.HitPoints = Random.Range(12 + level, 25 + (2 * level));
        this.AttackDamage = Random.Range(10 + (2 * level), 20 + (2 * level));
        this.TimeBetweenAttacks = 45f;

        this.baseXP = 50 * level;

        this.Attacks = new Attack[] {
            new Attack("Sword", 5, 10, "attack")
        };
    }


    public void TakeDamage(int damage) {
        HitPoints -= damage;
    }
}