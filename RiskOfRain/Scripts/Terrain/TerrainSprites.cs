
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiskOfRain { 

    [CreateAssetMenu(fileName = "TerrainSprites", menuName = "Shooter/TerrainSprites")]
    public class TerrainSprites : ScriptableObject
    {
    
        [Header("Sprites")]
        public TerrainSprite wallTileBase;
        public TerrainSprite blueTileBase;
        public TerrainSprite redTileBase;
        public TerrainSprite greenTileBase;
        public TerrainSprite grassTileBase;
        public TerrainSprite tallGrassTileBase;
        [Header("GameObjects")]
        public GameObject explosiveBarrel;
        public GameObject Portal;
        public GameObject itemPickUp;

    }
    
    [System.Serializable]
    public class TerrainSprite{
        public bool hasCollider;
        public TileBase sprite;
    }

}