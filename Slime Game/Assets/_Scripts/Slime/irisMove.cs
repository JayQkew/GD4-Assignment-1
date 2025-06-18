using UnityEngine;

public class irisMove : MonoBehaviour
{
    private Vector2 _parent;
    public Vector3 eyePosition;
    [HideInInspector] public InputHandler inputHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _parent = transform.parent.parent.parent.localPosition;
        //transform.parent.localPosition = eyePosition;
        inputHandler = transform.parent.parent.parent.parent.GetComponent<InputHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        eyeFollow(inputHandler.moveInput);
    }

    public void eyeFollow(Vector3 pos)
    {
        
        Vector2 direction = new Vector2(((pos.x- _parent.x))/5 , ((pos.y - _parent.y))/5); //15 -0.443f/15
        transform.localPosition = direction;
        
    }
    
}
