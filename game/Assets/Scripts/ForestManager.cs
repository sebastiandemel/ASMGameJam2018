using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

using Elements;

public class ForestManager : MonoBehaviour {

    private ForestElement[,] _map;
    
    private Dictionary<string, FireElement> _fires;
    private Dictionary<string, WaterElement> _waterTargets;

    public static ForestManager instance = null; 

    public int MaxFires;

    public int FireDecay;

    public int Padding;
    
    public TreeObject TreeElement;

    [Range(0,100)]
    public int SpreadOdds;

    [Range(0,100)]
    public int ForestDensity;

    public int Width;
    public int Height;

    private System.Random _random;

    void Awake() {
        _map = new ForestElement[Width,Height];
        _fires = new Dictionary<string, FireElement>(2);
        _waterTargets = new Dictionary<string, WaterElement>();
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

        AddFire(10, 10);
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
    ForestElementType RandomForestElement()
    {
        if(_random.Next(0, 100) < ForestDensity){
            return ForestElementType.TREE;
        }
        else{
            return ForestElementType.EMPTY;
        }
    }

    void GenerateForrest(){
        var modelWidth = 12.0f;
        var modelHeight = 12.0f;
        var centerX = (int) Math.Round(modelWidth / 2, 0);
        var centerY = (int) Math.Round(modelHeight / 2, 0);

        for(var x = 0; x < Width; x++){
            for(var y = 0; y < Height; y++){
                var element = new ForestElement();
                var gap = (x == 0 && y == 0 ) ? 0 : Padding;

                element.Healt = 1.0f;
                element.Type = RandomForestElement();

                _map[x, y] = element;

                if(element.Type == ForestElementType.TREE){                    
                    // Wigle
                    var wiggleX = _random.Next(-1, 1);
                    var wiggleY = _random.Next(-1, 1);

                    Vector3 pos = new Vector3(x * centerX + gap + wiggleX, 0, y * centerY + gap + wiggleY );
      
                    var newTree = Instantiate(TreeElement, pos, Quaternion.identity);
                    newTree.GridPosition = new Vector2Int(x, y);
                }
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

    public float GetHealt(int x, int y)
    {
        if(_map != null){
            return _map[x, y].Healt;
        }
        else{
            return 1.0f;
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
            
            Debug.Log("Tree burned " + fireName);
        }

        // Spread fire
        var targetTrees = GetSpreadLocations(fire.X, fire.Y);

        foreach(var tree in targetTrees){
            if(_random.Next(0,100) < SpreadOdds){
                AddFire(tree.Key.x, tree.Key.y);
                Debug.Log("Fire spreads " + fireName);
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
    void OnDrawGizmos() {
        if (_map != null) {                
            var modelWidth = 12.0f;
            var modelHeight = 12.0f;
            var centerX = (int) Math.Round(modelWidth / 2, 0);
            var centerY = (int) Math.Round(modelHeight / 2, 0);

            var startX = (int) Math.Round(modelWidth / 4, 0);
            var startY = (int) Math.Round(modelWidth / 4, 0);
            
            for (int x = 0; x < Width; x ++) {
                for (int y = 0; y < Height; y ++) {
                    
                    var gap = (x == 0 && y == 0 ) ? 0 : Padding;
                    Gizmos.color = Color.cyan;              
                    Gizmos.DrawWireCube( new Vector3(x * centerX + startX + gap, 5.0f, y * centerY - startY + gap), new Vector3(modelWidth, 10.0f, modelHeight));
                }
            }
        }
    }
}
