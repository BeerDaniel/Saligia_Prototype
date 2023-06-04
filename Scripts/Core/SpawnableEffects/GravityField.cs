using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{

    public static void Create(float force, float gravityRadius, Vector3 center, List<string> affectedTags, Transform parentTransform = null)
    {
        GameObject newGravityField = new GameObject("GravityField");
        newGravityField.transform.position = center;
        if (parentTransform != null)
            newGravityField.transform.parent = parentTransform;

        var gravityField = newGravityField.AddComponent<GravityField>();
        gravityField.Setup(force, gravityRadius, affectedTags);
    }

    public static void Create(float force, float gravityRadius, Vector3 center, string affectedTag, Transform parentTransform = null)
    {
        var listDummy = new List<string>();
        listDummy.Add(affectedTag);
        Create(force, gravityRadius, center, listDummy, parentTransform);
    }

    private float _force;
    private List<string> _affectedTags;

    private void Setup(float force, float gravityRadius, List<string> affectedTags)
    {
        _force = force;
        _affectedTags = affectedTags;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        var sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = gravityRadius;
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_affectedTags.Contains(other.tag))
            return;
        if (!other.TryGetComponent(out HealthComponent healthComponent))
            return;
        Rigidbody rigidbody = healthComponent.GetComponent<Rigidbody>(); // By Definition the Collider other MUST have a rigidbody to fire Trigger
        Vector3 centerVectorNormalised = (transform.position - healthComponent.transform.position).normalized;
        Vector3 forceVector = new Vector3(centerVectorNormalised.x, 0, centerVectorNormalised.z) * _force * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.transform.position + forceVector);
    }
}
