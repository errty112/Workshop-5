// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f; // Default speed sensitivity
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireCooldown = 0.1f; // Rate of Fire

    private float currentCooldown = 0.0f;
    private MeshRenderer _renderer;

    private void Awake()
    {
        this._renderer = gameObject.GetComponent<MeshRenderer>();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * (this.speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * (this.speed * Time.deltaTime));
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    
        
        // Use the "down" variant to avoid spamming projectiles. Will only get
        // triggered on the frame where the key is initially pressed.
        if ((Input.GetMouseButton(0) && currentCooldown <= 0) || Input.GetMouseButtonDown(0))
        {
            Shoot();
            currentCooldown = fireCooldown;
        }
    }

    private void Shoot()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.up, transform.position);

        if(plane.Raycast(ray, out float hitDistance))
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Vector3 fireDirection = (targetPoint - transform.position).normalized;

            var projectile = Instantiate(this.projectilePrefab);
            projectile.transform.position = gameObject.transform.position;
            projectile.GetComponent<ProjectileController>().velocity = 25*fireDirection;
        }


        
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public void UpdateHealth(float frac)
    {
        //this._renderer.material.color = Color.blue * frac;
    }

}
