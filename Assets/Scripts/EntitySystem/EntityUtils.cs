using UnityEngine;

namespace EntitySystem
{
    public class EntityUtils
    {
        public static TEntity Instantiate<TEntity>(GameObject prefab, Vector3 position, Quaternion rotation)
            where TEntity : IEntity
        {
            var go = Object.Instantiate(prefab, position, rotation);
            if (go.TryGetComponent(out TEntity entity))
            {
                return entity;
            }

            Debug.LogError($"{prefab} is not an entity of type '{typeof(TEntity)}'", prefab);
            Object.Destroy(go);
            return default;
        }
    }
}