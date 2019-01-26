using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]CharacterController controladorPer;
    [SerializeField] float velocidadMov = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float aVertical = Input.GetAxis("Vertical");
        float aHorizontal = Input.GetAxis("Horizontal");
        Vector3 movimiento = new Vector3(aHorizontal, 0, aVertical);
        controladorPer.SimpleMove(movimiento*velocidadMov);
        
    }
}
