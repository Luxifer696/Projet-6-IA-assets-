using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public string m_TankTeam;
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        private Vector3 _previousPosition;

        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
        }


        private void OnEnable ()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start ()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;

            //StartCoroutine(MoveTo(new Vector3(11, 1, -4)));


            //Debug.Log(Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(11, -4) - new Vector2(transform.position.x, transform.position.z)));

            //Debug.Log(IsMyNextRotationBringMeCloserToTarget(Quaternion.Euler(0, 90, 0), new Vector3(-11, 1, -4)));
        }


        private void Update ()
        {
            // Store the value of both input axes.
            m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

            EngineAudio ();

            //Debug.Log(Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(11, -4) - new Vector2(transform.position.x, transform.position.z)));
            //Debug.Log(Vector3.Angle(transform.forward, new Vector3(11, 1, -4) - transform.position));
        }


        private void EngineAudio ()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move ();
            Turn ();
        }


        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }

        public IEnumerator MoveTo(Vector3 destination)
        {
            // Conversion des vector en 2D pour ne pas prendre en compte la hauteur dans le calcule de l'angle            
            // Oriente le tank vers sa destination
            while (Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(destination.x, destination.z) - new Vector2(transform.position.x, transform.position.z)) > 6)
            {
                float turn = m_TurnSpeed * Time.deltaTime;
                Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

                if(!IsMyNextRotationBringMeCloserToTarget(turnRotation, destination))
                {
                    turnRotation = Quaternion.Euler(0f, -turn, 0f);
                }

                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

                //Debug.Log(Vector2.Angle(forward2D, destination2D - position2D));

                yield return null;
            }

            // Conversion des vector en 2D
            // Avance le tank jusqu'à sa destination
            while (Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z)) > 1 &&
                IsMyNextPositionBringMeCloserToTarget(transform.position + transform.forward * m_Speed * Time.deltaTime, destination))
            {
                Vector3 movement = transform.forward * m_Speed * Time.deltaTime;

                m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

                yield return null;
            }
        }

        // Retourne vrais si ma prochaine rotation me rapproche face à ma cible
        private bool IsMyNextRotationBringMeCloserToTarget(Quaternion nextRotation, Vector3 targetPosition)
        {
            // Create a new transform for preview the next rotation
            GameObject transformContainer = new GameObject();
            Transform nextStep = transformContainer.transform;
            nextStep.position = transform.position;
            nextStep.localEulerAngles = transform.localEulerAngles;

            nextStep.localRotation *= nextRotation;

            //Debug.Log(transform.localRotation.eulerAngles);
            //Debug.Log(nextStep.localRotation.eulerAngles);

            float actualAngle = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targetPosition.x, targetPosition.z) - new Vector2(transform.position.x, transform.position.z));
            float futurAngle = Vector2.Angle(new Vector2(nextStep.forward.x, nextStep.forward.z), new Vector2(targetPosition.x, targetPosition.z) - new Vector2(nextStep.position.x, nextStep.position.z));
            
            //Debug.Log(actualAngle);
            //Debug.Log(futurAngle);

            if (futurAngle < actualAngle)
            {
                Destroy(transformContainer);
                return true;
            }

            Destroy(transformContainer);
            return false;
        }

        private bool IsMyNextPositionBringMeCloserToTarget(Vector3 nextPosition, Vector3 targetPosition)
        {
            GameObject transformContainer = new GameObject();
            Transform nextStep = transformContainer.transform;
            nextStep.position = nextPosition;

            float actualPosition = Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosition.x, targetPosition.z));
            float futurPosition = Vector3.Distance(new Vector2(nextStep.position.x, nextStep.position.z), new Vector2(targetPosition.x, targetPosition.z));

            if (futurPosition < actualPosition)
            {
                Destroy(transformContainer);
                return true;
            }

            Destroy(transformContainer);
            return false;
        }

        public IEnumerator SetItinary(List<Vector3> newItinary)
        {
            // On enregistre l'itinéraire dans un nouvel espace mémoire pour qu'il ne soit pas modifié
            List<Vector3> itinary = new List<Vector3>(newItinary);

            foreach (Vector3 position in itinary)
            {
                yield return StartCoroutine(MoveTo(position));
            }
        }
    }
}