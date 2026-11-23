using System.Collections;
using UnityEngine;

public class Ruby : InteractObject
{
    protected override IEnumerator OnInteract()
    {
        playerInteract.ruby = true;
        yield return null;
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
