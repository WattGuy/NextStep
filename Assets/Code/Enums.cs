﻿using System;
using UnityEngine;

public enum HeroType
{

    BLUE,
    GREEN,
    PINK,
    ORANGE,
    LIGHTGREEN,
    CONCRETE,
    NONE

}

public enum DLCType
{

    HORIZONTAL,
    VERTICAL,
    ADJACENT,
    BOMB,
    NONE

}

public enum OnType {

    ICE = 1,
    CHAINS = 3,
    NONE = 0

}

public enum PopupType {

    PAUSE,
    LOSE,
    WIN,
    LEVEL

}

public class TypeUtils{

    public static HeroType? getType(string s) {

        try
        {
            return (HeroType) Enum.Parse(typeof(HeroType), s);
        }
        catch (Exception ignored) { }

        return null;

    }

    public static DLCType? getDType(string s)
    {

        try
        {
            return (DLCType)Enum.Parse(typeof(DLCType), s);
        }
        catch (Exception ignored) { }

        return null;

    }

    public static OnType? getOType(string s)
    {

        try
        {
            return (OnType) Enum.Parse(typeof(OnType), s);
        }
        catch (Exception ignored) { }

        return null;

    }

    public static string getName(OnType ot) {

        if (ot == OnType.ICE)
        {

            return "Ice";

        }
        else if (ot == OnType.CHAINS)
        {

            return "Chains";

        }
        else return "";

    }

    public static string getName(HeroType ht) {

        if (ht == HeroType.BLUE)
        {

            return "Blue";

        }
        else if (ht == HeroType.GREEN)
        {

            return "Green";

        }
        else if (ht == HeroType.LIGHTGREEN)
        {

            return "Light Green";

        }
        else if (ht == HeroType.PINK)
        {

            return "Pink";

        }
        else if (ht == HeroType.ORANGE)
        {

            return "Orange";

        } else if (ht == HeroType.CONCRETE) {

            return "Concrete";

        }
        else return "";

    }

    public static string GetDirectory(HeroType ht, DLCType dt)
    {

        if (dt == DLCType.HORIZONTAL)
        {

            return "Pieces/" + getName(ht) + " Row";

        }
        else if (dt == DLCType.VERTICAL)
        {

            return "Pieces/" + getName(ht) + " Column";

        }
        else if (dt == DLCType.ADJACENT)
        {

            return "Pieces/" + getName(ht) + " Adjacent";

        }
        else if (dt == DLCType.BOMB)
        {

            return "Pieces/" + getName(ht) + " Bomb";

        }
        else {

            if (ht != HeroType.CONCRETE)
            {

                return "Pieces/" + getName(ht) + " Piece";

            }
            else return "Pieces/" + getName(ht);

        }

    }

}
