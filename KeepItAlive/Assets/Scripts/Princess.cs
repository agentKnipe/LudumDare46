using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Princess : IHitable {
    public int HitPoints { get; set; }

    public void TakeDamage(int damage) {
        HitPoints -= damage;
    }
}