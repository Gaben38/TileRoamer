using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    /// <summary>
    /// Угол переодического вращения объекта.
    /// </summary>
    public float RotationAngle = 2F;
    /// <summary>
    /// Интервал периодического вращения объекта (сек.).
    /// </summary>
    public float RotationInterval = 0.016667F;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Rotate), 0F, RotationInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Rotate()
    {
        this.transform.Rotate(0, RotationAngle, 0, Space.Self);
    }
}
