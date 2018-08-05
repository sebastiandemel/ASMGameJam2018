using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

using Elements;

public class ForestManager : MonoBehaviour {

    private GridElement[,] _map;

    public static ForestManager instance = null; 

    public int Padding;

    public float GridSize;

    public int Wiggle;
    
    public GridElement GridElement;

    [Range(0,100)]
    public int SpreadOdds;

    [Range(0,100)]
    public int ForestDensity;

    public int Width;
    public int Height;

    private System.Random _random;

    void Awake() {
        _map = new GridElement[Width, Height];

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

        var centerX = (int) Math.Round( Width / 2.0f, 0);
        var centerY = (int) Math.Round( Height / 2.0f, 0);

        _map[centerX, centerY].gameObject.AddComponent(typeof(FireElement));
        Debug.Log("Created fire at " + centerX + ", " + centerY);
    }

    void Start() {
    }

    void Update() {
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
                var gap = (x == 0 && y == 0 ) ? 0 : Padding;

                // Wigle
                var wiggleX = _random.Next(-Wiggle, Wiggle);
                var wiggleY = _random.Next(-Wiggle, Wiggle);

                Vector3 pos = new Vector3(x * centerX + gap + wiggleX, 0, y * centerY + gap + wiggleY );
    
                var newTree = Instantiate(GridElement, pos, Quaternion.identity);
                newTree.GridPosition = new Vector2Int(x, y);
                _map[x, y] = newTree;
            }
        }
    }
}
