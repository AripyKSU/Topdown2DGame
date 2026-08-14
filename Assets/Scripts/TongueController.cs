using System;
using System.Collections;
using UnityEngine;
//
// //[RequireComponent(typeof(LineRenderer))]
// [RequireComponent(typeof(Collider2D))]
// [RequireComponent(typeof(Rigidbody2D))]
public class TongueController : MonoBehaviour
{
    //[SerializeField] GameObject beam;
    private Collider2D col;
    private SpriteRenderer sr;

    public void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
    public void InitAndFire(Vector2 startWorld, Vector2 dirNorm, float maxLength,
        float extendSpd, float retractSpd, float width, Action onDone)
    {
        StartCoroutine(BeamCoroutine(onDone));
    }

    public IEnumerator BeamCoroutine(Action onDone)
    {
        sr.enabled = true;
        yield return new WaitForSeconds(0.1f);

        sr.enabled = false;
        yield return new WaitForSeconds(0.4f);
        
        sr.enabled = true;
        yield return new WaitForSeconds(0.1f);

        sr.enabled = false;
        yield return new WaitForSeconds(0.4f);

        sr.enabled = true;
        col.enabled = true;
        yield return new WaitForSeconds(2.0f);
        
        col.enabled = false;
        sr.enabled = false;
        onDone?.Invoke();
    }
}
//     private LineRenderer lr;
//     private EdgeCollider2D edge;
//     private Rigidbody2D rb;
//
//     private Vector2 start;
//     private Vector2 dir;
//     private float maxLen;
//     private float extendSpeed;
//     private float retractSpeed;
//     private float thickness;
//     private Action onFinished;
//
//     private float curLen;
//     private bool extending;
//     private bool active;
//
//     private void Awake()
//     {
//         // lr = GetComponent<LineRenderer>();
//         // edge = GetComponent<EdgeCollider2D>();
//         // rb = GetComponent<Rigidbody2D>();
//         // rb.bodyType = RigidbodyType2D.Kinematic;
//         //
//         // // ???? ????
//         // gameObject.tag = "Tongue";
//         //
//         // // LineRenderer ???? ????
//         // lr.positionCount = 2;
//         // lr.useWorldSpace = true;
//     }
//
//     public void InitAndFire(Vector2 startWorld, Vector2 dirNorm, float maxLength,
//                             float extendSpd, float retractSpd, float width, Action onDone)
//     {
//         // start = startWorld;
//         // dir = dirNorm.normalized;
//         // maxLen = Mathf.Max(0.1f, maxLength);
//         // extendSpeed = extendSpd;
//         // retractSpeed = retractSpd;
//         // thickness = width;
//         // onFinished = onDone;
//         //
//         // curLen = 0f;
//         // extending = true;
//         // active = true;
//
//         // ???? ????/????????
//         ApplyRenderAndCollider();
//     }
//
//     private void Update()
//     {
//         if (!active || !GameFlow.I || !GameFlow.I.IsRunning || GameFlow.I.IsGameOver)
//         {
//             if (active && GameFlow.I && GameFlow.I.IsGameOver) // ???????? ?? ???? ????
//             {
//                 active = false;
//             }
//             return;
//         }
//
//         if (extending)
//         {
//             curLen += extendSpeed * Time.deltaTime;
//             if (curLen >= maxLen)
//             {
//                 curLen = maxLen;
//                 extending = false; // ???? ???? ???? ?? ????
//             }
//         }
//         else
//         {
//             curLen -= retractSpeed * Time.deltaTime;
//             if (curLen <= 0f)
//             {
//                 curLen = 0f;
//                 active = false;
//                 onFinished?.Invoke();
//             }
//         }
//
//         ApplyRenderAndCollider();
//     }
//
//     private void ApplyRenderAndCollider()
//     {
//         Vector2 end = start + dir * curLen;
//
//         lr.startWidth = thickness;
//         lr.endWidth = thickness;
//         lr.SetPosition(0, start);
//         lr.SetPosition(1, end);
//
//         // EdgeCollider2D?? ???????? ???? ?? ?????????? ????
//         Vector2 p0 = transform.InverseTransformPoint(start);
//         Vector2 p1 = transform.InverseTransformPoint(end);
//         edge.points = new Vector2[] { p0, p1 };
//         edge.isTrigger = true;
//     }
// }
