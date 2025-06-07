using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions
{
    public class RockMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter reference;
        [SerializeField] private Material[] materials;
        [SerializeField] private float noiseScale = 1f;
        [SerializeField] private float displacementStrength = 1f;
        [SerializeField] private float objectScale = 1f;

        private MeshFilter obj = null;
        private Vector3[] baseVertices;
        private Vector3[] baseNormals;

        private void Start()
        {
            GenerateMesh();
        }

        [ContextMenu("Generate Mesh")]
        public void GenerateMesh()
        {
            if (obj == null) GenerateObject();

            Vector3[] vertices = new Vector3[baseVertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 direction = baseNormals[i].normalized;
                float noise = Noise3D(direction.x * noiseScale + Time.time, direction.y * noiseScale + Time.time, direction.z * noiseScale + Time.time);
                noise = noise * 2f - 1f;
                vertices[i] = baseVertices[i] + direction * (noise * displacementStrength);
            }

            obj.mesh.vertices = vertices;
            obj.mesh.RecalculateNormals();
            obj.mesh.RecalculateBounds();
            
            obj.transform.localScale = Vector3.one * objectScale;
        }

        private void GenerateObject()
        {
            var go = new GameObject("RockMesh");
            go.transform.SetParent(transform, false);

            obj = go.AddComponent<MeshFilter>();
            obj.mesh = Instantiate(reference.sharedMesh);
            baseVertices = obj.mesh.vertices;
            baseNormals = obj.mesh.normals;
            
            var meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.materials = materials;
        }

        private float Noise3D(float x, float y, float z)
        {
            float xy = Mathf.PerlinNoise(x, y);
            float yz = Mathf.PerlinNoise(y, z);
            float xz = Mathf.PerlinNoise(x, z);
            float yx = Mathf.PerlinNoise(y, x);
            float zy = Mathf.PerlinNoise(z, y);
            float zx = Mathf.PerlinNoise(z, x);
            return (xy + yz + xz + yx + zy + zx) / 6f;
        }
    }
}