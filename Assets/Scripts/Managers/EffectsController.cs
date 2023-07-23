using UnityEngine;

public class EffectsController : Singleton<EffectsController>
{
    [SerializeField] private GameObject cubeCrackEffectPrefab;
    
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color purpleColor;
    [SerializeField] private Color pinkColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color yellowColor;
    
    public void SpawnCubeCrackEffect(Vector3 spawnPos, CubeTypes cubeType)
    {
        GameObject spawnedEffect = Instantiate(cubeCrackEffectPrefab, spawnPos, Quaternion.identity);
        var mainModule = spawnedEffect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = DetectCubeColor(cubeType);
        var subModule = spawnedEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        subModule.startColor = DetectCubeColor(cubeType);
        spawnedEffect.GetComponent<ParticleSystem>().Play();
        Destroy(spawnedEffect, 3f);
    }
    
    private Color DetectCubeColor(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Blue:
                return blueColor;
            case CubeTypes.Green:
                return greenColor;
            case CubeTypes.Pink:
                return pinkColor;
            case CubeTypes.Purple:
                return purpleColor;
            case CubeTypes.Red:
                return redColor;
            case CubeTypes.Yellow:
                return yellowColor;
            
            default: 
                return blueColor;
        }
    }
}