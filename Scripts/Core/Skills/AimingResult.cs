using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public struct AimingResult
    {
        Vector3 targetPos;
        GameObject targetObject;

        public bool GetPosition(out Vector3 result)
        {
            if (targetPos == null)
            {
                if (targetObject != null)
                {
                    result = targetObject.transform.position;
                    return true;
                }
                else
                {
                    result = Vector3.zero;
                    return false;
                }

            }
            result = targetPos;
            return true;
        }

        public bool GetObject(out GameObject result)
        {
            if (targetObject == null)
            {
                result = null;
                return false;
            }
            result = targetObject;
            return true;
        }
    }
}




