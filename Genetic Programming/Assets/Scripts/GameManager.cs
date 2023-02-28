using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private int animated = 0;
    private Ant antToAnimate;
    private Ant ant2;
    private Individual individualToAnimate;
    private List<string> animationsBuffer;

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

        Individual test = new Individual();
        test.behaviorTree = CreateRandomTree();
        Debug.Log(test.behaviorTree.toString());
        
        Debug.Log("Init");
        
        // Initialisation de la population
        population = new Population();
        for (int i = 0; i < populationSize; i++)
        {
            Individual individual = new Individual();
            individual.behaviorTree = CreateRandomTree();
            population.individuals.Add(individual);
        }
        
        Debug.Log("Gen loop");
        // Boucle principale de l'algorithme génétique
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            // Evaluation de la population actuelle
            EvaluatePopulation();
            
            List<Individual> parents = SelectParents(); // Sélection des parents pour la reproduction
            List<Individual> newGeneration = Crossover(parents); // Création d'une nouvelle génération d'individus
            
            //ApplyMutation(newGeneration); // Application de la mutation
            
            population.individuals = newGeneration; // Remplacement de la population actuelle par la nouvelle génération
            
            Debug.Log("Gen : " + generation);
        }

        // Sélection du meilleur individu
        Individual bestIndividual = GetBestIndividual(population.individuals);
        Debug.Log(bestIndividual.behaviorTree.toString());
        
        // Affichage du comportement du meilleur individu
        //bestIndividual.behaviorTree.Execute(map, ant, new List<string>());
        
        Ant a = new Ant(0, 31);
        DisplayAnt(a);

        individualToAnimate = bestIndividual;
        individualToAnimate.behaviorTree = CreateSantaFeTree();
        Debug.Log(individualToAnimate.behaviorTree.toString());
        antToAnimate = a;
        ant2 = new Ant(0, 31);
        animationsBuffer = new List<string>();
    }

    private void FixedUpdate()
    {
        if (animated < 100)
        {
            animationsBuffer.AddRange(individualToAnimate.behaviorTree.Execute(map, antToAnimate, new List<string>()));
            animated++;
        }

        if (animationsBuffer.Count > 0)
        {
            string action = animationsBuffer[0];
            
            switch (action)
            {
                case "TURN_LEFT":
                    ant2.direction = mod(ant2.direction - 1, 4);
                    break;
                case "TURN_RIGHT":
                    ant2.direction = mod(ant2.direction + 1, 4);
                    break;
                case "MOVE_FORWARD":
                    if (ant2.direction == 0 && ant2.posY+1 < 32) // top
                    {
                        ant2.posY += 1;
                    }
                    if (ant2.direction == 1 && ant2.posX+1 < 32) // right
                    {
                        ant2.posX += 1;
                    }
                    if (ant2.direction == 2 && ant2.posY-1 >= 0) // bottom
                    {
                        ant2.posY -= 1;
                    }
                    if (ant2.direction == 3 && ant2.posX-1 >= 0) // left
                    {
                        ant2.posX -= 1;
                    }
                    break;
            }
            animationsBuffer.RemoveAt(0);
            DisplayAnt(ant2);
        }
    }

    void DisplayAnt(Ant ant)
    {
        visualAnt.position = new Vector2(ant.posX, ant.posY);
        // rotation
        if (ant.direction == 0) // top
        {
            visualAnt.eulerAngles = new Vector3(0, 0, -270);
        }
        else if (ant.direction == 1) // right
        {
            visualAnt.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (ant.direction == 2) // bottom
        {
            visualAnt.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (ant.direction == 3) // left
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
        map[12][21] = 2;
        map[12][20] = 1;
        map[12][19] = 1;
        map[12][18] = 1;
        map[12][17] = 1;
        map[12][16] = 2;
        map[12][15] = 2;
        map[12][14] = 1;
        map[12][13] = 1;
        map[12][12] = 1;
        map[12][11] = 1;
        map[12][10] = 1;
        map[12][9] = 1;
        map[12][8] = 1;
        map[12][7] = 2;
        
        map[11][7] = 1;
        map[10][7] = 1;
        map[9][7] = 1;
        map[8][7] = 1;
        map[7][7] = 1;
        map[6][7] = 2;
        map[5][7] = 2;
        map[4][7] = 1;
        map[3][7] = 1;
        map[2][7] = 2;
        map[1][7] = 2;
        
        map[1][6] = 1;
        map[1][5] = 1;
        map[1][4] = 1;
        map[1][3] = 1;
        map[1][2] = 2;
        map[1][1] = 2;
        
        map[1][1] = 1;
        map[2][1] = 1;
        map[3][1] = 1;
        map[4][1] = 1;
        map[5][1] = 1;
        map[6][1] = 2;
        map[7][1] = 2;
        
        map[7][2] = 1;
        map[7][3] = 1;
        map[7][4] = 2;
        
        map[8][4] = 1;
        map[9][4] = 1;
        map[10][4] = 1;
        map[11][4] = 1;
        map[12][4] = 1;
        map[13][4] = 1;
        map[14][4] = 1;
        map[15][4] = 2;
        map[16][4] = 2;
        
        map[16][5] = 1;
        map[16][6] = 1;
        map[16][7] = 1;
        map[16][8] = 2;
        map[16][9] = 2;
        map[16][10] = 1;
        map[16][11] = 1;
        map[16][12] = 1;
        map[16][13] = 1;
        map[16][14] = 2;
        map[16][15] = 2;
        map[16][16] = 2;
        
        map[17][16] = 1;
        map[18][16] = 2;
        map[19][16] = 2;
        map[20][16] = 2;
        
        map[20][17] = 1;
        map[20][18] = 1;
        map[20][19] = 2;
        map[20][20] = 2;
        map[20][21] = 1;
        map[20][22] = 1;
        map[20][23] = 1;
        map[20][24] = 1;
        map[20][25] = 2;
        map[20][26] = 2;
        
        map[21][26] = 1;
        map[22][26] = 1;
        map[23][26] = 2;
        map[24][26] = 2;
        
        map[24][27] = 1;
        map[24][28] = 1;
        map[24][29] = 2;
        
        map[25][29] = 1;
        map[26][29] = 1;
        map[27][29] = 1;
        map[28][29] = 2;
        map[29][29] = 2;
        
        map[29][28] = 1;
        map[29][27] = 1;
        map[29][26] = 2;
        map[29][25] = 1;
        map[29][24] = 2;
        map[29][23] = 2;
        map[29][22] = 1;
        map[29][21] = 2;
        map[29][20] = 2;
        map[29][19] = 1;
        map[29][18] = 2;
        map[29][17] = 2;
        
        map[28][17] = 1;
        map[27][17] = 1;
        map[26][17] = 1;
        map[25][17] = 2;
        map[24][17] = 2;
        map[23][17] = 2;

        map[23][16] = 1;
        map[23][15] = 2;
        map[23][14] = 2;
        map[23][13] = 2;
        
        map[24][13] = 1;
        map[25][13] = 2;
        map[26][13] = 2;
        map[27][13] = 2;
        
        map[27][12] = 1;
        map[27][11] = 2;
        map[27][10] = 2;
        map[27][9] = 2;
        
        map[26][9] = 1;
        map[25][9] = 2;
        map[24][9] = 2;
        map[23][9] = 2;
        
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
    private TreeNode CreateRandomTree()
    {
        // Création de l'arbre de comportement aléatoire
        TreeNode root = new TreeNode("PROGN2");

        TreeNode seq1 = new TreeNode("PROGN2");
        seq1.children.Add(new TreeNode("IS_FOOD"));
        seq1.children.Add(new TreeNode("MOVE_FORWARD"));
        seq1.children[0].children.Add(new TreeNode("MOVE_FORWARD"));
        seq1.children[0].children.Add(new TreeNode("TURN_RIGHT"));

        TreeNode seq2 = new TreeNode("IS_FOOD");
        seq2.children.Add(new TreeNode("TURN_LEFT"));
        seq2.children.Add(new TreeNode("MOVE_FORWARD"));

        root.children.Add(seq1);
        root.children.Add(seq2);
        
        return root;
    }

    private TreeNode CreateSantaFeTree()
    {
        TreeNode n1 = new TreeNode("IS_FOOD");
        TreeNode n2 = new TreeNode("MOVE_FORWARD");
        TreeNode n3 = new TreeNode("PROGN3");
        TreeNode n4 = new TreeNode("TURN_LEFT");
        TreeNode n5 = new TreeNode("PROGN2");
        TreeNode n6 = new TreeNode("IS_FOOD");
        TreeNode n7 = new TreeNode("MOVE_FORWARD");
        TreeNode n8 = new TreeNode("TURN_RIGHT");
        TreeNode n9 = new TreeNode("PROGN2");
        TreeNode n10 = new TreeNode("TURN_RIGHT");
        TreeNode n11 = new TreeNode("PROGN2");
        TreeNode n12 = new TreeNode("TURN_LEFT");
        TreeNode n13 = new TreeNode("TURN_RIGHT");
        TreeNode n14 = new TreeNode("PROGN2");
        TreeNode n15 = new TreeNode("IS_FOOD");
        TreeNode n16 = new TreeNode("MOVE_FORWARD");
        TreeNode n17 = new TreeNode("TURN_LEFT");
        TreeNode n18 = new TreeNode("MOVE_FORWARD");

        n1.children.Add(n2);
        n1.children.Add(n3);
        
        n3.children.Add(n4);
        n3.children.Add(n5);
        n3.children.Add(n14);
        
        n5.children.Add(n6);
        n5.children.Add(n9);
        
        n6.children.Add(n7);
        n6.children.Add(n8);
        
        n9.children.Add(n10);
        n9.children.Add(n11);
        
        n11.children.Add(n12);
        n11.children.Add(n13);
        
        n14.children.Add(n15);
        n14.children.Add(n18);
        
        n15.children.Add(n16);
        n15.children.Add(n17);
        return n1;
    }

    TreeNode createEasyTree()
    {
        TreeNode n1 = new TreeNode("IS_FOOD");
        TreeNode n2 = new TreeNode("MOVE_FORWARD");
        TreeNode n3 = new TreeNode("TURN_LEFT");
        
        n1.children.Add(n2);
        n1.children.Add(n3);
        return n1;
    }
    
    // Evaluation de la population actuelle
    private void EvaluatePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Individual individual = population.individuals[i];
            float fitness = EvaluateIndividual(individual);
            individual.fitness = fitness;
        }
    }

    private float EvaluateIndividual(Individual individual)
    {
        // Reset de la fourmi
        Ant ant = new Ant(0, 31);

        // Reset de la map
        int[][] originalMap = map;
        
        float score = 0;
        
        
        for (int i = 0; i < 200; i++) // boucle de 200 pas maximum
        {
            List<string> actions = individual.behaviorTree.Execute(originalMap, ant, new List<string>());

            foreach (string action in actions)
            {
                if (action == "TURN_LEFT") // tourner à gauche
                {
                    ant.direction = mod(ant.direction - 1, 4);
                }
                else if (action == "TURN_RIGHT") // tourner à droite
                {
                    ant.direction = mod(ant.direction + 1, 4);
                }
                else if (action == "MOVE_FORWARD") // avancer tout droit
                {
                    if (ant.direction == 0 && ant.posY+1 < 32) // top
                    {
                        ant.posY += 1;
                    }
                    if (ant.direction == 1 && ant.posX+1 < 32) // right
                    {
                        ant.posX += 1;
                    }
                    if (ant.direction == 2 && ant.posY-1 >= 0) // bottom
                    {
                        ant.posY -= 1;
                    }
                    if (ant.direction == 3 && ant.posX-1 >= 0) // left
                    {
                        ant.posX -= 1;
                    }
                    
                    // Correction de la position de la fourmi
                    //ant.posX = mod(ant.posX, 32);
                    //ant.posY = mod(ant.posY, 32);

                    // Vérification de la case sur laquelle se trouve la fourmi
                    if (map[ant.posX][ant.posY] == 1) // sur le chemin correct
                    {
                        score++;
                    }
                    else if (map[ant.posX][ant.posY] == 2) // nourriture trouvée
                    {
                        score += 10;
                        map[ant.posX][ant.posY] = 0;
                    }
                }
            }
            
            DisplayAnt(ant);
        }
        
        return score;
    }

    // Sélection des parents pour la reproduction
    private List<Individual> SelectParents()
    {
        List<Individual> parents = new List<Individual>();
        
        for (int i = 0; i < populationSize; i++)
        {
            Individual parent1 = TournamentSelection();
            Individual parent2 = TournamentSelection();

            parents.Add(parent1);
            parents.Add(parent2);
        }

        return parents;
    }
    
    private Individual TournamentSelection()
    {
        List<Individual> tournamentPopulation = new List<Individual>();

        for (int i = 0; i < tournamentSelectionSize; i++)
        {
            int randomIndex = Random.Range(0, population.individuals.Count);
            tournamentPopulation.Add(population.individuals[randomIndex]);
        }

        return GetBestIndividual(tournamentPopulation);
    }
    
    // Création d'une nouvelle génération d'individus
    private List<Individual> Crossover(List<Individual> parents)
    {
        List<Individual> newGeneration = new List<Individual>();

        Individual parent1 = parents[0];
        Individual parent2 = parents[1];
        
        for (int i = 0; i < populationSize; i++)
        {
            Individual child = new Individual();
            
            TreeNode newRoot = CrossoverNodes(parent1.behaviorTree, parent2.behaviorTree);
            child.behaviorTree = newRoot;
            
            newGeneration.Add(child);
        }

        return newGeneration;
    }

    private TreeNode CrossoverNodes(TreeNode node1, TreeNode node2)
    {
        // On clone un des deux parents pour obtenir un nouvel arbre
        TreeNode newNode = new TreeNode(node1.action);
        
        // Si les deux parents ont des enfants, on choisit aléatoirement un point de croisement
        if (node1.children.Count > 0 && node2.children.Count > 0)
        {
            int crossoverPoint1 = UnityEngine.Random.Range(0, node1.children.Count);
            int crossoverPoint2 = UnityEngine.Random.Range(0, node2.children.Count);

            // On croise les sous-arbres correspondants aux deux points de croisement
            TreeNode child1 = CrossoverNodes(node1.children[crossoverPoint1], node2.children[crossoverPoint2]);
            TreeNode child2 = CrossoverNodes(node2.children[crossoverPoint2], node1.children[crossoverPoint1]);

            // On ajoute les nouveaux enfants au nouvel arbre
            newNode.children.Add(child1);
            newNode.children.Add(child2);

            // On met à jour les parents des enfants
            child1.parent = newNode;
            child2.parent = newNode;
        }
        // Si un des deux parents n'a pas d'enfant, on recopie directement les enfants du parent qui en a
        else if (node1.children.Count > 0)
        {
            foreach (TreeNode childNode in node1.children)
            {
                TreeNode child = CrossoverNodes(childNode, childNode);
                newNode.children.Add(child);
                child.parent = newNode;
            }
        }
        else if (node2.children.Count > 0)
        {
            foreach (TreeNode childNode in node2.children)
            {
                TreeNode child = CrossoverNodes(childNode, childNode);
                newNode.children.Add(child);
                child.parent = newNode;
            }
        }

        return newNode;
    }
    
    // Application de la mutation
    private void ApplyMutation(List<Individual> individuals)
    {
        for (int i = 0; i < individuals.Count; i++)
        {
            if (Random.value < mutationRate)
            {
                Mutate(individuals[i]);
            }
        }
    }

    private void Mutate(Individual individual)
    {
        // TODO : implémenter la méthode de mutation d'un arbre de comportement
    }
    
    private Individual GetBestIndividual(List<Individual> individuals)
    {
        Individual bestIndividual = individuals[0];

        foreach (Individual individual in individuals)
        {
            if (individual.fitness > bestIndividual.fitness)
            {
                bestIndividual = individual;
            }
        }

        return bestIndividual;
    }
    
    int mod(int x, int m) {
        return (x%m + m)%m;
    }
}

