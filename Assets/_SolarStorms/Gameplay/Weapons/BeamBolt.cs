using System;
using UnityEngine;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// One beam bolt. Flies in a straight line and returns to its pool when its lifetime ends.
    /// Step 1 has no damage or collisions.
    /// </summary>
    public sealed class BeamBolt : MonoBehaviour
    {
        Action<BeamBolt> returnToPool;
        Vector2 velocity;
        float lifetime;

        public float Age { get; private set; }
        public float DistanceTravelled { get; private set; }
        public float Speed => velocity.magnitude;

        public void SetPoolReturn(Action<BeamBolt> returnToPool)
        {
            this.returnToPool = returnToPool;
        }

        public void Launch(Vector2 position, float headingDegrees, Vector2 velocity, float lifetime)
        {
            transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, headingDegrees));
            this.velocity = velocity;
            this.lifetime = lifetime;
            Age = 0f;
            DistanceTravelled = 0f;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            transform.position += (Vector3)(velocity * deltaTime);
            Age += deltaTime;
            DistanceTravelled += Speed * deltaTime;

            if (Age >= lifetime)
            {
                returnToPool(this);
            }
        }
    }
}
