using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHandler : MonoBehaviour
{
    private enum FloorDimension
    {
        Overworld,
        Hell,
        Faerie
    }

    public bool isLight = true;

    private GameController gameController;
    private FloorDimension floorDimension;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            floorDimension = FloorDimension.Overworld;
            if (isLight) animator.Play("lightoverworld");
            else animator.Play("darkoverworld");
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            floorDimension = FloorDimension.Hell;
            if (isLight) animator.Play("lighthell");
            else animator.Play("darkhell");
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            floorDimension = FloorDimension.Faerie;
            if (isLight) animator.Play("lightfaerie");
            else animator.Play("darkfaerie");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime, transform.position.y, transform.position.z);
        if (transform.position.x <= -10.0f) transform.position = new Vector3(10.0f, transform.position.y, transform.position.z);
        if (gameController.currentDimension == GameController.Dimension.Overworld && floorDimension != FloorDimension.Overworld)
        {
            floorDimension = FloorDimension.Overworld;
            if (isLight) animator.Play("lightoverworld");
            else animator.Play("darkoverworld");
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell && floorDimension != FloorDimension.Hell)
        {
            floorDimension = FloorDimension.Hell;
            if (isLight) animator.Play("lighthell");
            else animator.Play("darkhell");
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie && floorDimension != FloorDimension.Faerie)
        {
            floorDimension = FloorDimension.Faerie;
            if (isLight) animator.Play("lightfaerie");
            else animator.Play("darkfaerie");
        }
    }
}
