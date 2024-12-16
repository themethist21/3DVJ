using UnityEngine;

public class Terrain : MonoBehaviour
{
    private GameObject renderedObject;

    private bool makeVisible = false;

    private bool isVisible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderedObject = this.transform.GetChild(0).gameObject;
        renderedObject.transform.Translate(Vector3.up * (-8 - this.transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVisible && makeVisible)
        {
            if (renderedObject.transform.position.y < this.transform.position.y)
            {
                renderedObject.transform.Translate(Vector3.up * 2.0f * Time.deltaTime);
            }
            else
            {
                renderedObject.transform.SetPositionAndRotation(new Vector3(renderedObject.transform.position.x, this.transform.position.y, renderedObject.transform.position.z), renderedObject.transform.rotation);
                isVisible = true;
            }
        }
    }

    public void SetVisible()
    {
        makeVisible = true;
    }
}
