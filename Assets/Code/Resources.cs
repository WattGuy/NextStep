using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Resources {

    public Dictionary<KeyValuePair<HeroType, DLCType>, Sprite> sprites = new Dictionary<KeyValuePair<HeroType, DLCType>, Sprite>();

    public Resources() {

        foreach (HeroType ht in Enum.GetValues(typeof(HeroType)).Cast<HeroType>()) {

            foreach (DLCType dt in Enum.GetValues(typeof(DLCType)).Cast<DLCType>()) {

                sprites.Add(new KeyValuePair<HeroType, DLCType>(ht, dt), UnityEngine.Resources.Load<Sprite>(TypeUtils.GetDirectory(ht, dt)));

            }

        }

    }

}
