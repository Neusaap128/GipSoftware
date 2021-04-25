
using UnityEngine;
using System.Collections.Generic;

namespace RiskOfRain {

    public static class TerrainPropsGenerator
    {

        public static List<GameObject> spawnedProps = new List<GameObject>();

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

            for (int i = 0; i < Random.Range(4,8); i++){
                GameObject go = TerrainGenerator.SpawnGroundPropGameObject(TerrainGenerator.Sprites.itemPickUp);
                go.GetComponent<ItemPickUp>().SetItem(TerrainGenerator.AllItem.SelectRandomItem());
                spawnedProps.Add(go);
            }

        }

        static void SpawnPlainProps(){
        
            for (int i = 0; i < 50; i++){
                SpawnVine();
            }

            for (int i = 0; i < 500; i++){
                SpawnGrass();
            }

            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnWaterPuddle();
            }

        }

        static void SpawnWaterProps(){

            for (int i = 0; i < 20; i++){
                SpawnVine();
            }

            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnWaterPuddle();
            }

        }

        static void SpawnLavaProps(){

            int amountOfPuddles = Random.Range(1, Mathf.RoundToInt(TerrainGenerator.Regions.Count / 3));
            for (int i = 0; i < amountOfPuddles; i++){
                SpawnLavaPuddle();
            }

            for (int i = 0; i < Random.Range(1,6); i++){
                spawnedProps.Add(TerrainGenerator.SpawnGroundPropGameObject(TerrainGenerator.Sprites.explosiveBarrel));
            }

        }

        #region Plains
    
        static void SpawnVine(){

            Tile tile = TerrainGenerator.CeilingTiles[Random.Range(0,TerrainGenerator.CeilingTiles.Count)];

            int randSize = Random.Range(2, 7);
            for (int i = 1; i < randSize+1; i++){
                if (tile.y - i < 0)
                    return;
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

            if(randRegion == Region.largestRegion)
                randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight / 16));

            foreach (Tile tile in randRegion.tiles){
        
                if (tile.y <= randRegion.minY + randPuddleHeight)
                    tile.sprite = TerrainGenerator.Sprites.blueTileBase;
                
            }
        
        }

        static void SpawnGrass(){

            int randHeight = Random.value < 0.5f ? 1 : 2;

            switch (randHeight){
                case 1:
                    TerrainGenerator.SpawnGroundPropSprite(TerrainGenerator.Sprites.grassTileBase);
                    break;
                case 2:
                    TerrainGenerator.SpawnGroundPropSprite(TerrainGenerator.Sprites.tallGrassTileBase);
                    break;
            }

        }

        #endregion

        #region Water 

        #endregion

        #region Lava

        static void SpawnLavaPuddle(){
        
            Region randRegion = TerrainGenerator.Regions[Random.Range(0, TerrainGenerator.Regions.Count)];

            int regionHeight = randRegion.maxY - randRegion.minY;
            int randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight/2));
        
            if (randRegion == Region.largestRegion)
                randPuddleHeight = Random.Range(1, Mathf.RoundToInt(regionHeight / 16));

            foreach (Tile tile in randRegion.tiles){
        
                if (tile.y <= randRegion.minY + randPuddleHeight)
                    tile.sprite = TerrainGenerator.Sprites.redTileBase;
                
            }
        
        }

        #endregion

    }
}