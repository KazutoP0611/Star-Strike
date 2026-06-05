using UnityEngine;

public class Player_VFX : Entity_VFX
{
    public override void OnDamage(Vector3 hitPoint)
    {
        base.OnDamage(hitPoint);

        //may be change material process too?

        //add short immortality?

        //do red screen only when hit;
        //check hp and show warning according to hp level;
    }
}
