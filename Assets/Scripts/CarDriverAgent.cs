using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.IO.Enumeration;
//using System.Threading.Tasks.Dataflow;

public class CarDriverAgent : Agent
{
    
    private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;
     private CarController carController;
    
    private void Awake(){
        carController = GetComponent<CarController>();
        GameObject sceneObjects = GameObject.Find("SceneObjects");
        trackCheckpoints = sceneObjects.GetComponent<TrackCheckpoints>();
    }

    private void Start(){
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnPlayerWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }



    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender,TrackCheckpoints.CarCheckpointEventArgs e){
        if(e.eventArgsCarTransform == transform){
            AddReward(1f);
        }
    }
    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender,TrackCheckpoints.CarCheckpointEventArgs e){
        if(e.eventArgsCarTransform == transform){
            AddReward(-1f);
        }
    }

    public override void OnEpisodeBegin() {
        
        transform.position = spawnPosition.position + new Vector3(Random.Range(-5f,+5f),0,0);
        transform.forward = spawnPosition.forward;
        trackCheckpoints.ResetCheckpoint(transform);
        //car jolts around when respawining because of retention of speed from collision. solution below is supposed to work but does not. fix.
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    public override void CollectObservations(VectorSensor sensor) {
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward,checkpointForward);
        sensor.AddObservation(directionDot);
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = actions.ContinuousActions[0];
        float turnAmount = actions.ContinuousActions[1];
        
        // switch (actions.continuousActions[0]){
        //     case 0: forwardAmount =0f; break;
        //     case 1: forwardAmount =+1f; break;
        //     case 2: forwardAmount =-1f; break;
        // }
        // switch (actions.continuousActions[1]){
        //     case 0: turnAmount = 0f; break;
        //     case 1: turnAmount = +1f; break;
        //     case 2: turnAmount = -1f; break;
        // }
        
        carController.Move(turnAmount,forwardAmount,0f,0f);
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        int forwardAction =0;
        if(Input.GetKey(KeyCode.W)) forwardAction =1;
        if(Input.GetKey(KeyCode.S)) forwardAction =2;
        int turnAction =0;
        if(Input.GetKey(KeyCode.D)) turnAction =1;
        if(Input.GetKey(KeyCode.A)) turnAction =2;

        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = forwardAction;
        continuousActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.TryGetComponent(out Wall _)) {
            AddReward(-0.5f);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            EndEpisode();
        }
    }
    private void OnCollisionStay(Collision collision){
        if(collision.gameObject.TryGetComponent(out Wall _)) {
            AddReward(-0.1f);
            
        }
    }
}
