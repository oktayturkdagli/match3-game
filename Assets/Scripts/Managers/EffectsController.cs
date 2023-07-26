using UnityEngine;

public class EffectsController : Singleton<EffectsController>
{
    [SerializeField] private GameObject blockFragmentationEffectPrefab;
    
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color purpleColor;
    [SerializeField] private Color pinkColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color yellowColor;
    
    public void PlayBlockFragmentationEffect(Vector3 spawnPosition, BlockTypes blockType)
    {
        GameObject instantiatedEffect = Instantiate(blockFragmentationEffectPrefab, spawnPosition, Quaternion.identity);
        var mainModule = instantiatedEffect.GetComponent<ParticleSystem>().main;
        var subModule = instantiatedEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        var blockColor = DetectBlockColor(blockType);
        mainModule.startColor = blockColor;
        subModule.startColor = blockColor;
        instantiatedEffect.GetComponent<ParticleSystem>().Play();
        Destroy(instantiatedEffect, 3f);
    }
    
    private Color DetectBlockColor(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueColor;
            case BlockTypes.Green:
                return greenColor;
            case BlockTypes.Pink:
                return pinkColor;
            case BlockTypes.Purple:
                return purpleColor;
            case BlockTypes.Red:
                return redColor;
            case BlockTypes.Yellow:
                return yellowColor;
            
            default: 
                return blueColor;
        }
    }
}