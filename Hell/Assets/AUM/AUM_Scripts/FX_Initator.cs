using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Initator : MonoBehaviour
{
    public Transform fxTransform;

    public ParticleSystem dustPT;

    Coroutine lastDesactivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FXInitiator(int fxIndex)
    {
        FXManager.fxm.fxInstancier(fxIndex, fxTransform, 0);
    }

    void StartDesactiveAfter()
    {
        if (lastDesactivate != null)
            StopCoroutine(lastDesactivate);

        Debug.Log("oui");

        lastDesactivate = StartCoroutine(ParticleDustDesactivateAfter());
    }

    IEnumerator ParticleDustDesactivateAfter()
    {
        dustPT.gameObject.SetActive(false);

        yield return null;

        dustPT.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        dustPT.gameObject.SetActive(false);
    }
}
