using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

}
