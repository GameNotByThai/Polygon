using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/MeshContainer")]
public class MeshContainer : ScriptableObject
{
    public string id;
    public Mesh m_mesh;
    public Mesh f_mesh;
    public Material material;
}
