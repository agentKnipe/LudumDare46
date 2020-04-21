using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels{
    public List<Level> XPLevels { get; set; }
   
    public Levels() {
        XPLevels = new List<Level>();
        var previousLevel = 0;

        for(int i = 1; i <= 100; i++) {
            var level = new Level() {
                LevelValue = i,
                XP = previousLevel + (i * 100)
            };

            XPLevels.Add(level);
        }
    }

    public int GetLevel(int xpValue) {
        var levels = XPLevels.Where(w => w.XP < xpValue).OrderByDescending(o => o.XP).ToList();

        return levels.First().LevelValue;
    }
}


public class Level {
    public int LevelValue { get; set; }
    public int XP { get; set; }
}