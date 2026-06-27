using UnityEngine;

public class CropInstance : TileObjectInstance
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private CropData m_cropData;

    private int m_currGrowthStage = 0;
    private int m_currGrowthDay = 0;

    private bool m_isWatered = true;        // True for now, TEMP!!
    private bool m_harvestable = false;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Init
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void Init(Vector2Int gridPos, PlaceableData placeableData)
    {
        base.Init(gridPos, placeableData);

        m_cropData = placeableData as CropData;
        if (m_cropData == null)
            return;

        CropManager.Instance.AddCrop(this);

        if(m_spriteRenderer != null && m_cropData.growthStageSprites.Length > 0)
        {
            m_spriteRenderer.sprite = m_cropData.growthStageSprites[0];
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    protected override void OnDestroy()
    {
        base.OnDestroy();

        CropManager.Instance.RemoveCrop(this);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Crop Logic
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void OnNewDay()
    {
        if (!m_isWatered || m_harvestable)
            return;

        m_currGrowthDay++;

        if (m_currGrowthDay >= m_cropData.growthStageDays[m_currGrowthStage])
        {
            m_currGrowthDay = 0;
            UpdateGrowthStage();
        }
    }

    private void UpdateGrowthStage()
    {
        m_currGrowthStage++;

        if(m_spriteRenderer != null)
        {
            m_spriteRenderer.sprite = m_cropData.growthStageSprites[m_currGrowthStage];
        }

        if(m_currGrowthStage >= m_cropData.growthStageSprites.Length - 1)
        {
            m_harvestable = true;
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // IInteractable Logic
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void OnPrimaryInteract(IItemReceiver receiver, ToolData itemData)
    {
        if (itemData == null)
            return;     // Does nothing

        if(itemData.toolType != ToolType.None && itemData.toolType != ToolType.Hoe)
        {
            Destroy(gameObject);    // The crop has been destroy
        }
    }

    public override void OnSecondaryInteract(IItemReceiver receiver)
    {
        if (!m_harvestable)
            return; // Not interactable until it is harvestable

        receiver.ReceiveItem(m_cropData.dropItem);
        Destroy(gameObject);
    }

    public override string GetDebugInfo()
    {
        string daysTilNextStage = "";
        if (!m_harvestable)
            daysTilNextStage = $"Days til next stage: {m_currGrowthDay}/{m_cropData.growthStageDays[m_currGrowthStage]}\n";

        return $"=== Crop ===\n" +
            $"Type: {m_cropData.displayName}\n" +
            daysTilNextStage +
            $"Stage: {m_currGrowthStage + 1}/{m_cropData.growthStageSprites.Length}\n" +
            $"Harvestable: {m_harvestable}";
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public bool IsHarvestable => m_harvestable;
}
