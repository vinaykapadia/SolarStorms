using SolarStorms.Simulation;
using UnityEngine;
using UnityEngine.Pool;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// The ship's beam, single burst only. Asks the fire timer each physics step and launches pooled bolts,
    /// stamping each with the damage and intensity its Weapons power gave it.
    /// </summary>
    public sealed class BeamWeapon : MonoBehaviour
    {
        [SerializeField] BeamWeaponData weapon;
        [SerializeField] PlayerShip ship;
        [SerializeField] PlayerInputReader input;
        [SerializeField] ShipReactor reactor;
        [SerializeField] Transform muzzle;
        [SerializeField] BeamBolt boltPrefab;

        BeamFireTimer fireTimer;
        ObjectPool<BeamBolt> boltPool;

        public BeamWeaponData Weapon => weapon;

        /// <summary>Seconds between shots at the current Weapons power. The debug readout uses this.</summary>
        public float CurrentInterval => fireTimer.CurrentInterval;

        /// <summary>Shots fired since the scene started. The debug readout watches this.</summary>
        public int ShotCount { get; private set; }

        /// <summary>Physics time of the most recent shot, in seconds.</summary>
        public float LastShotTime { get; private set; }

        public BeamBolt LastBolt { get; private set; }

        void Awake()
        {
            fireTimer = new BeamFireTimer(weapon, reactor.Model);
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

            ReactorModel model = reactor.Model;
            float weaponsMultiplier = model.Multiplier(ReactorSystem.Weapons);
            float damage = weapon.damageAtStandard * weaponsMultiplier;
            float intensity = Mathf.InverseLerp(model.Data.floorMultiplier, model.Data.capMultiplier, weaponsMultiplier);

            BeamBolt bolt = boltPool.Get();
            bolt.Launch(muzzle.position, ship.Flight.HeadingDegrees, velocity, weapon.boltLifetime, damage, intensity);

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
