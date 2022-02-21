using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "expose/transform")]
public class AccessTransform : ScriptableObject
{
	public Transform target{get; set;}
}
