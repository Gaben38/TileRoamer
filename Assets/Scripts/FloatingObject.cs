using System;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    /// <summary>
    /// Использование относительного или абсолютного отклонения. 
    /// Относительное - Deviation это множитель размера объекта по Y оси, результатом будет отклонение..
    /// Абсолютное - Deviation это отклонение в единицах измерения Unity.
    /// </summary>
    public bool UseAbsoluteDeviation = true;

    /// <summary>
    /// Максимум отклонения по Y. Может быть относительным или абсолютным, см UseAbsoluteDeviation.
    /// Относительное - Deviation это множитель размера объекта по Y оси, результатом будет отклонение.
    /// Абсолютное - Deviation это отклонение в единицах измерения Unity.    /// </summary>
    public float MaxDeviation = 0.25F;

    /// <summary>
    /// GameObject у которого берется рендерер для получения размеров при относительном отклонении.
    /// </summary>
    public GameObject RendererOrigin;

    /// <summary>
    /// Отклонение в текущий момент времени.
    /// </summary>
    private float ActualDeviation = 0F;

    public void Start()
    {
        if (!UseAbsoluteDeviation)
        {
            var sizeY = RendererOrigin.GetComponent<Renderer>().bounds.size.y;
            MaxDeviation *= sizeY;
            ActualDeviation *= sizeY;
            Debug.Log($"{nameof(FloatingObject)}: Object '{name}' uses relative deviation {MaxDeviation / sizeY} x {sizeY} = {MaxDeviation}");
        }
    }

    /// <summary>
    /// Вычисление нового отклонения и изменение высоты объекта.
    /// </summary>
    public void Update()
    {
        var oldDeviation = ActualDeviation;
        ActualDeviation = MaxDeviation * (float)Math.Sin(Time.realtimeSinceStartup);
        transform.position = new Vector3(transform.position.x , transform.position.y - oldDeviation + ActualDeviation, transform.position.z);
    }

    private void Reset()
    {
        UseAbsoluteDeviation = true;
        MaxDeviation = 0.25F;
        ActualDeviation = 0F;
        RendererOrigin = gameObject;
    }

    private void OnValidate()
    {
        if (RendererOrigin == null)
            RendererOrigin = gameObject;
        if (Mathf.Abs(ActualDeviation) > MaxDeviation)
            Debug.LogError($"{name}: {nameof(ActualDeviation)}({ActualDeviation}) cannot be more (or less) than {nameof(MaxDeviation)}({MaxDeviation}).", gameObject);
    }


}