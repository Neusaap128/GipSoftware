
using UnityEngine;

namespace RiskOfRain {

    public class Arrow : PlayerBullet
    {

        public Transform target;
        [SerializeField] float anglularMomentum;

        void FixedUpdate(){

            transform.position += transform.up * speed * Time.deltaTime;

            if (target == null)
                return;

            Vector2 dirToTarget = (target.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.up, dirToTarget);
            transform.eulerAngles = new Vector3(0, 0,
                                                transform.rotation.z - (angle*anglularMomentum*Mathf.Deg2Rad*Time.deltaTime));

        }

    }

}