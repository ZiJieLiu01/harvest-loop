using UnityEngine;

[CreateAssetMenu(menuName = "Crop/Crop Data")]
public class CropData : PlaceableData
{
    [Header("Crop Data")]
    public Sprite[] growthStageSprites;     // Sprites for each stages of the crop growth
    public int[] growthStageDays;           // Days required to reach each stage
}
