using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Object")]
public class CharObject : ScriptableObject
{
    public string id;
    public MyBones parentBone;
    public GameObject m_prefab;
    public GameObject f_prefab;

}
