using System.Collections;
using UnityEngine;

public class Diamond : InteractObject
{

    protected override IEnumerator OnInteract()
    {
        playerInteract.diamond = true;
        yield return null;
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
