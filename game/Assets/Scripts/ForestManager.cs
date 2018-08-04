using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

using Elements;

public class ForestManager : MonoBehaviour {

    ForestElement[,] _map;
    
    private Dictionary<string, FireElement> _fires;
    private Dictionary<string, WaterElement> _waterTargets;

    public int MaxFires;

    public int FireDecay;
    
    [Range(0,100)]
    public int SpreadOdds;

    public int Width;
    public int Height;

    private System.Random _random = new System.Random(Time.time.GetHashCode());

    void Start() {
        _fires = new Dictionary<string, FireElement>(2);
        GenerateForrest();
    }

    void Update() {

        foreach(var fire in _fires){
            CalculateFireSpread(fire.Key);
        }

        foreach(var water in _waterTargets){
            if(water.Value.Update()){
                DecayFire(water.Value.X, water.Value.Y);
            }
        }
    }

    void GenerateForrest(){
        for(var x = 0; x < Width; x++){
            for(var y = 0; y < Height; y++){
                var element = new ForestElement();
                element.Healt = 1.0f;
                element.Type = ForestElementType.TREE;

                _map[x, y] = element;
            }
        }
    }

    void DecayFire(int x, int y)
    {
        string targetFire = "fire-" + CreateKey(x, y);
        if(_fires.ContainsKey(targetFire))
        {
            var fire = _fires[targetFire];
            fire.Healt -= 0.1f;

            if(fire.Healt < 0.1f){
                fire.Healt = 0.0f;
                Debug.Log("Fire " + targetFire + " has decayed");
            }            
        }
    }

    void AddFire(int x, int y, float intensity = 1.0f)
    {
        var keyName = "fire-" + CreateKey(x, y);
        if( !_fires.ContainsKey(keyName) )
        {
            _fires.Add( keyName, new FireElement{
                X = x,
                Y = y,
                Healt = intensity
            });
        }
        else{
            Debug.LogWarning(String.Format("Fire already exists at position {0}, {1}", x, y));
        }
    }

    string CreateKey(int x, int y){
        return x.ToString() + "," + y.ToString();
    }

    Dictionary<Vector2Int, ForestElement> GetSpreadLocations(int x, int y){
        var targets = new Dictionary<Vector2Int, ForestElement>(9);

        // Left Top
        if(x - 1 > 0 && y - 1 > 0 && _map[x - 1, y - 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x - 1, y - 1), _map[x - 1, y - 1]);
        }

        // Middle Top
        if(y - 1 > 0 && _map[x, y - 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x, y - 1), _map[x, y - 1]);
        }

        // Right Top
        if(x + 1 > 0 && y - 1 > 0 && _map[x + 1, y - 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x + 1, y - 1), _map[x + 1, y - 1]);
        }

        // Left Middle
        if(x - 1 > 0 && _map[x - 1, y].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x - 1, y), _map[x - 1, y]);
        }

        // Right Middle
        if(x + 1 > 0 && _map[x - 1, y].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x + 1, y), _map[x + 1, y]);
        }

        // Left Bottom
        if(x - 1 > 0 && y + 1 > 0 && _map[x - 1, y + 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x - 1, y + 1), _map[x - 1, y + 1]);
        }

        // Middle Bottom
        if(y - 1 > 0 && _map[x, y + 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x, y + 1), _map[x, y + 1]);
        }

        // Right Bottom
        if(x + 1 > 0 && y + 1 > 0 && _map[x + 1, y + 1].Type == ForestElementType.TREE){
            targets.Add(new Vector2Int(x + 1, y + 1), _map[x + 1, y + 1]);
        }

        return targets;
    }

    void CalculateFireSpread(string fireName)
    {
        var fire = _fires[fireName];

        // Has the tree burned down?
        if(_map[fire.X, fire.Y].Healt <= 0.1f){
            _map[fire.X, fire.Y].Healt = 0.0f;
            _map[fire.X, fire.Y].Type = ForestElementType.BURNEDTREE;
        }

        // Spread fire
        var targetTrees = GetSpreadLocations(fire.X, fire.Y);

        foreach(var tree in targetTrees){
            if(_random.Next(0,100) < SpreadOdds){
                AddFire(tree.Key.x, tree.Key.y);
            }
        }

        // Decay fire
        if(_map[fire.X, fire.Y].Healt == 0.0f){
            DecayFire(fire.X, fire.Y);
        }

        if(fire.Healt == 0.0f){
            _fires.Remove(fireName);
            Debug.Log("Removed fire " + fireName);
        }
    }
}
