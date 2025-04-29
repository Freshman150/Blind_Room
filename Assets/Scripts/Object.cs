using UnityEngine;

// Enumération des types de matériaux possibles
// Utilisée pour identifier la "nature" de l'objet (utile pour jouer un son spécifique)
public enum Mat
{
    METAL,
    STONE,
    WOOD
}

public class Object : MonoBehaviour
{
    // Définit le matériau de cet objet dans l'inspecteur
    public Mat material;
}
