using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[][] map;
    private int rows = 32;
    private int cols = 32;
    public Transform cam;
    public Transform visualAnt;
    
    public Population population;
    public int populationSize;
    public int maxGenerations;
    public float mutationRate;
    public float crossoverRate;
    public float tournamentSelectionSize;

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
        
        // Initialisation de la grille de jeu
        GenerateGrid(); // map

        /*// Initialisation de la population
        population = new Population();
        for (int i = 0; i < populationSize; i++)
        {
            Individual individual = new Individual();
            individual.behaviorTree = CreateRandomBehaviorTree();
            population.individuals.Add(individual);
        }

        // Boucle principale de l'algorithme génétique
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            // Evaluation de la population actuelle
            EvaluatePopulation();
            
            List<Individual> parents = SelectParents(); // Sélection des parents pour la reproduction
            List<Individual> newGeneration = Reproduce(parents); // Création d'une nouvelle génération d'individus
            
            ApplyMutation(newGeneration); // Application de la mutation
            
            population.individuals = newGeneration; // Remplacement de la population actuelle par la nouvelle génération
        }

        // Sélection du meilleur individu
        Individual bestIndividual = GetBestIndividual();

        // Affichage du comportement du meilleur individu dans le labyrinthe
        bestIndividual.behaviorTree.Execute(maze);*/
        
        Ant a = new Ant(0, 0);
        a.direction = 1;
        DisplayAnt(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayAnt(Ant ant)
    {
        visualAnt.position = new Vector2(ant.posX, ant.posY);
        // rotation
        if (ant.direction == 1) // top
        {
            visualAnt.eulerAngles = new Vector3(0, 0, -270);
        }
        else if (ant.direction == 2) // right
        {
            visualAnt.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (ant.direction == 3) // bottom
        {
            visualAnt.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (ant.direction == 4) // left
        {
            visualAnt.eulerAngles = new Vector3(0, 0, -180);
        }
    }
    
    void GenerateGrid()
    {
        map[0][31] = 1;
        map[1][31] = 1;
        map[2][31] = 1;
        map[3][31] = 1;
        map[3][30] = 1;
        map[3][29] = 1;
        map[3][28] = 1;
        map[3][27] = 1;
        map[3][26] = 1;
        
        map[4][26] = 1;
        map[5][26] = 1;
        map[6][26] = 1;
        map[7][26] = 2;
        map[8][26] = 1;
        map[9][26] = 1;
        map[10][26] = 1;
        map[11][26] = 1;
        map[12][26] = 1;

        map[12][25] = 1;
        map[12][24] = 1;
        map[12][23] = 1;
        map[12][22] = 1;
        map[12][21] = 1;
        map[12][20] = 1;
        map[12][19] = 1;
        map[12][18] = 1;
        map[12][17] = 1;
        map[12][16] = 1;
        map[12][15] = 1;
        map[12][14] = 1;
        map[12][13] = 1;
        map[12][12] = 1;
        map[12][11] = 1;
        map[12][10] = 1;
        map[12][9] = 1;
        map[12][8] = 1;
        map[12][7] = 1;
        
        map[11][7] = 1;
        map[10][7] = 1;
        map[9][7] = 1;
        map[8][7] = 1;
        map[7][7] = 1;
        map[6][7] = 1;
        map[5][7] = 1;
        map[4][7] = 1;
        map[3][7] = 1;
        map[2][7] = 1;
        map[1][7] = 1;
        
        map[1][6] = 1;
        map[1][5] = 1;
        map[1][4] = 1;
        map[1][3] = 1;
        map[1][2] = 1;
        map[1][1] = 1;
        
        map[1][1] = 1;
        map[2][1] = 1;
        map[3][1] = 1;
        map[4][1] = 1;
        map[5][1] = 1;
        map[6][1] = 1;
        map[7][1] = 1;
        
        map[7][2] = 1;
        map[7][3] = 1;
        map[7][4] = 1;
        
        map[8][4] = 1;
        map[9][4] = 1;
        map[10][4] = 1;
        map[11][4] = 1;
        map[12][4] = 1;
        map[13][4] = 1;
        map[14][4] = 1;
        map[15][4] = 1;
        map[16][4] = 1;
        
        map[16][4] = 1;
        map[16][5] = 1;
        map[16][6] = 1;
        map[16][7] = 1;
        map[16][8] = 1;
        map[16][9] = 1;
        map[16][10] = 1;
        map[16][11] = 1;
        map[16][12] = 1;
        map[16][13] = 1;
        map[16][14] = 1;
        map[16][15] = 1;
        map[16][16] = 1;
        
        map[17][16] = 1;
        map[18][16] = 1;
        map[19][16] = 1;
        map[20][16] = 1;
        
        map[20][17] = 1;
        map[20][18] = 1;
        map[20][19] = 1;
        map[20][20] = 1;
        map[20][21] = 1;
        map[20][22] = 1;
        map[20][23] = 1;
        map[20][24] = 1;
        map[20][25] = 1;
        map[20][26] = 1;
        
        map[21][26] = 1;
        map[22][26] = 1;
        map[23][26] = 1;
        map[24][26] = 1;
        
        map[24][27] = 1;
        map[24][28] = 1;
        map[24][29] = 1;
        
        map[25][29] = 1;
        map[26][29] = 1;
        map[27][29] = 1;
        map[28][29] = 1;
        map[29][29] = 1;
        
        map[29][28] = 1;
        map[29][27] = 1;
        map[29][26] = 1;
        map[29][25] = 1;
        map[29][24] = 1;
        map[29][23] = 1;
        map[29][22] = 1;
        map[29][21] = 1;
        map[29][20] = 1;
        map[29][19] = 1;
        map[29][18] = 1;
        map[29][17] = 1;
        
        map[28][17] = 1;
        map[27][17] = 1;
        map[26][17] = 1;
        map[25][17] = 1;
        map[24][17] = 1;
        map[23][17] = 1;

        map[23][16] = 1;
        map[23][15] = 1;
        map[23][14] = 1;
        map[23][13] = 1;
        
        map[24][13] = 1;
        map[25][13] = 1;
        map[26][13] = 1;
        map[27][13] = 1;
        
        map[27][13] = 1;
        map[27][12] = 1;
        map[27][11] = 1;
        map[27][10] = 1;
        map[27][9] = 1;
        
        map[26][9] = 1;
        map[25][9] = 1;
        map[24][9] = 1;
        map[23][9] = 1;
        
        map[23][8] = 1;
        
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (map[x][y] == 0)
                    Instantiate(Resources.Load("White"), new Vector2(x, y), Quaternion.identity);
                else if (map[x][y] == 1) // correct path
                    Instantiate(Resources.Load("Green"), new Vector2(x, y), Quaternion.identity);
                else if (map[x][y] == 2) // food
                    Instantiate(Resources.Load("Yellow"), new Vector2(x, y), Quaternion.identity);
            }
        }
        
        // Center camera to grid
        cam.transform.position = new Vector3((float)cols / 2 - 0.5f, (float)rows / 2 - 0.5f, -10);
    }

    // Création d'un arbre de comportement aléatoire
    private BehaviorTree CreateRandomBehaviorTree()
    {
        // TODO : implémenter la création d'un arbre de comportement aléatoire
        return null;
    }

    // Evaluation de la population actuelle
    private void EvaluatePopulation()
    {
        foreach (Individual individual in population.individuals)
        {
            // Exécution du labyrinthe avec le comportement de l'individu
            float fitness = RunMazeWithBehaviorTree(map, individual.behaviorTree);

            // Stockage de la performance de l'individu
            individual.fitness = fitness;
        }
    }
    
    // Sélection des parents pour la reproduction
    private List<Individual> SelectParents()
    {
        List<Individual> parents = new List<Individual>();

        // TODO : implémenter la sélection des parents à l'aide d'un tournoi de sélection

        return parents;
    }

    // Création d'une nouvelle génération d'individus
    private List<Individual> Reproduce(List<Individual> parents)
    {
        List<Individual> newGeneration = new List<Individual>();

        // TODO : implémenter la reproduction des parents pour créer une nouvelle génération d'individus

        return newGeneration;
    }

    // Application de la mutation
    private void ApplyMutation(List<Individual> newGeneration)
    {
        // TODO : implémenter l'application de la mutation sur la nouvelle génération
    }

    private Individual GetBestIndividual()
    {
        Individual bestIndividual = population.individuals[0];

        foreach (Individual individual in population.individuals)
        {
            if (individual.fitness > bestIndividual.fitness)
            {
                bestIndividual = individual;
            }
        }

        return bestIndividual;
    }

    // Exécution du labyrinthe avec un arbre de comportement donné
    private float RunMazeWithBehaviorTree(int[][] map, BehaviorTree behaviorTree)
    {
        if (behaviorTree.leftChild != null) RunMazeWithBehaviorTree(map, behaviorTree.leftChild);

        
        
        if (behaviorTree.rightChild != null) RunMazeWithBehaviorTree(map, behaviorTree.rightChild);
        
        return 0f; // compter les passages sur la nourriture
    }
}

public class BehaviorTree
{
    public BehaviorTree parent;
    public BehaviorTree leftChild;
    public BehaviorTree rightChild;
    public string value;
        
    public BehaviorTree(string value)
    {
        this.value = value;
    }

    public void Execute()
    {
        
    }
}

public class Individual
{
    public BehaviorTree behaviorTree;
    public float fitness;
}

public class Population
{
    public List<Individual> individuals;
}