using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public static void Create(int explosionDamage, float explosionRadius, Vector3 center, List<string> affectedTags)
    {
        GameObject newExplosion = new GameObject("Explosion");
        newExplosion.transform.position = center;
        var explosion = newExplosion.AddComponent<Explosion>();
        explosion.Setup(explosionDamage, explosionRadius, affectedTags);
    }

    public static void Create(int explosionDamage, float explosionRadius, Vector3 center, string affectedTag)
    {
        var listDummy = new List<string>();
        listDummy.Add(affectedTag);
        Create(explosionDamage, explosionRadius, center, listDummy);
    }

    private int _explosionDamage;
    private List<string> _affectedTags;
    private List<Entity> _affectedEntities;
    private int _lastSize = int.MaxValue;
    private DamageData _explosionDamageData;

    private void Setup(int explosionDamage, float explosionRadius, List<string> affectedTags)
    {
        _explosionDamage = explosionDamage;
        _affectedTags = affectedTags;
        var sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = explosionRadius;
        sphereCollider.isTrigger = true;
        _affectedEntities = new List<Entity>();

        _explosionDamageData.damageAmount = explosionDamage;
        _explosionDamageData.forceStagger = true;
        _explosionDamageData.canStagger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_affectedTags.Contains(other.tag))
            return;
        if (!other.TryGetComponent(out Entity entity))
            return;
        _affectedEntities.Add(entity);
    }

    private void FixedUpdate()
    {
        if (_lastSize != _affectedEntities.Count)
        {
            _lastSize = _affectedEntities.Count;
            return;
        }

        foreach (var entity in _affectedEntities)
            entity.ApplyDamage(new DamageData(_explosionDamageData));

        Destroy(gameObject);

    }
}