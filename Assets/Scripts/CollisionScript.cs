using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //change if number of cars change plz
        for(int i =8;i<16;i++){
            for(int ii =8; ii<16;ii++){
                if(ii>i){
                    Physics.IgnoreLayerCollision(i, ii);
                }
            }
        }
    }
}
