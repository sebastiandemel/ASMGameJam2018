using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ElementState {
    IDLE = 0,
    BURNING = 1,
    DEAD = 2
}

public class FireElement : MonoBehaviour {
    public float Healt = 1.0f;

    public float Decay = 0.01f;

    public float Damage = 0.01f;

    public bool isDead = false;

	public float CoolDown = 3.0f;

	private float _currentTime = 0;

    private void ChangeTo(ElementState state){  
        var activate = (int) state;
        
        if(!gameObject.transform.GetChild(activate).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false); // Idle
            gameObject.transform.GetChild(1).gameObject.SetActive(false); // Burning
            gameObject.transform.GetChild(2).gameObject.SetActive(false); // Burned

            gameObject.transform.GetChild(activate).gameObject.SetActive(true);
        }               
    }

    private void Start(){
        ChangeTo(ElementState.BURNING);
    }

    private void Update() {
        if(!isDead)
        {            
            _currentTime += Time.deltaTime;

            if(_currentTime >= CoolDown)
            {
                _currentTime = 0.0f;
                var elementHealt = gameObject.GetComponent<GridElement>().Health;
                
                if(Healt < 0.1f || elementHealt == 0.0f)
                {
                    if(Healt < 0.1f)
                    {
                        OnDeath();
                    }
                    else
                    {
                        gameObject.GetComponent<GridElement>().Health -= Decay;
                        ChangeTo(ElementState.IDLE);
                    }
                }
                else if(elementHealt < 0.1f)
                {
                    gameObject.GetComponent<GridElement>().Health = 0.0f;
                    ChangeTo(ElementState.DEAD);
                }
                else if(elementHealt > 0.0f)
                {
                    gameObject.GetComponent<GridElement>().Health -= Damage;
                    ChangeTo(ElementState.BURNING);
                }
            }
        }            
    }

    public void OnDeath()
    {
        Healt = 0.0f;
        isDead = true;
        
        var particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        if(particleSystems != null)
        {
            foreach( var ps in particleSystems) {
                ps.Stop(true);
                ps.Clear();
            }
        }
        
    }
}
