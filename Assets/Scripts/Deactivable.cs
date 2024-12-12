using UnityEngine;

public class Deactivable : MonoBehaviour
{
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
