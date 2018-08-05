using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

using Elements;

public class ForestManager : MonoBehaviour {

    private GridElement[,] _map;

    private Dictionary<string, Vector2Int> _fire;
    private List<Vector2Int> _removeQueue;

    public static ForestManager instance = null; 

    public int Padding = 10;

    public float GridSize = 12.0f;

    public int Wiggle = 1;
    
    public GridElement GridElement;

    public float SpreadFrequency = 3.0f;
	private float _currentTime = 0;

    [Range(0,100)]
    public int SpreadOdds = 80;

    [Range(0,100)]
    public int ForestDensity = 80;

    public int Width = 15;
    public int Height = 15;

    private System.Random _random;

    void Awake() {
        _map = new GridElement[Width, Height];
        _fire = new Dictionary<string, Vector2Int>(10);
        _removeQueue = new List<Vector2Int>(10);

        _random = new System.Random(Time.time.GetHashCode());

        //Check if instance already exists
        if (instance == null){
            
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this) {
            
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        GenerateForrest();
        CreateFire();
    }

    void Start() {
    }

    void Update() {
        _currentTime += Time.deltaTime;

        if(_currentTime >= SpreadFrequency)
        {
            _currentTime = 0.0f;

            foreach(var fire in _fire)
            {
                // Spread fire
                var targetTrees = GetSpreadLocations(fire.Value.x, fire.Value.y);

                foreach(var newFire in targetTrees){
                    if(_random.Next(0,100) < SpreadOdds){
                        CreateFire(newFire.GridPosition.x, newFire.GridPosition.y);
                        var fireName = CreateFireName(newFire.GridPosition.x, newFire.GridPosition.y);

                        Debug.Log("Fire(" + fire.Value.x + ", " + fire.Value.y + ") spreads to " + newFire.GridPosition.x + ", " + newFire.GridPosition.y);
                    }
                }
            }

            ProcessRemoveQueue();
        }
    }

    string CreateFireName(int x, int y)
    {
        return "fire-" + x + "," + y;
    }

    bool RandomForestElement()
    {
        if(_random.Next(0, 100) < ForestDensity){
            return true;
        }
        else{
            return false;
        }
    }

    List<GridElement> GetSpreadLocations(int x, int y){
        var targets = new List<GridElement>(9);

        // Left Top
        if(x - 1 > -1 && y - 1 > -1 && _map[x - 1, y - 1]){
            if(_map[x - 1, y - 1] != null) targets.Add(_map[x - 1, y - 1]);
        }

        // Middle Top
        if(y - 1 > -1 && _map[x, y - 1]){
            if(_map[x, y - 1] != null) targets.Add(_map[x, y - 1]);
        }

        // Right Top
        if(x + 1 < Width && y - 1 > -1 && _map[x + 1, y - 1]){
            if(_map[x + 1, y - 1] != null) targets.Add(_map[x + 1, y - 1]);
        }

        // Left Middle
        if(x - 1 > -1 && _map[x - 1, y]){
            if(_map[x - 1, y] != null) targets.Add(_map[x - 1, y]);
        }

        // Right Middle
        if(x + 1 < Width && _map[x - 1, y]){
            if(_map[x + 1, y] != null) targets.Add(_map[x + 1, y]);
        }

        // Left Bottom
        if(x - 1 > -1 && y + 1 < Height && _map[x - 1, y + 1]){
            if(_map[x - 1, y + 1] != null) targets.Add(_map[x - 1, y + 1]);
        }

        // Middle Bottom
        if(y + 1 < Height && _map[x, y + 1]){
            if(_map[x, y + 1] != null) targets.Add(_map[x, y + 1]);
        }

        // Right Bottom
        if(x + 1 < Width && y + 1 < Height && _map[x + 1, y + 1]){
            if(_map[x + 1, y + 1] != null) targets.Add(_map[x + 1, y + 1]);
        }

        return targets;
    }

    public void RemoveFire(int x, int y)
    {
        _removeQueue.Add(new Vector2Int(x, y));
    }

    public void ProcessRemoveQueue()
    {
        foreach( var fire in _removeQueue )
        {
            if(fire.x >= 0 && fire.x < Width && fire.y >= 0 && fire.y < Height )
            {
                var fireName = CreateFireName(fire.x, fire.y);
                if(_fire.ContainsKey(fireName))
                {
                    _fire.Remove(fireName);
                }
            }
        }

        _removeQueue = new List<Vector2Int>(10);
    }

    private void CreateFire(int x = -1, int y = -1)
    {
        if(x == -1 && y == -1)
        {
            x = (int) Math.Round( Width / 2.0f, 0);
            y = (int) Math.Round( Height / 2.0f, 0);
        }

        var fireName = CreateFireName(x, y);

        if(!_fire.ContainsKey(fireName))
        {
            if(_map[x, y] == null){
                CreateTree(x, y);
            }

            _map[x, y].gameObject.AddComponent(typeof(FireElement));
            _fire.Add(CreateFireName(x, y), new Vector2Int(x, y));

            Debug.Log("Created fire at " + x + ", " + y);
        }
    }

    private void CreateTree(int x, int y)
    {
        var centerX = (int) Math.Round(GridSize / 2, 0);
        var centerY = (int) Math.Round(GridSize / 2, 0);

        var gap = (x == 0 && y == 0 ) ? 0 : Padding;

        // Wigle
        var wiggleX = _random.Next(-Wiggle, Wiggle);
        var wiggleY = _random.Next(-Wiggle, Wiggle);

        Vector3 pos = new Vector3(x * centerX + gap + wiggleX, 0, y * centerY + gap + wiggleY );

        var newTree = Instantiate(GridElement, pos, Quaternion.identity);
        newTree.GridPosition = new Vector2Int(x, y);
        newTree.Health = 1.0f;
        _map[x, y] = newTree;
    }

    void GenerateForrest()
    {
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                if(RandomForestElement())
                {
                    CreateTree(x, y);
                }
            }
        }
    }
}
