using Mirror;
using UnityEngine;

public class WoodenGate : InteractableBuilding
{
    public Sprite[] gateSprites = new Sprite[2];
    private SpriteRenderer gateRenderer;
    [SyncVar] private bool open = false;

    public override int id => 1;
    public override string buildingName => "Wooden Gate";

    protected override void AwakeExtension()
    {
        base.AwakeExtension();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("OpenGate"), LayerMask.NameToLayer("Default"), true);
    }

    protected override void StartExtension() 
    { 
        base.StartExtension();

        gateRenderer = gameObject.GetComponent<SpriteRenderer>(); 

        if (isServer)
        {
            maxHealth = 1000;
        }
    }

    private void Update()
    {
        if (open)
        {
            gateRenderer.sprite = gateSprites[1];
            gateRenderer.sortingLayerName = "Low Building";
            gameObject.layer = LayerMask.NameToLayer("OpenGate");
        } 
        else
        {
            gateRenderer.sprite = gateSprites[0];
            gateRenderer.sortingLayerName = "Default";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void Interact()
    {
        if (isServer)
        {
            if (!open)
            {
                open = true;
            }
            else
            {
                open = false;
            }
        }
    }
}
