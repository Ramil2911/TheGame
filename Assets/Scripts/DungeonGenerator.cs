using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Linq;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum DungeonState { Inactive, GeneratingMain, GeneratingBranches, Cleanup, Completed }

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] startTilePrefabs;
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] exitPrefabs;
    [SerializeField] private GameObject[] blockedPrefabs;
    [SerializeField] private GameObject[] doorPrefabs;

    [Header("Debugging Options")] 
    [SerializeField] private bool useBoxColliders;
    [SerializeField] private bool useLightsForDebugging;
    [SerializeField] private bool restoreLightsAfterDebugging;
    
    [Header("Key Bindings")]
    [SerializeField] private KeyCode reloadButton =  KeyCode.Backspace;
    [SerializeField] private KeyCode toggleMapKey = KeyCode.R;
    
    [Header("Generation Limits")]
    [SerializeField][Range(2, 100)] private int mainLength = 10;
    [SerializeField][Range(0, 50)] private int branchLength = 5;
    [SerializeField][Range(0, 25)] private int numBranches = 10;
    [SerializeField][Range(0, 1f)] private float constructionDelay;
    
    [Header("Available at Runtime")]
    public List<Tile> generatedTiles = new List<Tile>();

    [HideInInspector] public DungeonState dungeonState = DungeonState.Inactive;
    // public GameObject goCamera, goPlayer;
    private List<Connector> _availableConnectors = new List<Connector>();
    private Color _startLightColor = Color.white;
    private Transform _tileFrom, _tileTo, _tileRoot;
    private Transform _container;
    private int _attempts;
    private int _maxAttempts = 50;
    
    private void Start()
    {
        // goCamera = GameObject.Find("OverheadCamera");
        // goPlayer = GameObject.FindWithTag("Player");
        StartCoroutine(DungeonBuild());
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(reloadButton))
        {
            SceneManager.LoadScene("Game");
        }
        // if (Input.GetKeyDown(toggleMapKey))
        // {
        //     goCamera.SetActive(!goCamera.activeInHierarchy);
        //     goPlayer.SetActive(!goPlayer.activeInHierarchy);
        // }
    }
    
    
    

    IEnumerator DungeonBuild()
    {
        //////////////                          ///////////
        /////////////     Builds main path     ////////////
        ///////////                           /////////////
        
        
        // goCamera.SetActive(true);
        // goPlayer.SetActive(false);
        GameObject goContainer = new GameObject("Main Path");
        _container = goContainer.transform;
        _container.SetParent(transform);
        _tileRoot = GenerateStartTile();
        DebugRoomLighting(_tileRoot, Color.blue);
        _tileTo = _tileRoot; // it'll be changed to _tileFrom, so don't worry
        dungeonState = DungeonState.GeneratingMain;
        while (generatedTiles.Count < mainLength)
        {
            yield return new WaitForSeconds(constructionDelay);
            _tileFrom = _tileTo;
            if (generatedTiles.Count == mainLength - 1)
            {
                _tileTo = GenerateExitTile();
                DebugRoomLighting(_tileTo, Color.magenta);
            }
            else
            {
                _tileTo = GenerateTile();
                DebugRoomLighting(_tileTo, Color.yellow);
            }
            ConnectTiles();
            CollisionCheck();
        }
        
        //////////////                          ///////////
        /////////////     Builds branches      ////////////
        ///////////                           /////////////
        
        // get all connectors within container that NOT already connected
        foreach (Connector connector in _container.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                if (!_availableConnectors.Contains(connector))
                {
                    _availableConnectors.Add(connector);
                }
            }
        }
        
        // creates branches
        dungeonState = DungeonState.GeneratingBranches;
        for (int b = 0; b < numBranches; b++)
        {
            if (_availableConnectors.Count > 0) 
            {
                goContainer = new GameObject("Branch " + (b + 1));
                _container = goContainer.transform;
                _container.SetParent(transform);
                int availIndex = Random.Range(0, _availableConnectors.Count);
                _tileRoot = _availableConnectors[availIndex].transform.parent.parent;
                _availableConnectors.RemoveAt(availIndex);
                _tileTo = _tileRoot;
                for (int i = 0; i < branchLength - 1; i++)
                {
                    yield return new WaitForSeconds(constructionDelay);
                    _tileFrom = _tileTo;
                    _tileTo = GenerateTile();
                    DebugRoomLighting(_tileTo, Color.green);
                    ConnectTiles();
                    CollisionCheck();
                }
            }
            else { break; }
        }
        
        //////////////                          ///////////
        /////////////        Cleanup           ////////////
        ///////////                           /////////////

        dungeonState = DungeonState.Cleanup;
        LightRestoration();
        CleanupBoxes();
        BlockedPassages();
        dungeonState = DungeonState.Completed;
        yield return null;
        // goCamera.SetActive(false);
        // goPlayer.SetActive(true);
    }

    // private void SpawnDoors()
    // {
    //     if (doorPercent > 0)
    //     {
    //         Connector[] allConnectors = transform.GetComponentsInChildren<Connector>();
    //         for (int i = 0; i < allConnectors.Length; i++)
    //         {
    //             Connector myConnector = allConnectors[i];
    //             if (myConnector.isConnected)
    //             {
    //                 int roll = Random.Range(1, 101);
    //                 if (roll <= doorPercent)
    //                 {
    //                     Vector3 halfExtents = new Vector3(myConnector.size.x, 1f, myConnector.size.x);
    //                     Vector3 pos = myConnector.transform.position;
    //                     Vector3 offset = Vector3.up * 0.5f; 
    //                     Collider[] hits = Physics.OverlapBox(pos + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Door"));
    //                     if (hits.Length == 0)
    //                     {
    //                         int doorIndex = Random.Range(0, doorPrefabs.Length);
    //                         GameObject goDoor = Instantiate(doorPrefabs[doorIndex], pos, myConnector.transform.rotation, myConnector.transform) as GameObject;
    //                         goDoor.name = doorPrefabs[doorIndex].name;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    private void BlockedPassages()
    {
        foreach (Connector connector in transform.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                Vector3 pos = connector.transform.position;
                int wallIndex = Random.Range(0, blockedPrefabs.Length);
                GameObject goWall = Instantiate(blockedPrefabs[wallIndex], pos, connector.transform.rotation, connector.transform) as GameObject;
                goWall.name = blockedPrefabs[wallIndex].name;
            }
        }
    }

    private void CollisionCheck()
    {
        BoxCollider box = _tileTo.GetComponent<BoxCollider>();
        if (box == null)
        {
            box = _tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        var center = box.center;
        Vector3 offset = (_tileTo.right * center.x) + (_tileTo.up * center.y) + (_tileTo.forward * center.z);
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics.OverlapBox(_tileTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Tile")).ToList();
        if (hits.Count > 0)
        {
            if (hits.Exists(x => x.transform != _tileFrom && x.transform != _tileTo))
            {
                // something other than _tileTo and _tileFrom was hit
                _attempts++;
                int toIndex = generatedTiles.FindIndex(x => x.tile == _tileTo);
                if (generatedTiles[toIndex].connector != null)
                {
                    generatedTiles[toIndex].connector.isConnected = false;
                }
                generatedTiles.RemoveAt(toIndex);
                DestroyImmediate(_tileTo.gameObject);
                
                // backtracking
                if (_attempts >= _maxAttempts)
                {
                    int fromIndex = generatedTiles.FindIndex(x => x.tile == _tileFrom);
                    Tile myTileFrom = generatedTiles[fromIndex];
                    if (_tileFrom != _tileRoot)
                    {
                        if (myTileFrom.connector != null)
                        {
                            myTileFrom.connector.isConnected = false;
                        }
                        _availableConnectors.RemoveAll(x => x.transform.parent.parent == _tileFrom);
                        generatedTiles.RemoveAt(fromIndex);
                        DestroyImmediate(_tileFrom.gameObject);
                        
                        if (myTileFrom.origin != _tileRoot)
                        {
                            _tileFrom = myTileFrom.origin;
                        }
                        else if (_container.name.Contains("Main"))
                        {
                            if (myTileFrom.origin != null)
                            {
                                _tileRoot = myTileFrom.origin;
                                _tileFrom = _tileRoot;
                            }
                        }
                        else if (_availableConnectors.Count > 0)
                        {
                            PickRandomAvailableConnector();
                        }
                        else { return; }
                    }
                    else if (_container.name.Contains("Main"))
                    {
                        if (myTileFrom != null)
                        {
                            _tileRoot = myTileFrom.origin;
                            _tileFrom = _tileRoot;
                        }
                    }
                    else if (_availableConnectors.Count > 0)
                    {
                        PickRandomAvailableConnector();
                    }
                    else { return; }
                }
                // retry generating tile
                if (_tileFrom != null)
                {
                    if (generatedTiles.Count == mainLength - 1)
                    {
                        _tileTo = GenerateExitTile();
                        DebugRoomLighting(_tileTo, Color.magenta);
                    }
                    else
                    {
                        _tileTo = GenerateTile();
                        Color retryColor = _container.name.Contains("Branch") ? Color.green : Color.yellow;
                        DebugRoomLighting(_tileTo, retryColor * 2f);
                    }
                    ConnectTiles();
                    CollisionCheck();
                }
            }
            else { _attempts = 0; } // nothing other than _tileTo and _tileFrom was hit
        }
    }

    void PickRandomAvailableConnector()
    {
        int availIndex = Random.Range(0, _availableConnectors.Count);
        _tileRoot = _availableConnectors[availIndex].transform.parent.parent;
        _availableConnectors.RemoveAt(availIndex);
        _tileFrom = _tileRoot;
    }

    private void LightRestoration()
    {
        if (useLightsForDebugging && restoreLightsAfterDebugging && Application.isEditor)
        {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach (Light light in lights)
            {
                light.color = _startLightColor;
            }
        }
    }

    private void DebugRoomLighting(Transform tile, Color lightColor)
    {
        if (useLightsForDebugging && Application.isEditor)
        {
            Light[] lights = tile.GetComponentsInChildren<Light>();
            if (lights.Length > 0)
            {
                if (_startLightColor == Color.white)
                {
                    _startLightColor = lights[0].color;
                }

                foreach (Light light in lights)
                {
                    light.color = lightColor;
                }
            }
        }
    }

    private void CleanupBoxes()
    {
        if (!useBoxColliders)
        {
            foreach (Tile myTile in generatedTiles)
            {
                BoxCollider box = myTile.tile.GetComponent<BoxCollider>();
                if (box != null) { Destroy(box); }
            }
        }
    }

    private void ConnectTiles()
    {
        Transform connectFrom = getRandomConnector(_tileFrom);
        if (connectFrom == null) { return; }
        Transform connectTo = getRandomConnector(_tileTo);
        if (connectTo == null) { return; }
        connectTo.SetParent(connectFrom);
        _tileTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero;
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0, 180f, 0);
        _tileTo.SetParent(_container);
        connectTo.SetParent(_tileTo.Find("Connectors"));
        generatedTiles.Last().connector = connectFrom.GetComponent<Connector>();

    }

    private Transform getRandomConnector(Transform tile)
    {
        if (tile == null) {return null; }
        List<Connector> connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(x => x.isConnected == false);
        if (connectorList.Count > 0)
        {
            int connectorIndex = Random.Range(0, connectorList.Count);
            connectorList[connectorIndex].isConnected = true;
            if (tile == _tileFrom)
            {
                BoxCollider box = tile.GetComponent<BoxCollider>();
                if (box == null)
                {
                    box = tile.gameObject.AddComponent<BoxCollider>();
                    box.isTrigger= true;
                }
            }
            return connectorList[connectorIndex].transform;
        }
        return null;
    }
    private Transform GenerateTile()
    {
        int index = Random.Range(0, tilePrefabs.Length);
        GameObject goTile = Instantiate(tilePrefabs[index], Vector3.zero, Quaternion.identity, _container) as GameObject;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == _tileFrom)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        goTile.name = tilePrefabs[index].name;
        return goTile.transform;
    }
    
    private Transform GenerateExitTile()
    {
        int index = Random.Range(0, exitPrefabs.Length);
        GameObject goTile = Instantiate(exitPrefabs[index], Vector3.zero, Quaternion.identity, _container) as GameObject;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == _tileFrom)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        goTile.name = "Exit Room";
        return goTile.transform;
    }

    private Transform GenerateStartTile()
    {
        
        int index = Random.Range(0, startTilePrefabs.Length);
        GameObject goTile = Instantiate(startTilePrefabs[index], Vector3.zero, Quaternion.identity, _container) as GameObject;
        goTile.name = "Start Room";
        generatedTiles.Add(new Tile(goTile.transform, null));
        return goTile.transform;
    }
}
