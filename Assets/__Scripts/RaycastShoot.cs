using UnityEngine;
using System.Collections;

public class RaycastShoot : MonoBehaviour {

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f; 
    public float hitForce = 100f; 
    Transform gunEnd;        
    public Camera fpsCam;     
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio; 
    private LineRenderer laserLine;   
    private float nextFire;  
    public Transform toShootAt;

    
    void Start () 
    {
        laserLine = GetComponent<LineRenderer>();
        // gunAudio = GetComponent<AudioSource>();
        gunEnd = GameObject.FindGameObjectWithTag("GunEnd").GetComponent<Transform>();
    }


    void Update () 
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) 
        {
            nextFire = Time.time + fireRate;
            
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            laserLine.SetPosition (0, gunEnd.position);

            // Check if our raycast has hit anything
            float xRandom = Random.Range(-0.01f,0.01f);
            float yRandom = Random.Range(-0.01f,0.01f);
            float zRandom = Random.Range(-0.01f,0.01f);
            Vector3 fpsFront = fpsCam.transform.forward;
            // fpsFront.x += xRandom;
            // fpsFront.y += yRandom;
            // fpsFront.z += zRandom;
            if (Physics.Raycast (rayOrigin, fpsFront, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition (1, hit.point);

                if(hit.transform.tag == "Enemy")
                {
                    EnemyManager enemyManager= hit.transform.gameObject.GetComponent<EnemyManager>();
                    enemyManager.DecreaseHealth(100f);
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