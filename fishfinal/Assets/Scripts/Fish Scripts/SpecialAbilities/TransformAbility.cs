using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransformAbility : MonoBehaviour, ISpecialAbility
{
    [SerializeField] private float coolDown = 5f;

    private int currentIdx = 0;
    private float cooldownTimer;
    private List<GameObject> models = new List<GameObject>();

    public bool IsReady => cooldownTimer <= 0f;
    public float Cooldown => coolDown;

    private System.Action<GameObject> onModelChanged;

    public void RegisterModel(GameObject model)
    {
        models.Add(model);
        model.SetActive(models.Count == 1);
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    public void Activate()
    {
        models[currentIdx].SetActive(false);
        currentIdx = (currentIdx + 1) % models.Count;
        models[currentIdx].SetActive(true);
        cooldownTimer = coolDown;
    }

    public void Deactivate() { }

}
