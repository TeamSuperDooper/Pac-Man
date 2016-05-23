using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileManager : MonoBehaviour {
    //MESH MERGER OPTIMIZATION VARS
    public MeshFilter[] meshFilters;
    public Material material;


    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private GameObject floor;

    [SerializeField]
    private GameObject pellet;

    public class Tile {
        public int x { get; set; }
        public int y { get; set; }
        public bool occupied { get; set; }
        public int adjacentCount { get; set; }
        public bool isIntersection { get; set; }

        public Tile left, right, up, down;

        public Tile(int x_in, int y_in) {
            x = x_in; y = y_in;
            occupied = false;
            left = right = up = down = null;
        }
    };

    public List<Tile> tiles = new List<Tile>();

    // Use this for initialization
    void Start() {
        ReadTiles();
        MeshMerge();
    }

    // Update is called once per frame
    void Update() {
        //DrawNeighbors();

    }

    //-----------------------------------------------------------------------
    // hardcoded tile data: 1 = free tile, 0 = wall
    void ReadTiles() {
        // hardwired data instead of reading from file (not feasible on web player)
        string row = "";
        string data = @"0000000000000000000000000000
0111111111111001111111111110
0100001000001001000001000010
0500001000001111000001000050
0100001000001001000001000010
0111111111111001111111111110
0100001001000000001001000010
0100001001000000001001000010
0111111001111001111001111110
0001001000001001000001001000
0001001000001001000001001000
0111001111111111111111001110
0100001001000000001001000010
0100001001022442201001000010
3111111001022222201001111113
0100001001022222201001000010
0100001001000000001001000010
0111001001111111111001001110
0001001001000000001001001000
0001001001000000001001001000
0111111111111111111111111110
0500001000001001000001000050
0100001000001001000001000010
0111001111111001111111001110
0001001001000000001001001000
0001001001000000001001001000
0111111001111001111001111110
0100001000001001000001000010
0100001000001001000001000010
0111111111111111111111111110
0000000000000000000000000000";
        Vector3 pos = Vector3.zero;

        //Create floor
        GameObject mapFloor = (GameObject)Instantiate(floor, new Vector3(-30f / 2f, -.23f, -29f / 2f), Quaternion.identity);
        Vector3 floorScale = mapFloor.transform.localScale;
        floorScale.x *= 31;
        floorScale.z *= 28;
        mapFloor.transform.localScale = floorScale;

        int X = 1, Y = 31;
        using (StringReader reader = new StringReader(data)) {
            string line;
            while ((line = reader.ReadLine()) != null) {

                pos.z = 0;
                X = 1; // for every line
                for (int i = 0; i < line.Length; ++i) {
                    row += line[i].ToString();
                    Tile newTile = new Tile(X, Y);

                    pos.z -= 1f;
                    //We found a wall
                    if (line[i] == '0') {
                        GameObject newWall = (GameObject)Instantiate(wall, pos, Quaternion.identity);
                        newWall.transform.parent = transform;
                    } else if (line[i] == '1') {
                        //Instantiate(pellet, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
                        GameObject newPellet = (GameObject)Instantiate(pellet, new Vector3(pos.x, pos.y + 0.1f, pos.z), pellet.transform.rotation);
                        //newPellet.transform.parent = transform;
                    } else if(line[i] == '2') {
                        //Ghost house
                    } else if (line[i] == '3') {
                        //Portal
                    } else if (line[i] == '4') {
                        //Ghost door
                    } else if (line[i] == '5') {
                        //Powerup: Ghost eater
                    }


                    // if the tile we read is a valid tile (movable)
                    if (line[i] == '1') {
                        // check for left-right neighbor
                        if (i != 0 && line[i - 1] == '1') {
                            // assign each tile to the corresponding side of other tile
                            newTile.left = tiles[tiles.Count - 1];
                            tiles[tiles.Count - 1].right = newTile;

                            // adjust adjcent tile counts of each tile
                            newTile.adjacentCount++;
                            tiles[tiles.Count - 1].adjacentCount++;
                        }
                    }

                    // if the current tile is not movable
                    else newTile.occupied = true;

                    // check for up-down neighbor, starting from second row (Y<30)
                    int upNeighbor = tiles.Count - line.Length; // up neighbor index
                    if (Y < 30 && !newTile.occupied && !tiles[upNeighbor].occupied) {
                        tiles[upNeighbor].down = newTile;
                        newTile.up = tiles[upNeighbor];

                        // adjust adjcent tile counts of each tile
                        newTile.adjacentCount++;
                        tiles[upNeighbor].adjacentCount++;
                    }

                    tiles.Add(newTile);
                    X++;
                }

                Y--;

                //Debug.Log("Row " + Y.ToString("D3") + " = " + row);
                row = "";

                pos.x -= 1f;
            }
        }

        // after reading all tiles, determine the intersection tiles
        foreach (Tile tile in tiles) {
            if (tile.adjacentCount > 2)
                tile.isIntersection = true;
        }

    }

    //-----------------------------------------------------------------------
    // Draw lines between neighbor tiles (debug)
    void DrawNeighbors() {
        foreach (Tile tile in tiles) {
            Vector3 pos = new Vector3(tile.x, tile.y, 0);
            Vector3 up = new Vector3(tile.x + 0.1f, tile.y + 1, 0);
            Vector3 down = new Vector3(tile.x - 0.1f, tile.y - 1, 0);
            Vector3 left = new Vector3(tile.x - 1, tile.y + 0.1f, 0);
            Vector3 right = new Vector3(tile.x + 1, tile.y - 0.1f, 0);

            if (tile.up != null) Debug.DrawLine(pos, up);
            if (tile.down != null) Debug.DrawLine(pos, down);
            if (tile.left != null) Debug.DrawLine(pos, left);
            if (tile.right != null) Debug.DrawLine(pos, right);
        }

    }


    //----------------------------------------------------------------------
    // returns the index in the tiles list of a given tile's coordinates
    public int Index(int X, int Y) {
        // if the requsted index is in bounds
        //Debug.Log ("Index called for X: " + X + ", Y: " + Y);
        if (X >= 1 && X <= 28 && Y <= 31 && Y >= 1)
            return (31 - Y) * 28 + X - 1;

        // else, if the requested index is out of bounds
        // return closest in-bounds tile's index 
        if (X < 1) X = 1;
        if (X > 28) X = 28;
        if (Y < 1) Y = 1;
        if (Y > 31) Y = 31;

        return (31 - Y) * 28 + X - 1;
    }

    public int Index(Tile tile) {
        return (31 - tile.y) * 28 + tile.x - 1;
    }

    //----------------------------------------------------------------------
    // returns the distance between two tiles
    public float distance(Tile tile1, Tile tile2) {
        return Mathf.Sqrt(Mathf.Pow(tile1.x - tile2.x, 2) + Mathf.Pow(tile1.y - tile2.y, 2));
    }

    public void MeshMerge() {
        // Mesh Merger Script
        // Copyright 2009, Russ Menapace
        // http://humanpoweredgames.com
        // if not specified, go find meshes
        if (meshFilters.Length == 0) {
            // find all the mesh filters
            Component[] comps = GetComponentsInChildren(typeof(MeshFilter));
            meshFilters = new MeshFilter[comps.Length];

            int mfi = 0;
            foreach (Component comp in comps)
                meshFilters[mfi++] = (MeshFilter)comp;
        }

        // figure out array sizes
        int vertCount = 0;
        int normCount = 0;
        int triCount = 0;
        int uvCount = 0;

        foreach (MeshFilter mf in meshFilters) {
            vertCount += mf.mesh.vertices.Length;
            normCount += mf.mesh.normals.Length;
            triCount += mf.mesh.triangles.Length;
            uvCount += mf.mesh.uv.Length;
            if (material == null)
                material = mf.gameObject.GetComponent<Renderer>().material;
        }

        // allocate arrays
        Vector3[] verts = new Vector3[vertCount];
        Vector3[] norms = new Vector3[normCount];
        Transform[] aBones = new Transform[meshFilters.Length];
        Matrix4x4[] bindPoses = new Matrix4x4[meshFilters.Length];
        BoneWeight[] weights = new BoneWeight[vertCount];
        int[] tris = new int[triCount];
        Vector2[] uvs = new Vector2[uvCount];

        int vertOffset = 0;
        int normOffset = 0;
        int triOffset = 0;
        int uvOffset = 0;
        int meshOffset = 0;

        // merge the meshes and set up bones
        foreach (MeshFilter mf in meshFilters) {
            foreach (int i in mf.mesh.triangles)
                tris[triOffset++] = i + vertOffset;

            aBones[meshOffset] = mf.transform;
            bindPoses[meshOffset] = Matrix4x4.identity;

            foreach (Vector3 v in mf.mesh.vertices) {
                weights[vertOffset].weight0 = 1.0f;
                weights[vertOffset].boneIndex0 = meshOffset;
                verts[vertOffset++] = v;
            }

            foreach (Vector3 n in mf.mesh.normals)
                norms[normOffset++] = n;

            foreach (Vector2 uv in mf.mesh.uv)
                uvs[uvOffset++] = uv;

            meshOffset++;

            MeshRenderer mr =
              mf.gameObject.GetComponent(typeof(MeshRenderer))
              as MeshRenderer;

            if (mr)
                mr.enabled = false;
        }

        // hook up the mesh
        Mesh me = new Mesh();
        me.name = gameObject.name;
        me.vertices = verts;
        me.normals = norms;
        me.boneWeights = weights;
        me.uv = uvs;
        me.triangles = tris;
        me.bindposes = bindPoses;

        // hook up the mesh renderer        
        SkinnedMeshRenderer smr =
          gameObject.AddComponent(typeof(SkinnedMeshRenderer))
          as SkinnedMeshRenderer;

        smr.sharedMesh = me;
        smr.bones = aBones;
        GetComponent<Renderer>().material = material;
    }
}
