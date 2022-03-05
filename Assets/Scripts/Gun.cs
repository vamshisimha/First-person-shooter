
using UnityEngine;

public class Gun : MonoBehaviour
{
  
    [Header("Gun Properites")]
    public GameObject muzzlePoint;
    [SerializeField]
    private Transform player;

    //bullet 
    public GameObject bullet;
    public bool useBullet = true;
    public bool useRayCastToDamage = true;
    public float bulletForce = 10f;
    [Header("Audio")]
    public AudioSource shootAudio;
    public AudioSource reloadAudio;

    //Gun stats
    [Header("Gun Stats")]
    public float gunDamage = 20f;
    public float gunRange = 2f;
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public LayerMask enemyLayer;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Graphics
    public GameObject muzzleFlash;

    public bool allowInvoke = true;

    public bool useDebug = false;

    private float effectsDisplayTime = 0.5f;
    private float timer;
    private ParticleSystem flash;
    private LineRenderer gunRenderer;
    private GameObject muzzleFlashInstance;
    private RaycastHit hit;
    
    private Ray ray;

    public bool Reloading { get { return reloading; } }
    public bool IsShooting { get; private set; }


    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        gunRenderer = GetComponentInChildren<LineRenderer>();
        muzzleFlashInstance = Instantiate(muzzleFlash, transform);
        flash = muzzleFlashInstance.GetComponent<ParticleSystem>();
       
    }
    private void Update()
    {
        timer += Time.deltaTime;
        GetInput();

        if (timer >= timeBetweenShooting * effectsDisplayTime)
        {
            gunRenderer.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        if (useDebug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(muzzleFlash.transform.localPosition, player.transform.forward * gunRange);
            //Gizmos.DrawRay(ray);
        }
    }


    

    private void GetInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetMouseButtonDown(0);
        else shooting = Input.GetMouseButton(0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            IsShooting = false;
            Reload();
        }
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            IsShooting = false;
            Reload();
        }

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }

    }
    void Shoot()
    {
        readyToShoot = false;
        timer = 0;
        //Find the exact hit position using a raycast
        ray = new Ray(muzzlePoint.transform.position, player.transform.forward);

        //Audio
        shootAudio.Stop();
        shootAudio.Play();

        gunRenderer.enabled = true;
        gunRenderer.SetPosition(0, muzzlePoint.transform.position);
        if (Physics.Raycast(ray, out hit, gunRange, enemyLayer.value))
        {
            //Debug.DrawRay(ray.origin, ray.direction * gunRange, Color.green);
            gunRenderer.SetPosition(1, hit.point);
            //Debug.Log(hit.transform.name);
            if (useRayCastToDamage)
            {
                DealDamage();
            }

        }
        else
        {
            //Debug.DrawRay(ray.origin, ray.direction * gunRange, Color.red);
            gunRenderer.SetPosition(1, ray.origin + ray.direction * gunRange);
        }


        if (useBullet)
        {
            //Instantiate bullet/projectile
            GameObject currentBullet = Instantiate(bullet, muzzlePoint.transform.position, Quaternion.identity); //store instantiated bullet in currentBullet
            //currentBullet.GetComponent<Bullet>().ApplyForce(player.transform.forward);
        }


        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
        {
            muzzleFlashInstance.transform.position = muzzlePoint.transform.position;
            flash.Play();
        }


        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
        reloadAudio.Play();
    }

    void ReloadFinished()
    {

        bulletsLeft = magazineSize;
        reloading = false;
        reloadAudio.Stop();
    }


    void DealDamage()
    {

        //hit.transform.GetComponent<HealthManager>().TakeDamage(gunDamage, hit.normal, bulletForce);
        //Debug.Log(hit.transform.name);

    }


   
}
