using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public static bool isMobile;
    public bool mobileTest;
    public Joystick joystick, directionJoystick;
    public Transform orb;
    public float moveSpeed = 1;
    private float timeSpeed;
    public ParticleSystem collideParticle;
    public float smoothness = 0.05f;
    public float brakeTime = 1;
    [Space(10)]
    [Header("Boost")]
    public bool canBoost;
    public ParticleSystem boostParticle;
    public float BoostRechargeTime = 3;
    public float BoostStrength = 50;
    public Image image;
    private float currentTime, holdingTime = 3;
    public Shoot shoot;
    private int numberOfFingers;
    [Space(10)]
    [Header("Shockwave")]
    public bool canShockwave;
    public float explosionRadius = 5f;  // Adjust the radius of the shockwave.
    public float explosionDamage = 10f;
    private Rigidbody rb;
    [SerializeField] private bool canMove;
    private Vector3 movement;
    public GameObject[] UIs;
    private bool arrowPressed;

    private float firerate;
    public static bool canControl;
    private void Awake()
    {
        canControl = true;
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        for (int i = 0; i < UIs.Length; i++)
            UIs[i].SetActive(isMobile || mobileTest);

        UIs[UIs.Length - 1].SetActive(!isMobile);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameOver) return;
        Movement();
        CanonFilling();
        RotatePlayerTowardsMouse();

        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
            ShootBullet();

        if (canShockwave)
        {
            if (numberOfFingers == 2 || Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                holdingTime -= Time.deltaTime;
                if (holdingTime <= 0)
                {
                    holdingTime = 10;
                    ShockWave();
                }
            }
            else
                holdingTime = 3;
        }

        if (canBoost)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
                SpeedBoost();
        }

        // shoot.DrawLine();
    }

    void Movement()
    {
        float horizontalInput;
        float verticalInput;
        if (isMobile || mobileTest)
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }


        // Calculate the movement vector
        movement = new Vector3(horizontalInput, 0, verticalInput);

        if (movement != Vector3.zero && canMove)
        {
            timeSpeed += Time.deltaTime;
            if(rb.velocity.magnitude > moveSpeed * 3)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, movement * moveSpeed, smoothness);
            }
            else
                rb.velocity = movement * moveSpeed * timeSpeed;

            // direction
            //if(!arrowPressed)
            //{
                float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveSpeed / 10);
            //}
            
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 1/(brakeTime*100));
            timeSpeed = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SpeedBoost();

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            currentTime = 0;
        image.fillAmount = (BoostRechargeTime - currentTime)/BoostRechargeTime;
    }

    void RotatePlayerTowardsMouse()
    {
        float horizontalInput;
        float verticalInput;
        if (isMobile || mobileTest)
        {
            horizontalInput = directionJoystick.Horizontal;
            verticalInput = directionJoystick.Vertical;
        }
        else
        {
            horizontalInput = Input.GetAxis("ArrowHorizontal");
            verticalInput = Input.GetAxis("ArrowVecrtical");
        }

        arrowPressed = (horizontalInput != 0 || verticalInput != 0);
        if (arrowPressed)
        {
            Vector2 direction = new Vector2(-horizontalInput, verticalInput);
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle - 90, 0);
            orb.rotation = Quaternion.Slerp(orb.rotation, targetRotation, moveSpeed);

            firerate += Time.deltaTime;
            if (firerate > 0.5f)
            {
                firerate = 0;
                ShootBullet();
            }
        }
        else
            firerate = 0.3f;
    }

    public void SpeedBoost()
    {
        if(currentTime <= 0)
        {
            currentTime = BoostRechargeTime;
            rb.AddForce(transform.forward * BoostStrength, ForceMode.Impulse);
            boostParticle.Play();
        }
    }

    public void ShootBullet()
    {
        if(shoot.canonSlider.value >= 5)
        {
            shoot.canonSlider.value -= shoot.shootCostPerSecond;
            shoot.shootParticles[0].Play();

        }
    }

    public void CanonFilling()
    {
        shoot.canonSlider.value += Time.deltaTime * shoot.canonFillRate;
        if (shoot.canonSlider.value > shoot.canonSlider.maxValue)
            shoot.canonSlider.value = shoot.canonSlider.maxValue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            Stop();
            GetComponent<Health>().HealthUpdate(5);
            rb.AddForce(-transform.forward * 20 / (rb.velocity.magnitude + 0.01f));
            if(rb.velocity.magnitude >= 10)
                GetComponent<Health>().HealthUpdate(100);
        }
    }

    public void CanonRecharge(float val)
    {
        shoot.canonSlider.value += val;
        if(shoot.canonSlider.value > 100)
            shoot.canonSlider.value = 100;
    }

    public async void ShockWave(float time = 0.5f)
    {
        shoot.shootParticles[1].Play();
        await WaitAsync(time);
        // Find all colliders within the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            Health health = collider.GetComponent<Health>();

            if (health != null)
            {
                if(!health.isPlayer)
                    health.HealthUpdate(explosionDamage);
            }
        }
    }

    

    public async void Stop(float time = 0.5f)
    {
        canMove = false;
        await WaitAsync(time);
        canMove = true;
    }

    public void updateFinger(int val)
    {
        numberOfFingers += val;
    }

    public static Task WaitAsync(float seconds)
    {
        return Task.Delay((int)(seconds * 1000));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    [System.Serializable]
    public class Shoot
    {
        public ParticleSystem[] shootParticles;
        public Slider canonSlider;
        public float shootCostPerSecond = 5, canonFillRate = 5;
        public LineRenderer line;
        public float lineRange = 50;
        public LayerMask layer;
        RaycastHit hit;
        private float lineCurrentRange;
        public void DrawLine()
        {
            // Cast a ray from the object's position in the forward direction.
            Ray ray = new Ray(shootParticles[0].transform.position, shootParticles[0].transform.forward);

            // Create a RaycastHit variable to store information about the hit.


            // Perform the raycast.
            if (Physics.Raycast(ray, out hit, lineRange))
            {
                lineCurrentRange = Vector3.Distance(shootParticles[0].transform.position, hit.point);
                line.SetPosition(1, shootParticles[0].transform.forward * lineCurrentRange);
                if (hit.transform.gameObject.layer == layer)
                {
                    line.startColor = Color.green;
                    line.endColor = Color.green;
                }
            }
            else
            {
                line.startColor = Color.red;
                line.endColor = Color.red;
                line.SetPosition(1, shootParticles[0].transform.forward * lineRange);
            }
        }
    }
}
