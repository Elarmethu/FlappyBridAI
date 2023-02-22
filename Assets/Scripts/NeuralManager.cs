using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralManager : MonoBehaviour
{
    public static NeuralManager instance { get; private set; }

    [Header("Main Dependecies")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("Evolvement Variables")]
    [SerializeField] private int _populationSize;
    [SerializeField] private int _mutationChance;
    [SerializeField] private NeuralNetwork.NeuralNetworkType _networkType;

    public void BirdCrashed()
    {
        _deadCount += 1;
        if (_deadCount == _populationSize)
            EndGeneration();
    }

    [Header("Evolvement Infromation")]
    [SerializeField] private float _maxFitness;
    [SerializeField] private int _generation;
    [SerializeField] private int _deadCount;

    private List<Player> _lastPopulation = new List<Player>();
    private List<Player> _birdList = new List<Player>();
    private int[] _layers;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _layers = new int[4] { 4, 8, 8, 1 };
        InstantiatePopulation();
    }

    private void Update()
    {
        GameManager.instance.InformationNeural(_generation, _maxFitness, _birdList[0].fitness);
    }

    private void FixedUpdate()
    {
        _birdList.Sort(SortByFitness);

        if (_birdList.Count > 0)
            _maxFitness = Math.Max(_birdList[0].fitness, _maxFitness);
    }

    private void EndGeneration()
    {
        GameManager.instance.ResetGame();
        StartGeneration();
    }

    private void StartGeneration()
    {
        _generation += 1;
        _deadCount = 0;
        _lastPopulation = _birdList;
        _birdList = new List<Player>();

        InstantiatePopulation();
    }

    private void InstatiateBird()
    {
        Player player = Instantiate(_playerPrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<Player>();
        _birdList.Add(player);
    }

    private int SortByFitness(Player a, Player b)
    {
        return -(a.fitness.CompareTo(b.fitness));
    }

    private void InstantiatePopulation()
    {
        _birdList = new List<Player>();

        for (int i = 0; i < _populationSize; i++)
        {
            InstatiateBird();

            if (_generation == 0)
                _birdList[i].SetBrain(new NeuralNetwork(_layers), _networkType);
            else
                MutatelastPopulation(i);

            Debug.Log(_networkType.ToString());
        }
    }

    private void MutatelastPopulation(int i)
    {
        int top = 3;

        if (i < top)
        {
            NeuralNetwork copy = _lastPopulation[i].brain;
            _birdList[i].SetBrain(copy, _networkType);
        }
        else if (i < _populationSize * 0.25f)
        {
            NeuralNetwork copy = _lastPopulation[i].brain;
            copy.Mutate(_mutationChance, 1);
            _birdList[i].SetBrain(copy, _networkType);
        }
        else if (i < _populationSize * 0.50f)
        {
            NeuralNetwork copy = _lastPopulation[i].brain;
            copy.Mutate(_mutationChance, 1);
            _birdList[i].SetBrain(copy, _networkType);
        }
        else if (i < _populationSize * 0.75f)
        {
            NeuralNetwork copy = _lastPopulation[i].brain;
            copy.Mutate(Mathf.CeilToInt(_mutationChance * 0.5f), 1);
            _birdList[i].SetBrain(copy, _networkType);
        }
        else if (i < _populationSize * 1.00f)
        {
            NeuralNetwork copy = _lastPopulation[i].brain;
            copy.Mutate(Mathf.CeilToInt(_mutationChance * 2.0f), 1);
            _birdList[i].SetBrain(copy, _networkType);
        }

    }
}
