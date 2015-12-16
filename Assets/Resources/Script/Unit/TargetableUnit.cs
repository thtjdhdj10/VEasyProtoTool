using UnityEngine;
using System.Collections;

public class TargetableUnit : MonoBehaviour
{ 
	void Start ()
    {
        var layerSetting = gameObject.GetComponent<LayerSetting>();
        if (layerSetting == null)
        {
            layerSetting = gameObject.AddComponent<LayerSetting>();
        }

        layerSetting.AddLayer("Targetable");
    }
	
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
