using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[][] map;
    private int rows = 16;
    private int cols = 10;
    
    private Vector2 offsetMapStart = new Vector2(-0.9f, -3.2f);
    
    // Start is called before the first frame update
    void Start()
    {
        map = new int[rows][];
        
        for (int i = 0; i < rows; i++)
        {
            map[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                map[i][j] = 0;
            }
        }
        
        // Trail definition
        map[1][0] = 1;
        map[2][0] = 1;
        map[2][1] = 1;
        map[2][2] = 1;
        map[2][3] = 1;
        map[2][4] = 1;
        map[3][4] = 1;

        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Draw()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                float offsetX = offsetMapStart.x + (x * 0.35f);
                float offsetY = offsetMapStart.y + (y * 0.35f);
                
                if(map[y][x] == 0) 
                    Instantiate(Resources.Load("White"), new Vector2(offsetX, offsetY), Quaternion.identity);
                else if(map[y][x] == 1)
                    Instantiate(Resources.Load("Black"), new Vector2(offsetX, offsetY), Quaternion.identity);
            }
        }
    }
}
