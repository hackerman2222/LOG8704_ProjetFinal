using UnityEngine;
using UnityEngine.Events;

public class PunchingBagCombo : MonoBehaviour
{
    [Header("Combo Settings")]
    [Tooltip("The time (in seconds) allowed between consecutive hits before the combo resets.")]
    public float comboResetTime = 0.8f; 

    [Header("Current Combo Data")]
    [Tooltip("The current number of consecutive hits.")]
    [SerializeField]
    private int currentCombo = 0;

    private float lastHitTime;

    // Pour update le text mesh
    [Header("Events")]
    public UnityEvent<int> OnComboUpdate; 

    void Start()
    {

        lastHitTime = Time.time;
        OnComboUpdate.Invoke(currentCombo); 
    }

    // appelle cette fonction quand un rigidboydy/Collider touche le sac
    private void OnCollisionEnter(Collision collision)
    {
    
        // remplace "PlayerGloveTag" avec le vrai noms du gant dans la scene
        if (!collision.gameObject.CompareTag("PlayerGloveTag")) 
        {
            return; // Ignore collisions les autres collisions
        }

        // Combo 
        float timeSinceLastHit = Time.time - lastHitTime;

        if (timeSinceLastHit > comboResetTime)
        {
            // reset le combo si pas assez rapide 
            currentCombo = 1; // Reset à 1 parceque c'est le coup qui compte en starter
            Debug.Log($" New Combo Started: {currentCombo}");
        }
        else
        {
            //continue le combo si le coup est assez rapide
            currentCombo++;
            Debug.Log($"Combo Count: {currentCombo}");
        }

        // --- Update ---
        lastHitTime = Time.time;
        
        OnComboUpdate.Invoke(currentCombo);
    }
}