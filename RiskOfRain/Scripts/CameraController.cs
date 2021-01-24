
using UnityEngine;

namespace RiskOfRain { 

    public class CameraController : MonoBehaviour{

        [SerializeField] float smoothTime;
        [SerializeField] Transform target;
        
        void LateUpdate(){

            Vector3 desiredPos = new Vector3(target.position.x, target.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothTime*Time.deltaTime);

        }

    }

}