public class Population
{
    public List<Individual> individuals = new List<Individual>();
}

public class Individual
{
    public TreeNode behaviorTree;
    public float fitness;
}

public class TreeNode
{
    public TreeNode parent;
    public List<TreeNode> children = new List<TreeNode>();
    public string action;
    
    public TreeNode(string action)
    {
        this.action = action;
    }
    
    public string toString(string indent="", string final="", bool last=true)
    {
        final += indent;
        if (last)
        {
            final += "\\-";
            indent += "  ";
        }
        else
        {
            final += "|-";
            indent += "| ";
        }
        final += action + "\n";

        for (int i = 0; i < children.Count; i++)
            final = children[i].toString(indent, final, i == children.Count - 1);

        return final;
    }
    
    // Retourne l'action à effectuer en fonction de la map, la fourmi et l'arbre
    public List<string> Execute(int[][] map, Ant ant, List<string> actionList)
    {

        switch (action)
        {
            case "TURN_LEFT":
                actionList.Add(action);
                ant.direction = mod(ant.direction - 1, 4);
                break;
            case "TURN_RIGHT":
                actionList.Add(action);
                ant.direction = mod(ant.direction + 1, 4);
                break;
            case "MOVE_FORWARD":
                if (ant.direction == 0 && ant.posY+1 < 32) // top
                {
                    ant.posY += 1;
                    actionList.Add(action);
                }
                if (ant.direction == 1 && ant.posX+1 < 32) // right
                {
                    ant.posX += 1;
                    actionList.Add(action);
                }
                if (ant.direction == 2 && ant.posY-1 >= 0) // bottom
                {
                    ant.posY -= 1;
                    actionList.Add(action);
                }
                if (ant.direction == 3 && ant.posX-1 >= 0) // left
                {
                    ant.posX -= 1;
                    actionList.Add(action);
                }
                break;
            case "IS_FOOD":
            {
                TreeNode leftNode = children[0];
                TreeNode rightNode = children[1];

                int posX = ant.posX;
                int posY = ant.posY;
                
                if (ant.direction == 0 && posY < 31 && map[posX][posY + 1] == 2) // UP direction
                {
                    actionList = leftNode.Execute(map, ant, actionList);
                }
                if (ant.direction == 1 && posX < 31 && map[posX + 1][posY] == 2) // RIGHT direction
                {
                    actionList = leftNode.Execute(map, ant, actionList);
                }
                if (ant.direction == 2 && posY > 0 && map[posX][posY - 1] == 2) // BOTTOM direction
                {
                    actionList = leftNode.Execute(map, ant, actionList);
                }
                if (ant.direction == 3 && posX > 0 && map[posX - 1][posY] == 2) // LEFT direction
                {
                    actionList = leftNode.Execute(map, ant, actionList);
                }
                else
                {
                    actionList = rightNode.Execute(map, ant, actionList);
                }

                break;
            }
            case "PROGN2":
            {
                TreeNode leftNode = children[0];
                TreeNode rightNode = children[1];

                actionList = leftNode.Execute(map, ant, actionList);
                actionList = rightNode.Execute(map, ant, actionList);
                break;
            }
            case "PROGN3":
            {
                TreeNode leftNode = children[0];
                TreeNode rightNode = children[1];
                TreeNode bonusNode = children[2];
                
                actionList = leftNode.Execute(map, ant, actionList);
                actionList = rightNode.Execute(map, ant, actionList);
                actionList = bonusNode.Execute(map, ant, actionList);
                break;
            }
        }

        return actionList;
    }
    
    int mod(int x, int m) {
        return (x%m + m)%m;
    }
}
