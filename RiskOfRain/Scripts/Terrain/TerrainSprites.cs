
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiskOfRain { 

    [CreateAssetMenu(fileName = "Terrain Sprites")]
    public class TerrainSprites : ScriptableObject
    {
    
        public TerrainSprite wallTileBase;
        public TerrainSprite blueTileBase;
        public TerrainSprite redTileBase;
        public TerrainSprite greenTileBase;
        public TerrainSprite grassTileBase;
        public TerrainSprite tallGrassTileBase;
    
    }
    
    [System.Serializable]
    public class TerrainSprite
    {
        public bool hasCollider;
        public TileBase sprite;
    }

}