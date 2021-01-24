
using UnityEngine;

namespace RiskOfRain {

    public static class TerrainPropsGenerator
    {
        
        public static void SpawnProps(TerrainBiomes biome){
        
            switch (biome){
        
                case TerrainBiomes.Plains:
                    SpawnPlainProps();
                    break;

                case TerrainBiomes.Lava:
                    SpawnLavaProps();
                    break;
                case TerrainBiomes.Water:
                    SpawnWaterProps();
                    break;

            }
        
        }

        #region Plains
        static void SpawnPlainProps(){
        
            for (int i = 0; i < 20; i++){
                SpawnVine();
            }
        
            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnWaterPuddle();
            }

            for (int i = 0; i < 50; i++){
                SpawnGrass();
            }

        }
    
        static void SpawnVine(){

            Region randRegion = TerrainGenerator.Regions[Random.Range(0, TerrainGenerator.Regions.Count)];
        
            Tile tile = randRegion.tiles[Random.Range(0,randRegion.tiles.Length)];

            Tile tileAbove = TerrainGenerator.Map[tile.x, tile.y + 1];
        
            while (!tileAbove.wall){
                tile = randRegion.tiles[Random.Range(0, randRegion.tiles.Length)];
                tileAbove = TerrainGenerator.Map[tile.x, tile.y + 1];
            }
        
            //we dont have to check the tile above's edges as regions only contain empty tiles
            int randSize = Random.Range(2, 7);
            for (int i = 0; i < randSize; i++){
        
                Tile currentTile = TerrainGenerator.Map[tile.x, tile.y - i];
                if (currentTile.wall)
                    return;
        
                currentTile.sprite = TerrainGenerator.Sprites.greenTileBase;
            }
            
        }
    
        static void SpawnWaterPuddle(){
        
            Region randRegion = TerrainGenerator.Regions[Random.Range(0, TerrainGenerator.Regions.Count)];
        
            int regionHeight = randRegion.maxY - randRegion.minY;
            int randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight/2));

            if(randRegion.isSurface)
                randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight / 16));

            foreach (Tile tile in randRegion.tiles){
        
                if (tile.y <= randRegion.minY + randPuddleHeight)
                    tile.sprite = TerrainGenerator.Sprites.blueTileBase;
                
            }
        
        }

        static void SpawnGrass(){

            Region randRegion = TerrainGenerator.Regions[Random.Range(0, TerrainGenerator.Regions.Count)];

            int randHeight = Random.value < 0.5f ? 1 : 2;

            Tile tile  = randRegion.tiles[Random.Range(0, randRegion.tiles.Length)];
            Tile tileBelow = TerrainGenerator.Map[tile.x, tile.y - 1];

            while (!tileBelow.wall){
                tile = randRegion.tiles[Random.Range(0, randRegion.tiles.Length)];
                tileBelow = TerrainGenerator.Map[tile.x, tile.y - 1];
            }

            switch (randHeight){
                case 1:
                    tile.sprite = TerrainGenerator.Sprites.grassTileBase;
                    break;
                case 2:
                    tile.sprite = TerrainGenerator.Sprites.tallGrassTileBase;
                    break;
            }

        }

        #endregion

        #region Water 
        static void SpawnWaterProps(){

            for (int i = 0; i < 20; i++){
                SpawnVine();
            }

            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnWaterPuddle();
            }

        }

        #endregion

        #region Lava

        static void SpawnLavaProps(){

            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnLavaPuddle();
            }
        }

        static void SpawnLavaPuddle(){
        
            Region randRegion = TerrainGenerator.Regions[Random.Range(0, TerrainGenerator.Regions.Count)];

            int regionHeight = randRegion.maxY - randRegion.minY;
            int randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight/2));
        
            if (randRegion.isSurface)
                randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight / 16));

            foreach (Tile tile in randRegion.tiles){
        
                if (tile.y <= randRegion.minY + randPuddleHeight)
                    tile.sprite = TerrainGenerator.Sprites.redTileBase;
                
            }
        
        }

        #endregion

    }
}