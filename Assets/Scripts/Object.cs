using UnityEngine;

// Enum�ration des types de mat�riaux possibles
// Utilis�e pour identifier la "nature" de l'objet (utile pour jouer un son sp�cifique)
public enum Mat
{
    METAL,
    STONE,
    WOOD
}

public class Object : MonoBehaviour
{
    // D�finit le mat�riau de cet objet dans l'inspecteur
    public Mat material;
}
