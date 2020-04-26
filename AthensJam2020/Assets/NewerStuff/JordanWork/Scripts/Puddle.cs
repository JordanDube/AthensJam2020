using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour {
    
    public bool isInfinite = false;
    public float water = 12f;

    private float maxWater;
    private Animator _Animator;
    
    private void Awake() {
        _Animator = GetComponent<Animator>();
        maxWater = water;
    }

    private void Update() {
        if (water < 4f) {
            _Animator.SetBool("isLow", true);
        } else if (water < 0f) {
            _Animator.SetBool("isEmpty", true);
        }
    }
    
}
