using System.Xml.Serialization;
using System.Net;
using System.Xml.Xsl;
using System.Transactions;
//using System.Threading.Tasks.Dataflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour {

    private TrackCheckpoints trackCheckpoints;


    private void OnTriggerEnter(Collider other) {
        Transform car =other.transform.parent.parent;
        if (car.TryGetComponent(out Player _)) {
            trackCheckpoints.CarThroughCheckpoint(this, car);
        }
    }


    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints) {
        this.trackCheckpoints = trackCheckpoints;
    }
}
