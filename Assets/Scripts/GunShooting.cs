using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int damage;
    private float timer = 0;
    [SerializeField]
    private Transform firePoint;
    public GameObject bullet;
    public float bulletSpeed;
    int bulletsLeft, bulletsShot;
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    bool shooting, readyToShoot, reloading;
    public bool Reloading { get { return reloading; } }
    public bool IsShooting { get; private set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            timer = 0;
            if (Input.GetButton("Fire1"))
            {
                
                GameObject BulletInst = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation) as GameObject;
                Rigidbody bulletInstRigid = BulletInst.GetComponent<Rigidbody>();
                bulletInstRigid.AddForce(BulletInst.transform.forward * bulletSpeed);


                

                /*Ray ray = new Ray(firePoint.position, firePoint.forward);
                 RaycastHit hitinfo;

                 //raycast
                 if(Physics.Raycast(ray,out hitinfo,100.0f))
                 {
                     var health = hitinfo.collider.GetComponent<enemy>();
                     if(health !=null)
                     {
                         health.TakeDamage(damage);

                     }
                 }*/
            }
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


        }
    }
    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
      //  reloadAudio.Play();
    }

    void ReloadFinished()
    {

        bulletsLeft = magazineSize;
        reloading = false;
       // reloadAudio.Stop();
    }
}
