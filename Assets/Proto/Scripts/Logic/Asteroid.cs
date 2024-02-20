using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    [ExecuteInEditMode]
    public class Asteroid : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int division;

        // [HideInInspector]
        public int divideNb;

        [SerializeField]
        private float originalScale = 13;

        [SerializeField]
        private AsteroidInfo Params;

        [SerializeField]
        private ScoreData ScoreData;

        [SerializeField]
        private GameObject _deathParticuleEffect;

        private float _timeToTeleportCounter;

        private Renderer _renderer;

        private Vector3 _debugDirection;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void OnEnable()
        {
            Vector2 direction = new Vector2(Mathf.Cos(Random.Range(0, 2 * Mathf.PI)), Mathf.Sin(Random.Range(0, 2 * Mathf.PI)));
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            rb2d.velocity = direction * Random.Range(0, Params.MaxSpeed);
            rb2d.angularVelocity = Random.Range(0, Params.MaxTorn);
            if (divideNb == 0)
            {
                // Debug.Log("Reseting scale");
                transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            }
            if (divideNb >= division)
            {
                // Debug.Log("Small asteroid");
                Params.SmallAsteroidCounter++;
            }
            else
            {
                // Debug.Log("Big asteroid");
                Params.BigAsteroidCounter++;
            }
        }
        private void OnDisable()
        {
            if (divideNb <= division && divideNb != 0)
            {
                Params.BigAsteroidCounter--;
                ScoreData.AddScore(Params.BigAsteroidScore);
            }

        }

        public void TakeDamage(DamageCauser damageCauser_, float damages_, Vector2 impactPoint_)
        {
            if (damageCauser_ == DamageCauser.Asteroid)
                return;
            // Asteroid can spawn childrens
            if (divideNb < division)
            {
                divideNb ++;
                if (divideNb <= division && Params.SmallAsteroidCounter < Params.MaxSmallAsteroidNb)
                {
                    for (int i = 0; i < division; i++)
                    {
                        // GameObject clone = Instantiate(gameObject);
                        GameObject clone = AsteroidPool.SharedInstance.GetPooledObject();
                        clone.SetActive(true);
                        clone.transform.position = transform.position;
                        clone.transform.localScale = transform.localScale;
                        clone.transform.localScale *= Params.DebrisScale; // TODO CHECK
                        Asteroid asteroid = clone.GetComponent<Asteroid>();
                        if (asteroid != null)
                        {
                            asteroid.divideNb = divideNb;
                        }
                    }
                }
            }
            // Asteroid can generate new asteroids
            else if (Random.Range(0, 4) == 2 && Params.BigAsteroidCounter < Params.MaxBigAsteroidNb || Params.MinBigAsteroidCount >= Params.BigAsteroidCounter)
            {
                ScoreData.AddScore(Params.GetAsteroidScore(divideNb));
                divideNb = 0;
                // GameObject clone = Instantiate(gameObject);
                GameObject clone = AsteroidPool.SharedInstance.GetPooledObject();
                Asteroid asteroid = clone.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.divideNb = divideNb;
                } else
                {
                    Debug.Log("PTN DE TA MERE");
                }

                // TODO base spawning on render bounds
                clone.transform.position = Camera.main.transform.position;
                Vector2 startPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height * Random.Range(0.0f, 0.3f), 0));
                if (Random.Range(0, 2) > 0)
                {
                    clone.transform.position = new Vector2(startPoint.x, clone.transform.position.y);
                }
                else
                {
                    clone.transform.position = new Vector2(clone.transform.position.x + (clone.transform.position.x - startPoint.x), clone.transform.position.y);
                }
                if (Random.Range(0, 2) > 0)
                {
                    clone.transform.position = new Vector2(clone.transform.position.x, startPoint.y);
                }
                else
                {
                    clone.transform.position = new Vector2(clone.transform.position.x, (clone.transform.position.y - startPoint.y) + clone.transform.position.y);
                }
                Params.SmallAsteroidCounter--;
                clone.gameObject.SetActive(true);
            }
            Instantiate(_deathParticuleEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }

        private void Update()
        {
            _timeToTeleportCounter += Time.deltaTime;

            Camera mainCamera = Camera.main;
            // Using only vec2 to avoid z depth problem
            Vector2 PosToCam = mainCamera.transform.position - transform.position;
            bool IsNotInScreen = PosToCam.sqrMagnitude > new Vector2(mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize).sqrMagnitude;
            bool IsToFarFromSpaceship = PosToCam.sqrMagnitude > Mathf.Pow(Params.MaxDistanceToSpaceShip, 2);
            if (_timeToTeleportCounter > Params.DelayToTeleport && IsNotInScreen && IsToFarFromSpaceship)
            {
                Teleport(mainCamera, PosToCam);
            }
        }

        private void Teleport(Camera mainCamera, Vector2 PosToCam)
        {
            Vector2 CamPos = mainCamera.transform.position;
            Vector2 Destination = PosToCam.normalized;
            float DirectionRatio = Mathf.Abs(Destination.x / Destination.y);
            float teleportationOffset = _renderer.bounds.extents.magnitude + Params.TeleportationOffset;
            // Setting the correct size of the vector
            if (DirectionRatio > mainCamera.aspect)
            {
                Destination.x = mainCamera.orthographicSize * mainCamera.aspect + teleportationOffset;
                Destination.y = Mathf.Abs(Destination.x) / DirectionRatio;
            }
            else
            {
                Destination.y = mainCamera.orthographicSize + teleportationOffset;
                Destination.x = Mathf.Abs(Destination.y) * DirectionRatio;
            }

            Destination.x *= Mathf.Sign(PosToCam.x);
            Destination.y *= Mathf.Sign(PosToCam.y);

            _debugDirection = Destination.ToVector3();

            transform.position = (CamPos + Destination).ToVector3() + Vector3.forward * transform.position.z;
            _timeToTeleportCounter = 0.0f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(DamageCauser.Asteroid, 10.0f, collision.contacts[0].point);
            }
        }
        

        private void OnDrawGizmos()
        {
            Vector2 PosToCam = Camera.main.transform.position - transform.position;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(PosToCam.x, PosToCam.y, 0));
        }

        public void OnDrawGizmosSelected()
        {
            var r = GetComponent<Renderer>();
            if (r == null)
                return;
            var bounds = r.bounds;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(bounds.center, bounds.extents.magnitude);


            Camera mainCamera = Camera.main;

            // Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z)) *2.0f;
            float cameraHeight = mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            Gizmos.DrawWireCube(mainCamera.transform.position, new Vector2(cameraWidth, cameraHeight) * 2.0f);


            Gizmos.color = Color.red;
            Vector2 PosToCam = Camera.main.transform.position - transform.position;
            Gizmos.DrawLine(transform.position, transform.position + PosToCam.ToVector3());

            Gizmos.color = Color.green;
            PosToCam.Normalize();
            Vector2 CamPos = Camera.main.transform.position;
            PosToCam.x *= cameraWidth;
            PosToCam.y *= cameraHeight;

            Gizmos.DrawLine(CamPos, CamPos + PosToCam);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(CamPos, (CamPos.ToVector3() + _debugDirection + Vector3.forward * transform.position.z));

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(CamPos, Params.MaxDistanceToSpaceShip);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(mainCamera.transform.position, new Vector2(cameraWidth, cameraHeight) * 2.0f + Vector2.one * (Params.TeleportationOffset + _renderer.bounds.size.magnitude));
        }

    }
}