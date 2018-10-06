using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    [SerializeField] Transform _pathParentHomeToDoc;
    [HideInInspector] public List<Transform> PathsHomeToDoc = new List<Transform>();

    [SerializeField] Transform _pathParentDocToBed;
    [HideInInspector] public List<Transform> PathsDocToBed = new List<Transform>();

    [SerializeField] Transform _pathParentBedToExit;
    [HideInInspector] public List<Transform> PathsBedToExit = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PathsHomeToDoc = _pathParentHomeToDoc.GetComponentsInChildren<Transform>().ToList();
        if (PathsHomeToDoc[0] == _pathParentHomeToDoc)
            PathsHomeToDoc.Remove(PathsHomeToDoc[0]);

        PathsDocToBed = _pathParentDocToBed.GetComponentsInChildren<Transform>().ToList();
        if (PathsDocToBed[0] == _pathParentDocToBed)
            PathsDocToBed.Remove(PathsDocToBed[0]);

        PathsBedToExit = _pathParentBedToExit.GetComponentsInChildren<Transform>().ToList();
        if (PathsBedToExit[0] == _pathParentBedToExit)
            PathsBedToExit.Remove(PathsBedToExit[0]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < PathsHomeToDoc.Count; i++)
        {
            if (i < PathsHomeToDoc.Count - 1)
                Gizmos.DrawLine(PathsHomeToDoc[i].position, PathsHomeToDoc[i + 1].position);
        }

        Gizmos.color = Color.red;

        for (int i = 0; i < PathsDocToBed.Count; i++)
        {
            if (i < PathsDocToBed.Count - 1)
                Gizmos.DrawLine(PathsDocToBed[i].position, PathsDocToBed[i + 1].position);
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < PathsBedToExit.Count; i++)
        {
            if (i < PathsBedToExit.Count - 1)
                Gizmos.DrawLine(PathsBedToExit[i].position, PathsBedToExit[i + 1].position);
        }
    }
}
