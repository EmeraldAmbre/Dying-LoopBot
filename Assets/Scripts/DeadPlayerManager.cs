using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayerManager : MonoBehaviour {

    private Rigidbody2D _rigidbody;

    void Awake() {

        _rigidbody = GetComponent<Rigidbody2D>();

    }


    public void Freeze() {

        _rigidbody.bodyType = RigidbodyType2D.Kinematic;

    }

}
