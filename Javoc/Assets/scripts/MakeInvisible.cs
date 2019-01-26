using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour
{
    public class ObjetoEscondidoData {
        public Transform obj;
        public float ultimoCheck;
        public Material mat;
        public bool escondido;

        public ObjetoEscondidoData(Transform _obj, float _check, Material _m) {
            obj = _obj;
            ultimoCheck = _check;
            mat = _m;
            escondido = true;
        }
    }

    // Tag a buscar en objetos encontrados al hacer raycasting.
    [SerializeField] string tagName = "Esconder";
    [SerializeField] float checkTime = 0.1f;
    private Transform _jugador;
    List<ObjetoEscondidoData> objetosEscondidos;

    void Start()
    {
        objetosEscondidos = new List<ObjetoEscondidoData>();
        _jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 direccion = _jugador.position - transform.position;
        Ray ray = new Ray(transform.position, direccion);
        RaycastHit[] hits = Physics.RaycastAll(ray, direccion.magnitude);
        foreach(RaycastHit hit in hits) {
            if (hit.transform.CompareTag(tagName)) {
                // Esconde
                EscondeObjeto(hit.transform);
            }
        }
        //
        foreach(ObjetoEscondidoData o in objetosEscondidos) {
            if (o.escondido) {
                float tiempoTranscurrido = Time.time - o.ultimoCheck;
                if (tiempoTranscurrido > checkTime) AparecerObjeto(o);
            }
        }
    }

    void EscondeObjeto(Transform t) {
        ObjetoEscondidoData objData = null;
        foreach (ObjetoEscondidoData o in objetosEscondidos) {
            if (o.obj == t) objData = o;
            break;
        }
        if (objData == null) {
            Material m = null;
            MeshRenderer mr = t.GetComponent<MeshRenderer>();
            if (mr != null) m = mr.material;
            objData = new ObjetoEscondidoData(t, Time.time, m);
            objetosEscondidos.Add(objData);
        }
        else {
            objData.ultimoCheck = Time.time;
            if (objData.escondido) return;
            objData.escondido = true;
        }
        //
        if (objData.mat != null) {
            Debug.Log("Desaparece.");
            StandardShaderUtils.ChangeRenderMode(objData.mat, StandardShaderUtils.BlendMode.Transparent);
            Color c = objData.mat.color;
            c.a = 0.5f;
            objData.mat.color = c;
        }
    }

    void AparecerObjeto(ObjetoEscondidoData objData) {
        if (objData.mat != null) {
            Debug.Log("Reaparece.");
            StandardShaderUtils.ChangeRenderMode(objData.mat, StandardShaderUtils.BlendMode.Opaque);
            Color c = objData.mat.color;
            c.a = 1f;
            objData.mat.color = c;
        }
        objData.escondido = false;
    }
}
