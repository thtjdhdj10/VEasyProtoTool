using UnityEngine;
using System.Collections;

public class TargetableUnit : Unit {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public virtual void Damage(HitTypeSwing ht)
    //{
    //    AudioSource.PlayClipAtPoint(SoundManager.boom, transform.position, 0.2f);

    //    ImmortalTime = ht.immortalTime;

    //    moveForce = ht.force;
    //    rigid.AddForce(moveForce);
    //    rigid.velocity = moveForce * Time.fixedDeltaTime / rigid.mass;

    //    ParticleManager.CreateParticleRequest("ParticleSwingImpact", transform.position, ht.euler);
    //    ParticleManager.CreateParticleRequest("ParticleBlood", transform.position, ht.euler);

    //    statCurrentHp -= ht.damage;
    //}
}
