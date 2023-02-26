using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ParentTransfer : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<Transform> parentOfTarget;
    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private Transform container;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targets.Contains(other.transform) == false) 
            {
                targets.Add(other.transform);
                parentOfTarget.Add(other.transform.parent);
                other.transform.parent = container;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targets.Contains(other.transform))
            {
                Transform[] temp = targets.ToArray();
                int index = System.Array.IndexOf(temp, other.transform);
                targets.Remove(other.transform);
                other.transform.parent = parentOfTarget[index];
                parentOfTarget.RemoveAt(index);
            }
        }
    }
}
