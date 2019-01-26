using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	CharacterController controladorPersonaje;

	[SerializeField]
	float velocidadMovimiento = 2.0f;

	// Esta variable es usada para tener una referencia al gráfico, para rotarlo de acuerdo a la dirección de movimiento.
	[SerializeField]
	Transform grafico;

	// Se guarda una referencia a la cámara.
	private Camera _camaraPrincipal;

	// Start is called before the first frame update
	void Start()
	{
			// Es buena práctica guardar la referencia en lugar de utilizar Camera.main cada frame,
			// digamos que no es muy eficiente para llamarlo dentro de un "update".
			_camaraPrincipal = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{

		Movimiento();
	}


	void Movimiento()
	{
		float axisVertical = Input.GetAxis("Vertical");
		float axisHorizontal = Input.GetAxis("Horizontal");
		Vector3 movimiento = new Vector3(axisHorizontal, 0, axisVertical);

		// Rotar movimiento de acuerdo a la cámara.
		// Para hacer que el movimiento corresponda a la dirección de la cámara, vamos a ignorar todo excepto la rotación en el eje Y.
		float rotacionCamaraY = _camaraPrincipal.transform.rotation.eulerAngles.y;
		// Se crea un cuaternión (clase que contiene una matriz de rotación) a partir de ángulos de rotación. Eje Y rota al personaje horizontalmente.
		Quaternion rotacionCamara = Quaternion.Euler(0, rotacionCamaraY, 0);
		movimiento = rotacionCamara * movimiento; // Para rotar un vector, multiplica el quaternion por el vector. It's magic.

		controladorPersonaje.SimpleMove(movimiento*velocidadMovimiento);
		// Rotar el personaje de acuerdo al movimiento.
		// Si el vector es 0,0 tendrá magnitud 0, es mejor ignorarlo. sqrMagnitude es el cuadrado de la magnitud, lleva un cálculo menos -> es más liviano. :)
		if (movimiento.sqrMagnitude > 0)
		{
			// LookRotation básicamente crea una rotación que apunta hacia donde está mirando el vector que le pases.
			Quaternion direccion = Quaternion.LookRotation(movimiento);
			grafico.rotation = direccion; // Y aquí se asigna la dirección directamente al gráfico.
		}
		// POR HACER: Animar al personaje. Las animaciones de correr y de estar parado están hechas. :D
	}
}
