using SolarStorms.Simulation;
using UnityEngine;
using UnityEngine.Pool;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// The ship's beam, single burst only. Asks the fire timer each physics step and launches pooled bolts.
    /// </summary>
    public sealed class BeamWeapon : MonoBehaviour
    {
        [SerializeField] BeamWeaponData weapon;
        [SerializeField] PlayerShip ship;
        [SerializeField] PlayerInputReader input;
        [SerializeField] Transform muzzle;
        [SerializeField] BeamBolt boltPrefab;

        BeamFireTimer fireTimer;
        ObjectPool<BeamBolt> boltPool;

        public BeamWeaponData Weapon => weapon;

        /// <summary>Shots fired since the scene started. The debug readout watches this.</summary>
        public int ShotCount { get; private set; }

        /// <summary>Physics time of the most recent shot, in seconds.</summary>
        public float LastShotTime { get; private set; }

        public BeamBolt LastBolt { get; private set; }

        void Awake()
        {
            fireTimer = new BeamFireTimer(weapon);
            boltPool = new ObjectPool<BeamBolt>(
                createFunc: CreateBolt,
                actionOnGet: bolt => bolt.gameObject.SetActive(true),
                actionOnRelease: bolt => bolt.gameObject.SetActive(false),
                actionOnDestroy: bolt => Destroy(bolt.gameObject));
        }

        void FixedUpdate()
        {
            if (!fireTimer.Tick(input.FireHeld, Time.fixedDeltaTime))
            {
                return;
            }

            Vector2 velocity = ship.Flight.Forward * weapon.boltSpeed;
            if (weapon.inheritShipVelocity)
            {
                velocity += ship.Flight.Velocity;
            }

            BeamBolt bolt = boltPool.Get();
            bolt.Launch(muzzle.position, ship.Flight.HeadingDegrees, velocity, weapon.boltLifetime);

            ShotCount++;
            LastShotTime = Time.fixedTime;
            LastBolt = bolt;
        }

        BeamBolt CreateBolt()
        {
            BeamBolt bolt = Instantiate(boltPrefab);
            bolt.SetPoolReturn(boltPool.Release);
            return bolt;
        }
    }
}
