using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [FormerlySerializedAs("targetSpriteRenderer")] public Image targetImage; // Assign this in the Inspector

    public MapCard mapCard; // Assign this in the Inspector

    [SerializeField] private string imageName;
    void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        
        if (mapCard == null)
        {
            mapCard = GetComponent<MapCard>();
        }
        
        //Use mapCard to get the name of the Icon you need

        imageName = mapCard.map.mapName;

        // The path is relative to any Resources folder
        // For myImage.png in Assets/Textures/UI/Resources/Images/myImage.png, the path is "Images/myImage"
        string resourcePath = "Icons/Icon_" + imageName; 

        // Load the texture
        Texture2D loadedTexture = Resources.Load<Texture2D>(resourcePath);

        if (loadedTexture != null)
        {
            // Convert Texture2D to Sprite for the SpriteRenderer
            Sprite newSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
            targetImage.sprite = newSprite;
            Debug.Log($"Successfully loaded and applied {imageName}.png");
        }
        else
        {
            Debug.LogError($"Failed to load texture at path: {resourcePath}. Make sure it's in a 'Resources' folder and the name is correct.");
        }
    }
}