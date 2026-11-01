using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Scatters placeholder stars around the camera and wraps them to the far side as the camera moves,
    /// so the field never runs out. One layer, no parallax, so star movement shows true ship speed.
    /// Runs after the camera rig has moved each frame.
    /// </summary>
    [DefaultExecutionOrder(100)]
    public sealed class PlaceholderStarfield : MonoBehaviour
    {
        [SerializeField] StarfieldData data;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Sprite starSprite;
        [SerializeField] int sortingOrder = -100;

        Transform[] stars;

        void Start()
        {
            var random = new System.Random(data.seed);
            Vector2 centre = cameraTransform.position;
            stars = new Transform[data.starCount];

            for (int i = 0; i < stars.Length; i++)
            {
                var star = new GameObject($"Star {i}");
                star.transform.SetParent(transform, worldPositionStays: false);
                star.transform.position = centre + new Vector2(
                    Lerp(random, -data.areaHalfExtent.x, data.areaHalfExtent.x),
                    Lerp(random, -data.areaHalfExtent.y, data.areaHalfExtent.y));
                star.transform.localScale = Vector3.one * Lerp(random, data.sizeRange.x, data.sizeRange.y);

                var spriteRenderer = star.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = starSprite;
                spriteRenderer.sortingOrder = sortingOrder;
                float brightness = Lerp(random, data.brightnessRange.x, data.brightnessRange.y);
                spriteRenderer.color = new Color(brightness, brightness, brightness, 1f);

                stars[i] = star.transform;
            }
        }

        void LateUpdate()
        {
            Vector2 centre = cameraTransform.position;
            Vector2 halfExtent = data.areaHalfExtent;
            Vector2 size = halfExtent * 2f;

            foreach (Transform star in stars)
            {
                Vector2 position = star.position;
                Vector2 offset = position - centre;

                // Move any star that has fallen off one edge to the matching spot on the opposite edge.
                if (offset.x > halfExtent.x) position.x -= size.x * Mathf.Ceil((offset.x - halfExtent.x) / size.x);
                else if (offset.x < -halfExtent.x) position.x += size.x * Mathf.Ceil((-halfExtent.x - offset.x) / size.x);
                if (offset.y > halfExtent.y) position.y -= size.y * Mathf.Ceil((offset.y - halfExtent.y) / size.y);
                else if (offset.y < -halfExtent.y) position.y += size.y * Mathf.Ceil((-halfExtent.y - offset.y) / size.y);

                star.position = position;
            }
        }

        static float Lerp(System.Random random, float min, float max) => Mathf.Lerp(min, max, (float)random.NextDouble());
    }
}
