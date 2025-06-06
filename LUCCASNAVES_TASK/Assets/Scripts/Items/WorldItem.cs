using System;
using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemIdentifierSO itemIdentifierSO;
    public ItemIdentifier itemIdentifier;
    [Min(1)]public int amount;

    public float rotationSpeed = 1.0f;
    public float upAndDownSpeed = 1.0f;
    public float upAndDownAmplitude = 1.0f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        if (itemIdentifierSO) itemIdentifier = itemIdentifierSO.identifier;
    }

    private void Update()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y + Mathf.Sin(Time.time * upAndDownSpeed) * upAndDownAmplitude, startPosition.z);
        transform.Rotate(new Vector3(0.0f, rotationSpeed * Time.deltaTime));
    }

    public void OnHover()
    {
    }

    public void OnInteract()
    {
    }

    public void OnUnhover()
    {
    }

    private void OnValidate()
    {
        if(itemIdentifierSO) itemIdentifier = itemIdentifierSO.identifier;
    }
}