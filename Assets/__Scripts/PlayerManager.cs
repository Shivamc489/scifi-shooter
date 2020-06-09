using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private bool hasOpened = false;
    public Animator[] animators;
    private CharacterController _controller;
    public Transform cam;

    public float _speed = 5f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Actions actions;
    public GameObject panel1,panel2;

    float health = 100f;
    public Text healthText,enemyText;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = this.GetComponent<CharacterController>();
        actions.Stay();
    }    

    private void Update() {
        health = Mathf.Clamp(health, 0f, 100f);
        healthText.text = "Health : "+health.ToString("F0");
        int enemyLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyLeft == 0)
        {
            panel2.SetActive(true);
            panel1.SetActive(false);
        }
        enemyText.text = "Enemies : "+enemyLeft.ToString("F0");
        if(health <= 0f)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Debug.Log(horizontal + " " + vertical);
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _speed = 5f;
                actions.Run();
            }
            else
            {
                _speed = 2.5f;
                actions.Walk();
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            int a = 1;
            if(vertical < 0)
                a=-1;
            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * a;
            _controller.Move(movDir.normalized * _speed * Time.deltaTime);
        }
        else
        {
            actions.Stay();
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if(Input.GetMouseButton(0))
        {
            actions.Attack();
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Truigger entered : "+other.name);
        if(hasOpened)
        {
            return;
        }
        if(other.tag == "floor")
        {
            Debug.Log("Player trigerred");
            hasOpened = true;
            foreach (var x in animators)
            {
                x.Play("open");
            }
        }
    }

    public void DecreaseHealth(float hitpoints = 10f)
    {
        health -= hitpoints;
        if(health <= 0f)
        {
            actions.Death();
        }
        // else
        // {
        //     actions.Damage();
        // }
    }
}