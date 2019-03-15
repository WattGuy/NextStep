using UnityEngine;

public enum HeroType
{

    BLUE,
    GREEN,
    PINK,
    ORANGE,
    LIGHTGREEN,
    RAINBOW,
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

public class TypeUtils{

    public static string GetDirectory(HeroType ht) {

        if (ht != HeroType.CONCRETE) {

            return "Pieces/" + getName(ht) + " Piece";

        } else return "Pieces/" + getName(ht);

    }

    private static string getName(HeroType ht) {

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
        else return "";

    }

}
