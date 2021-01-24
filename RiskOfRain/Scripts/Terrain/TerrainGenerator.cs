
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace RiskOfRain{

    public class TerrainGenerator : MonoBehaviour
    {

        private static TerrainGenerator instance;

        Tilemap tilemap;
        Tilemap noColliderTileMap;
        [SerializeField] TerrainSprites sprites;
        [Space]
        [SerializeField] TerrainBiomes biome;
        enum TerrainGenType { CellularAutamata, PerlinNoise }
        [Space]
        [SerializeField] TerrainGenType terrainType;
        enum TerrainDepth { Surface, Cave }
        [SerializeField] TerrainDepth terrainDepth;
    
        [SerializeField] int mapWidth, mapHeight;
        [Range(0f, 1f)] [SerializeField] float fillPercent;
        [Header("Cellular Autamata")]
        [SerializeField] int smoothingPasses = 5;
        [Header("Perlin Noise")]
        [SerializeField] float amplitude;
        [SerializeField] int heightAddition;
        [SerializeField] float frequency;
        [SerializeField] float power;
        [SerializeField] int octaves;
        [SerializeField] float lucanarity;
        [SerializeField] float persistance;
        Tile[,] map;
        List<Region> regions = new List<Region>();

        bool hasInitialized = false;

        public static Tile[,] Map => instance.map;
        public static List<Region> Regions => instance.regions;
        public static TerrainSprites Sprites => instance.sprites;
        public static TerrainBiomes Biome => instance.biome;

        private void Awake(){
            if (instance == null)
                instance = this; 
        }

        void Start(){
            tilemap = transform.GetChild(0).GetComponent<Tilemap>();
            noColliderTileMap = transform.GetChild(1).GetComponent<Tilemap>();
            GenerateTerrain(false);
        }
    
        #region Terrain Generation

        public static void GenerateTerrain(bool clearMap){
            instance._GenerateTerrain(clearMap);
        }

        private void Initialize(){
            map = new Tile[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++){
                for (int y = 0; y < mapHeight; y++){
                    map[x, y] = new Tile(x,y);
                }
            }

            hasInitialized = true;

            
        }

        void _GenerateTerrain(bool clearMap){

            biome = Utility.RandomEnumElement<TerrainBiomes>();

            if(clearMap){
                ClearMap();
            }

            if(!hasInitialized)
                Initialize();

            switch (terrainType){
                case TerrainGenType.CellularAutamata:
                    GenerateTerrainCA();
                    break;
                case TerrainGenType.PerlinNoise:
                    GenerateTerrainNoise();
                    break;
            }

            SetEdgeWalls();

            regions = GetAllRegions(false);

            TerrainPropsGenerator.SpawnProps(biome);

            DrawMap();
        }

        #region Cellular Autamata

        void GenerateTerrainCA(){

            for (int x = -mapWidth / 2; x < mapWidth / 2; x++){
                for (int y = -mapHeight / 2; y < mapHeight / 2; y++) {
                    if (x < -mapWidth / 2 + 3|| x >= mapWidth / 2 - 3 || y < -mapHeight / 2 + 3 || y >= mapHeight / 2 -3){
                        map[x + mapWidth / 2, y + mapHeight / 2].SetWall(true);
                    } else{

                        if (terrainDepth == TerrainDepth.Surface){
                            float noiseVal = (Mathf.PerlinNoise(x * 0.03f, y * 0.03f) * 2 - 1)*10;

                            if (y > noiseVal)
                                continue;
                        }

                        bool rand = Random.value < fillPercent;
                        map[x+mapWidth/2, y+mapHeight/2].SetWall(rand);
                    }
                }
            }

            for (int i = 0; i < smoothingPasses; i++){
                SmoothMap(); 
            }


        }
        
        void SmoothMap(){

            for (int x = -mapWidth / 2; x < mapWidth / 2; x++){
                for (int y = -mapHeight / 2; y < mapHeight / 2; y++){

                    int activeNeighbours = GetActiveNeighbourCount(x + mapWidth / 2, y + mapHeight / 2);

                    if (activeNeighbours > 4){
                        map[x + mapWidth / 2, y + mapHeight / 2].SetWall(true);
                    }else if (activeNeighbours < 4){
                        map[x + mapWidth / 2, y + mapHeight / 2].SetWall(false);
                    }
                }
            }
        }

        int GetActiveNeighbourCount(int xCoord, int yCoord){

            Tile[] neighbours = Get8Neighbours(xCoord, yCoord);
            int activeNeighbours = 0;
    
            for (int i = 0; i < neighbours.Length; i++){
                activeNeighbours += neighbours[i].wall ? 1 : 0;
            }
    
            return activeNeighbours;
    
        }

        Tile[] Get8Neighbours(int xCoord, int yCoord) {

            List<Tile> neighbours = new List<Tile>();
    
            int i = 0;
            for (int x = -1; x <= 1; x++){
                for (int y = -1; y <= 1; y++){
    
                    if (x == 0 && y == 0)
                        continue;
    
                    if (xCoord + x < 0 || xCoord + x >= mapWidth || yCoord + y < 0 || yCoord + y >= mapHeight)
                        continue;
    
                    neighbours.Add( map[xCoord + x, yCoord + y]);
                    i++;
                }
    
            }

            return neighbours.ToArray();
    
        }

        Tile[] Get4Neighbours(int xCoord, int yCoord){

            List<Tile> neighbours = new List<Tile>();

            if (xCoord - 1 >= 0)
                neighbours.Add(map[xCoord - 1, yCoord]);

            if (yCoord + 1 < mapHeight)
                neighbours.Add(map[xCoord, yCoord+1]);

            if (xCoord + 1 < mapWidth)
                neighbours.Add(map[xCoord + 1, yCoord]);

            if (yCoord - 1 >= 0)
                neighbours.Add(map[xCoord, yCoord - 1]);

            return neighbours.ToArray();

        }

        #endregion

        #region Perlin Noise

        void GenerateTerrainNoise(){

            if(terrainDepth == TerrainDepth.Surface){

                GenerateNoiseSurfaceMap();

            }else{
                GenerateNoiseCaveMap();
            }
        }

        void GenerateNoiseSurfaceMap(){

            float[] offsets = new float[octaves];

            for (int i = 0; i < octaves; i++){
                offsets[i] = Random.Range(-10000, 10000);
            }

            for (int x = 0; x < mapWidth; x++) {

                int noiseVal = heightAddition;

                float amp = amplitude;
                float freq = frequency;

                for (int i = 0; i < octaves; i++) {
                    noiseVal += Mathf.RoundToInt(Mathf.PerlinNoise((x + offsets[i]) * freq, 0) * amp);
                    freq *= lucanarity;
                    amp *= persistance;
                }

                if (x == 0 || x == mapWidth - 1){
                    for (int y = 0; y < mapHeight; y++){
                        map[x, y].SetWall(true);
                    }
                }else{
                    for (int y = 0; y < noiseVal; y++){
                        map[x, y].SetWall(true);
                    }
                }

                
            }
        }

        void GenerateNoiseCaveMap(){

            float[] offsets = new float[octaves];

            for (int i = 0; i < octaves; i++){
                offsets[i] = Random.Range(-10000, 10000);
            }


            float[,] values = new float[mapWidth, mapHeight];
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            for (int x = 0; x < mapWidth; x++){
                for (int y = 0; y < mapHeight; y++){

                    if(x == 0 || x == mapWidth -1 || y==0 || y == mapHeight - 1){
                        map[x, y].SetWall(true);
                        continue;
                    }

                    float noiseVal = 0;

                    float amp = 1;
                    float freq = frequency;

                    for (int i = 0; i < octaves; i++){
                        noiseVal += Mathf.PerlinNoise((x + offsets[i]) * freq, (y+offsets[i])*freq) * amp;
                        freq *= lucanarity;
                        amp *= persistance;
                    }

                    if (noiseVal < minValue) minValue = noiseVal;
                    if (noiseVal > maxValue) maxValue = noiseVal;

                    values[x, y] = noiseVal;

                }
            }


            for (int x = 0; x < mapWidth; x++){
                for (int y = 0; y < mapHeight; y++){

                    values[x, y] = Mathf.InverseLerp(minValue, maxValue, values[x, y]);

                    if (values[x, y] < fillPercent){
                        map[x, y].SetWall(true);

                    }

                }
            }

        }

        #endregion

        List<Region> GetAllRegions(bool tileType){

            List<Region> regions = new List<Region>();
            int[,] mapFlags = new int[mapWidth, mapHeight];

            for (int x = 1; x < mapWidth-1; x++){
                for (int y = 1; y < mapHeight-1; y++){

                    if(mapFlags[x,y] == 0 && map[x,y].wall == tileType) {


                        Region newRegions = FloodFillRegion(x, y);
                        regions.Add(newRegions);

                        foreach (Tile tile in newRegions.tiles){
                            mapFlags[tile.x, tile.y] = 1;
                        }

                    }
                }
            }

            return regions;

        }

        Region FloodFillRegion(int startX, int startY){

            List<Tile> tiles = new List<Tile>();
            int[,] mapFlags = new int[mapWidth, mapHeight];
            bool tileType = map[startX, startY].wall;

            Queue<Tile> queue = new Queue<Tile>();
            queue.Enqueue(map[startX, startY]);
            mapFlags[startX, startY] = 1;

            while(queue.Count > 0){

                Tile tile = queue.Dequeue();

                tiles.Add(tile);

                Tile[] neighbours = Get4Neighbours(tile.x, tile.y);

                foreach (Tile neighbour in neighbours){
                    if(mapFlags[neighbour.x,neighbour.y] == 0 && neighbour.wall == tileType){
                        mapFlags[neighbour.x, neighbour.y] = 1;
                        queue.Enqueue(neighbour);
                    }
                }

            }

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            for (int i = 0; i < tiles.Count; i++){
                if (minX > tiles[i].x) minX = tiles[i].x;
                if (maxX < tiles[i].x) maxX = tiles[i].x;
                if (minY > tiles[i].y) minY = tiles[i].y;
                if (maxY < tiles[i].y) maxY = tiles[i].y;

            }

            bool isSurface = maxY >= mapHeight-5;

            return new Region(tiles.ToArray(), isSurface, minX, maxX, minY, maxY);

        }

        void SetEdgeWalls(){
            for (int x = 0; x < mapWidth; x++){
                for (int y = 0; y < mapHeight; y++){

                    if (map[x, y].wall){

                        Tile[] neighbours = Get4Neighbours(x, y);

                        for (int i = 0; i < neighbours.Length; i++){
                            if (!neighbours[i].wall){

                                int xDiff = x - neighbours[i].x;
                                int yDiff = y - neighbours[i].y;

                                if(xDiff == -1)
                                    map[x, y].edges |= WallSides.Left;
                                if(yDiff == 1)
                                    map[x, y].edges |= WallSides.Top;
                                if (xDiff == 1)
                                    map[x, y].edges |= WallSides.Right;
                                if(yDiff == -1)
                                    map[x, y].edges |= WallSides.Bottom;

                            }
                            
                        }
                    }
                }
            }
        }

        #endregion

        void ClearMap(){

            for (int x = 0; x < mapWidth; x++){
                for (int y = 0; y < mapHeight; y++){
                    map[x, y] = new Tile(x,y);
                }
            }

            tilemap.ClearAllTiles();
            noColliderTileMap.ClearAllTiles();

        }

        void DrawMap(){

            for (int x = -mapWidth / 2; x < mapWidth / 2; x++){
                for (int y = -mapHeight / 2; y < mapHeight / 2; y++) {

                    Tile tile = map[x + mapWidth / 2, y + mapHeight / 2];

                    if (tile.sprite == null)
                        continue;

                    if (tile.sprite.hasCollider){
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile.sprite.sprite);
                    }else{
                        noColliderTileMap.SetTile(new Vector3Int(x, y, 0), tile.sprite.sprite);
                    }
                }
            }
        }

    }


    [System.Flags]
    public enum TerrainBiomes : byte
    {
        Plains = 1,
        Lava = 2,
        Water = 4,
    }
   
}
