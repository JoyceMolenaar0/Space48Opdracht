using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private Image itemImageHolder;
    [SerializeField] private TMP_Text messageField;
    private List<Color> items = new List<Color>();
    private int activeItemIndex = -1;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private float cooldownTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            PickUpItems(other.gameObject);
        }
    }

    void PickUpItems(GameObject item)
    {
        Color color = item.GetComponent<Renderer>().material.color;
        Destroy(item);
        items.Add(color);
        activeItemIndex = items.Count - 1;
        itemImageHolder.color = items[activeItemIndex];
        itemImageHolder.enabled = true;
    }

    void Update()
    {
        CycleItems();
        UseItem();
    }

    void CycleItems()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && items.Count > 0)
        {
            activeItemIndex = (activeItemIndex + 1) % items.Count;
            itemImageHolder.color = items[activeItemIndex];
        }
    }

    void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && activeItemIndex != -1)
        {
            Color activeItem = items[activeItemIndex];

            if (activeItem == Color.blue)
            {
                StartCoroutine(ShowMessage(" + Move Speed"));
                moveSpeed += 5;
            }
            else if (activeItem == Color.red)
            {
                StartCoroutine(ShowMessage(" + Fire Rate"));
                cooldownTime -= 0.1f;
            }
            else if (activeItem == Color.green)
            {
                StartCoroutine(ShowMessage(" + Rotation Speed"));
                rotationSpeed += 10;
            }

            items.RemoveAt(activeItemIndex);

            if (items.Count > 0)
            {
                activeItemIndex = (activeItemIndex > 0) ? activeItemIndex - 1 : 0;
                itemImageHolder.color = items[activeItemIndex];
            }
            else
            {
                itemImageHolder.color = Color.white;
                activeItemIndex = -1;
                itemImageHolder.enabled = false;
            }
        }
    }

    IEnumerator ShowMessage(string message)
    {
        messageField.enabled = true;
        messageField.text = message;
        yield return new WaitForSeconds(3f);
        messageField.enabled = false;
    }
}