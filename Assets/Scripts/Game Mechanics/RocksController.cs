using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Mechanics
{
    public class RocksController : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshReference;
        [SerializeField] private Transform rocksParent;
        [SerializeField] private GameObject[] rockPrefabs;
        [SerializeField] private Vector3 scaleMin, scaleMax;
        [SerializeField] private int generateCount = 20;
        
        private void Start()
        {
            GenerateRocks();
        }

        private void GenerateRocks()
        {
            var verts = meshReference.sharedMesh.vertices;
            var normals = meshReference.sharedMesh.normals;

            var indexes = Enumerable.Range(0, verts.Length).OrderBy(_ => Random.value).Take(generateCount);

            foreach (var i in indexes)
            {
                var randPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                var rock = Instantiate(randPrefab);
                rock.transform.position = GetPosition(verts[i], meshReference.transform.localScale);
                rock.transform.forward = normals[i];
                rock.transform.SetParent(rocksParent, false);
                rock.transform.localScale = new Vector3(
                    Random.Range(scaleMin.x, scaleMax.x),
                    Random.Range(scaleMin.y, scaleMax.y),
                    Random.Range(scaleMin.z, scaleMax.z)
                );
            }
        }

        private Vector3 GetPosition(Vector3 verticePos, Vector3 localScale)
        {
            return new Vector3(
                    verticePos.x * localScale.x,
                    verticePos.y * localScale.y,
                    verticePos.z * localScale.z
            );
        }
    }
}