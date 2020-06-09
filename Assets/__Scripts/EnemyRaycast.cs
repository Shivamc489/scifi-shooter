using UnityEngine;
using System.Collections;

public class EnemyRaycast : MonoBehaviour {

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f; 
    public float hitForce = 100f; 
    Transform gunEnd;  
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio; 
    private LineRenderer laserLine;   
    private float nextFire;  
    Transform player;


    void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        laserLine = GetComponent<LineRenderer>();
        // gunAudio = GetComponent<AudioSource>();
        GameObject[] gunEnds = GameObject.FindGameObjectsWithTag("EnemyGunEnd");
        foreach (var x in gunEnds)
        {
            if(x.transform.parent.name == this.gameObject.name)
            {
                gunEnd = x.transform;
                return;
            }
        }
    }


    public void Shoot() 
    {
        nextFire = Time.time + fireRate;
            
        Vector3 rayOrigin = gunEnd.position;
        RaycastHit hit;
        laserLine.SetPosition (0, gunEnd.position);

        // Check if our raycast has hit anything
        float xRandom = Random.Range(-0.03f,0.03f);
        float yRandom = Random.Range(-0.03f,0.03f);
        float zRandom = Random.Range(-0.03f,0.03f);
        Vector3 fpsFront = gunEnd.forward /*(player.transform.position - gunEnd.transform.position).normalized*/;
        fpsFront.x += xRandom;
        fpsFront.y += yRandom;
        fpsFront.z += zRandom;
        if (Physics.Raycast (rayOrigin, fpsFront, out hit, weaponRange))
        {
            Debug.Log(hit.transform.name);
            // Set the end position for our laser line 
            laserLine.SetPosition (1, hit.point);

            if(hit.transform.tag == "Player")
            {
                Debug.Log("Player Hit");
                PlayerManager enemyManager = hit.transform.parent.gameObject.GetComponent<PlayerManager>();
                enemyManager.DecreaseHealth(0.1f);
            }

            // Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                // Add force to the rigidbody we hit, in the direction from which it was hit
                hit.rigidbody.AddForce (-hit.normal * hitForce);
            }
        }
        else
        {
            // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
            laserLine.SetPosition (1, /*rayOrigin +*/ fpsFront * weaponRange);
        }
            
        StartCoroutine (ShotEffect());
        
    }


    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        // gunAudio.Play ();

        yield return new WaitForSeconds(0.5f);

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}