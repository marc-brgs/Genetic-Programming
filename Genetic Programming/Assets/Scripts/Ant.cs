using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant
{
    public int startX;
    public int startY;

    public int posX;
    public int posY;

    public int direction = 2; // 1 : top, 2 : right, 3 : bottom, 4 : left
    
    public Ant(int startX, int startY)
    {
        this.startX = startX;
        this.startY = startY;
        this.posX = startX;
        this.posY = startY;
    }
}
