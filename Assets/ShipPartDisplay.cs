using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPartDisplay : MonoBehaviour
{
    public ShipPart shipPart;

    public Image artwork;
    public Image background;

    private Material outlineMaterial;

    public void rotate90CW()
    {
        artwork.transform.Rotate(new Vector3(0, 0, 90));
        shipPart.rotation = artwork.transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TEST COLLIDE");

        artwork.material = outlineMaterial;
    }

    private void Awake()
    {
         outlineMaterial = Resources.Load<Material>("OutlineMaterial");
    }
    // Start is called before the first frame update
    void Start()
    {
        artwork.sprite = shipPart.artwork;
    }
    void Update()
    {
    }



}
