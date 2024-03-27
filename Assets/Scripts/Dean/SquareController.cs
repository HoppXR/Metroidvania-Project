using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour
{
    private bool isDisabled = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<LineRenderer>() != null)
        {
            StartCoroutine(DisableColliderForDuration(2f)); 
        }
    }

    private IEnumerator DisableColliderForDuration(float duration)
    {
        if (!isDisabled)
        {
            isDisabled = true;
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(duration);
            GetComponent<Collider2D>().enabled = true;
            isDisabled = false;
        }
    }
}
