
using UnityEngine;

namespace DonkeyKong{

    public class BarrelWaypoint : MonoBehaviour
    {

        public BarrelWaypoint[] nextWaypoints = new BarrelWaypoint[0];

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            for (int i = 0; i < nextWaypoints.Length; i++)
            {
                Gizmos.DrawLine(transform.position, nextWaypoints[i].transform.position);
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }

    }
}