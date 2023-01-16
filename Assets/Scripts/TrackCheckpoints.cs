using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler<CarCheckpointEventArgs> OnPlayerCorrectCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnPlayerWrongCheckpoint;

     public List<Transform> carTransformList;

    public class CarCheckpointEventArgs : EventArgs {
        public Transform eventArgsCarTransform;
    }


    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    
    private void Awake() {
        GameObject sceneObjects = GameObject.Find("SceneObjects");
        Transform checkpointsTransform = sceneObjects.transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList) {
            nextCheckpointSingleIndexList.Add(0);
        }
        
    }  
      

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform) {

        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            UnityEngine.Debug.Log("Correct");

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            OnPlayerCorrectCheckpoint?.Invoke(this, new CarCheckpointEventArgs{eventArgsCarTransform = carTransform});
        } else {
            // Wrong checkpoint
            UnityEngine.Debug.Log("Wrong");

        OnPlayerWrongCheckpoint?.Invoke(this, new CarCheckpointEventArgs{eventArgsCarTransform = carTransform});

        }
    }

public void ResetCheckpoint(Transform carTransform){
    nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] =0;
}

public CheckpointSingle GetNextCheckpoint(Transform carTransform){
    return checkpointSingleList[nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]];
}

}
