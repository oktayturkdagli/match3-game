using TMPro;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    [SerializeField] private TMP_Text countText; 
    [SerializeField] private GameObject markerObject;
    [SerializeField] private ParticleSystem goalEffect;
    [SerializeField] private int count;
    
    public int Count
    {
        get => count;
        
        set
        {
            if (value < count)
            {
                goalEffect.Stop();
                goalEffect.Play();
            }
            
            count = value;
            
            if (count <= 0)
            {
                count = 0;
                countText.gameObject.SetActive(false);
                markerObject.gameObject.SetActive(true);
            }
            else
            {
                countText.text = count.ToString();
            }
        }
    }
}