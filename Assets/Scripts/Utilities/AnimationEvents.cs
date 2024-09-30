using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void DestorySelf() => Destroy(gameObject);

    public void Inactive() => gameObject.SetActive(false);
}
