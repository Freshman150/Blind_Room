# Blind Room

## 👤 Auteurs
- Antonin Claudel  
- Maxime Xu  
- Romain Malka  
- Enzo Mariant  

---

## 🎮 Présentation du projet

**Blind Room** est un escape game narratif et immersif en réalité virtuelle dans lequel le joueur incarne un personnage aveugle. L’expérience repose principalement sur l’**ouïe**, les **retours haptiques** et de légers indices visuels : navigation, interactions et ambiance sont perçues à travers les sons et le "toucher". Le joueur évolue dans des espaces qu’il découvre grâce à des indices auditifs, jusqu'à atteindre la sortie du niveau.

---

## 🧠 Architecture & Organisation Technique

### 📂 Organisation du projet Unity

```
Assets/ 
├── Abandoned_Asylum/ # Assets visuels d’asile abandonné
├── Audios/ # Effets sonores, musiques, ambiances, voix 
├── Materials/ 
├── NuGet/ # Dépendances via NuGet 
├── Packages/ 
├── Plugins/ 
├── Prefabs/ # Objets interactifs réutilisables 
├── Rust Key/ # Assets visuels de clé
├── Samples/ # Réutilisé dans le projet
├── Scenes/ 
├── TextMesh Pro/ 
├── VRTemplateAssets/ # Réutilisé dans le projet 
├── XR/ # Composants XR standards 
├── XRI/ # Composants XR Interaction Toolkit
```

### 🧩 Composants clés

- **`PlayerController.cs`**  
  Gère les mouvements du joueur, les sons de pas, la possession de clés et les interactions avec des zones déclencheuses pour progresser dans le niveau.

- **`AudioManagerController.cs`**  
  Centralise la gestion des sons avec un système d’`enum` et plusieurs `AudioSource` pour une lecture simultanée des effets sonores.

- **`VRProximityHaptics.cs`**  
  Gère les retours haptiques et les sphères visuelles en fonction de la proximité des contrôleurs VR avec des objets ou des surfaces.

- **`LidarSystem.cs`**  
  Simule un système de Lidar en générant des points dans l’espace pour aider le joueur à se repérer.

- **`VRMenuSpawner.cs`**  
  Génère un menu VR interactif autour du joueur, avec des boutons permettant de naviguer entre les scènes.

- **`VRMenuRaycaster.cs`**  
  Permet de sélectionner des boutons du menu VR en pointant avec le casque ou les contrôleurs.

- **`VolumeBasedOnAngle.cs`**  
  Ajuste le volume d’un son en fonction de l’angle entre la caméra et l’objet sonore.


---

### 🧠 Architecture logique

- **Interactions VR**  
  Gestion via `XRGrabInteractable`, les colliders et les triggers (`OnTriggerEnter`, `selectEntered`, etc.).

- **Progression**  
  Balisée par des zones de déclenchement (`OnTriggerEnter`) déclenchant sons, animations ou transitions de scène.

- **Audio**  
  Gestion centralisée et contextualisée selon les matériaux, les objets ou les étapes de la progression.

- **Haptique**  
  Retours haptiques dynamiques en fonction de la proximité des mains avec les surfaces ou objets interactifs.

- **Lidar**  
  Système d’aide à la navigation générant des points visuels dans l’espace à l’aide des contrôleurs.


---

## 🎧 Sound Design

Le jeu repose sur une ambiance sonore immersive et directionnelle.  
Les sons guident le joueur :  
- Bruits de pas synchronisés au mouvement  
- Sons de clés, portes, objets selon leur matériau
- Sons de scintillement pour indiquer la direction à suivre

—
## 🎮 Haptic Design

- **Proximity Feedback**  
  Utilise des impulsions haptiques pour indiquer la proximité d’objets ou de murs.  
  L’intensité augmente à mesure que le joueur s’approche.  
  Implémenté dans `VRProximityHaptics.cs` via `HapticImpulsePlayer`.

- **Interaction Feedback**  
  Déclenche un retour haptique lors des interactions avec les objets (ramassage de clés, déverrouillage).  
  Impulsions brèves et nettes pour simuler les actions.

- **Lidar Feedback**  
  Fournit un retour haptique lorsque le système Lidar détecte des objets.  
  Vibration continue à faible intensité pour simuler un balayage.

- **Directional Guidance**  
  Utilise les contrôleurs gauche ou droit pour orienter le joueur grâce à des vibrations localisées.

---

## 👁️ Visual Design

- **Minimal Visual Cues**  
  Sphères lumineuses simples (`VRProximityHaptics.cs`) pour indiquer la proximité ou l’interactivité.  
  Couleur variable : rouge pour les murs, bleu pour les objets.

- **Lidar Visualization**  
  Affiche des points dans l’environnement lors du déclenchement du Lidar (`LidarSystem.cs`).  
  Les points disparaissent après quelques secondes pour éviter l’encombrement visuel.

- **UI Placement**  
  Utilise `VRMenuSpawner.cs` pour positionner dynamiquement les menus face au joueur.  
  Menus simplifiés, accessibles et avec de grands boutons.

- **Audio-Driven Visuals**  
  Synchronise des effets visuels subtils (ex. : pulsation de sphères) avec des indices sonores pour renforcer l’immersion.


