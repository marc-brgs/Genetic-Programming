using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaFe : MonoBehaviour
{
    class Node
    {
        public Node parent;
        public List<Node> children;
        public string value;
        
        public Node(string value)
        {
            this.value = value;
            this.children = new List<Node> {};
        }
    }
    
    private int[,] map;
    private int startX, startY, endX, endY;

    public SantaFe(int[,] map, int startX, int startY, int endX, int endY) {
        this.map = map;
        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
    }

    public List<Tuple<int, int>> Solve() {
        List<Tuple<int, int>> path = new List<Tuple<int, int>>();

        int populationSize = 100;
        int generation = 0;
        int maxGenerations = 1000;
        float mutationRate = 0.1f;
        float fitnessThreshold = 0.999f;

        List<Individual> population = InitializePopulation(populationSize);

        while (generation < maxGenerations) {
            List<Individual> newPopulation = new List<Individual>();

            // Elitism: select the best individuals and add them to the new population
            population.Sort((a, b) => b.fitness.CompareTo(a.fitness));
            int elitism = (int)(populationSize * 0.1f);
            for (int i = 0; i < elitism; i++) {
                newPopulation.Add(population[i]);
                if (population[i].fitness >= fitnessThreshold) {
                    path = population[i].path;
                    return path;
                }
            }

            // Crossover: generate offspring by combining the genes of two parents
            for (int i = 0; i < populationSize - elitism; i++) {
                Individual parent1 = SelectParent(population);
                Individual parent2 = SelectParent(population);
                Individual offspring = Crossover(parent1, parent2);
                newPopulation.Add(offspring);
            }

            // Mutate: randomly modify the genes of some individuals
            for (int i = elitism; i < populationSize; i++) {
                if (UnityEngine.Random.value < mutationRate) {
                    Mutate(newPopulation[i]);
                }
            }

            population = newPopulation;
            generation++;
        }

        // If no solution is found, return an empty path
        return path;
    }

    private List<Individual> InitializePopulation(int populationSize) {
        List<Individual> population = new List<Individual>();
        for (int i = 0; i < populationSize; i++) {
            population.Add(GenerateIndividual());
        }
        return population;
    }

    private Individual GenerateIndividual() {
        List<Tuple<int, int>> path = new List<Tuple<int, int>>();
        int x = startX;
        int y = startY;
        while (x != endX || y != endY) {
            path.Add(new Tuple<int, int>(x, y));
            int nextX = x;
            int nextY = y;
            int direction = UnityEngine.Random.Range(0, 4);
            switch (direction) {
                case 0: nextY++; break; // move up
                case 1: nextY--; break; // move down
                case 2: nextX++; break; // move right
                case 3: nextX--; break; // move left
            }
            if (nextX >= 0 && nextX < map.GetLength(0) && nextY >= 0 && nextY < map.GetLength(1) && map[nextX, nextY] != 1) {
                x = nextX;
                y = nextY;
            }
        }
        path.Add(new Tuple<int, int>(endX, endY));
        float fitness = CalculateFitness(path);
        return new Individual(path, fitness);
    }

    private float CalculateFitness(List<Tuple<int, int>> path) {
        float distance = 0;
        for (int i = 1; i < path.Count; i++) {
            int dx = Math.Abs(path[i].Item1 - path[i - 1].Item1);
            int dy = Math.Abs(path[i].Item2 - path[i - 1].Item2);
            distance += dx + dy;
        }
        return 1.0f / (distance + 1.0f);
    }

    private Individual SelectParent(List<Individual> population) {
        float totalFitness = 0;
        foreach (Individual individual in population) {
            totalFitness += individual.fitness;
        }
        float randomFitness = UnityEngine.Random.Range(0, totalFitness);
        float cumulativeFitness = 0;
        foreach (Individual individual in population) {
            cumulativeFitness += individual.fitness;
            if (cumulativeFitness >= randomFitness) {
                return individual;
            }
        }
        return population[population.Count - 1];
    }

    private Individual Crossover(Individual parent1, Individual parent2) {
        int splitIndex = UnityEngine.Random.Range(0, Math.Min(parent1.path.Count, parent2.path.Count));
        List<Tuple<int, int>> path = new List<Tuple<int, int>>();
        for (int i = 0; i < splitIndex; i++) {
            path.Add(parent1.path[i]);
        }
        for (int i = splitIndex; i < parent2.path.Count; i++) {
            path.Add(parent2.path[i]);
        }
        float fitness = CalculateFitness(path);
        return new Individual(path, fitness);
    }

    private void Mutate(Individual individual) {
        int index1 = UnityEngine.Random.Range(0, individual.path.Count);
        int index2 = UnityEngine.Random.Range(0, individual.path.Count);
        Tuple<int, int> temp = individual.path[index1];
        individual.path[index1] = individual.path[index2];
        individual.path[index2] = temp;
        individual.fitness = CalculateFitness(individual.path);
    }

    private class Individual {
        public List<Tuple<int, int>> path;
        public float fitness;

        public Individual(List<Tuple<int, int>> path, float fitness) {
            this.path = path;
            this.fitness = fitness;
        }
    }
}
