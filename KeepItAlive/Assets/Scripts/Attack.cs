using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Attack {
    public string AttackName { get; set; }
    public int AttackModifier { get; set; }
    public float AttackTimeout { get; set; }
    public string AnimationName { get; set; }
    public bool IsRegen { get; set; }

    public Attack(string attackName, int attackModifier, float attackTimeout, string animationName, bool isRegen = false) {
        this.AttackName = attackName;
        this.AttackModifier = AttackModifier;
        this.AttackTimeout = attackTimeout;
        this.AnimationName = animationName;
        this.IsRegen = IsRegen;
    }
}
