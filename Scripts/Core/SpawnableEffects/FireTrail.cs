using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    //public static void Create(int damage, float tickrate, float lifeTime, float width, Vector3 center, List<string> affectedTags)
    //{
    //    GameObject newGKDrop = Instantiate(Resources.Load<GameObject>("SpawnableEffects/GKDrop"), center, Quaternion.identity);
    //    var gkDrop = newGKDrop.AddComponent<GKDrop>();
    //    gkDrop.Setup(damage, scale, affectedTags);
    //}

    //public static void Create(float gkAmount, float scale, Vector3 center, string affectedTag)
    //{
    //    var listDummy = new List<string>();
    //    listDummy.Add(affectedTag);
    //    Create(gkAmount, scale, center, listDummy);
    //}

    //private float _gkAmount;
    //private List<string> _affectedTags;

    //private void Setup(float gkAmount, float scale, List<string> affectedTags)
    //{
    //    _gkAmount = gkAmount;
    //    _affectedTags = affectedTags;
    //    transform.localScale *= scale;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!_affectedTags.Contains(other.tag))
    //        return;
    //    //Todo: Fill GK
    //    Destroy(gameObject);
    //}
}
