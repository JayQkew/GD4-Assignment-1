using UnityEngine;

public class irisMove : MonoBehaviour
{
    private Vector2 _parent;
    [HideInInspector] public InputHandler inputHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _parent = transform.parent.parent.parent.parent.position;
        inputHandler = transform.parent.parent.parent.parent.GetComponent<InputHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        eyeFollow(inputHandler.moveInput);
    }

    public void eyeFollow(Vector3 pos)
    {
        
        Vector2 direction = new Vector2(((pos.x- _parent.x)-6.5f)/5 , ((pos.y - _parent.y)-0.7f)/5); //15 -0.443f/15
        transform.localPosition = direction;
        
    }
}
