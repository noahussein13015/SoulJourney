# Composants

## PlayerSoul — l'âme
- **Scène / script :** `Components/PlayerSoul/player_soul.tscn`, `Scripts/PlayerSoul.cs` (`CharacterBody2D`)
- **Rôle :** personnage joueur. Seul script qui lit `action_button` (voir flux dans [Architecture.md](Architecture.md)).
- **États :** `SOUL` (flotte, mouvement amorti par `Lerp`) / `IN_HOST` (cachée, collision désactivée, ne bouge plus).
- **Exports :** `Speed` (200), `FloatingEffectAttenuation` (5).
- **Cibles publiques :** `TargetHost`, `TargetPortal` (posées par les objets).
- **Signal :** `StateChanged`. En sortant d'un corps, l'âme réapparaît sur la position du corps avec une vitesse nulle.

## Host — base abstraite des corps
- **Scène / script :** `Components/Hosts/host_base.tscn` (sans script), `Scripts/Host.cs` (`abstract`, `CharacterBody2D`)
- **Nœuds attendus :** `AnimatedSprite2D` (animations `idle`, `idle_possessed`), `CollisionShape2D`, `PossessionArea`.
- **États :** `EMPTY` / `POSSESSED` (propriété `State` en lecture seule, modifier via `SetState()`).
- **Exports :** `Speed` (100), `Weight` (`LIGHT`/`NORMAL`/`HEAVY`, prévu pour les plaques de pression).
- **Points d'extension :** `HandleAction()` (par défaut : utilise `TargetInteractable` si `CanBeUsedBy`), `ComputeVelocity()`, `OnStateChanged()`.
- **Signal :** `StateChanged`.

## Human
- **Scène / script :** `Components/Hosts/Human/human.tscn` (héritée de `host_base.tscn`), `Scripts/Human.cs`
- **Rôle :** hôte de base, ouvre les portes et actionne les leviers. Classe vide : tout vient de `Host`. Sprite : `KnightSpritesheet.png`.

## IInteractable
- **Script :** `Components/Interactables/Scripts/IInteractable.cs`
- **Contrat :** `bool CanBeUsedBy(Host host)` et `void Interact(Host host)`.
- **Modèle d'implémentation :** un `StaticBody2D` (layer World) + une `DetectionZone` (`Area2D`, masque HostNormal) qui pose/efface `Host.TargetInteractable`.

## Lever — levier
- **Scène / script :** `Components/Interactables/lever.tscn`, `Scripts/Lever.cs` (`StaticBody2D, IInteractable`)
- **Utilisable par :** `Human` uniquement.
- **Comportement :** `Interact` bascule `_isOn`, joue l'animation `on`/`off` et émet `Toggled(bool isOn)`. Commence toujours sur off.
- **Usage :** connecter `Toggled` dans la scène de niveau à l'objet contrôlé (porte, pont…). Le levier ne connaît pas sa cible.
- **Collisions :** corps 12×4 décalé de 6 px vers le bas ; `DetectionZone` 26×21 décalée de 18 px vers le bas.
- **Placé dans :** `SpawnDungeon` (213, 206), `Toggled` pas encore connecté.
- **Sprite :** `Assets/LeverSpritesheet.png`, `off` = région (0, 0), `on` = région (0, 16), 16×16 chacune.

## MaterialDoor — porte physique
- **Scène / script :** `Components/Doors/material_door.tscn`, `Scripts/MaterialDoor.cs` (`StaticBody2D, IInteractable`)
- **Utilisable par :** `Human` uniquement.
- **Comportement :** ouverte tant qu'un hôte possédé autorisé est dans sa `DetectionZone` (écoute aussi le `StateChanged` de cet hôte) ; animation `closed` jouée après `CloseDelay` (0,5 s) si elle ne s'est pas rouverte entre-temps. `Interact` → émet `TeleportRequested`.
- **Exports :** `TargetRoom`, `TeleportationPoint` (`Marker2D`).

## SoulPortal — portail de l'âme
- **Scène / script :** `Components/Doors/soul_portal.tscn`, `Scripts/SoulPortal.cs` (`Area2D`)
- **Utilisable par :** l'âme libre uniquement (pose `PlayerSoul.TargetPortal`). Remplace l'idée de grille.
- **Comportement :** `RequestTeleport` → émet `TeleportRequested`.
- **Exports :** `TargetRoom`, `TeleportationPoint`.

## SpawnDungeon — zone de départ
- **Scène / script :** `Components/Areas/SpawnDungeon/spawn_dungeon.tscn`, `Scripts/SpawnDungeon.cs` (`Node2D`)
- **Contenu :** Tilemap (Walls, Ground, Decorations), `Camera2D`, `CameraPositions` (SpawnRoom = 0, Hall = 1), `Doors` (SoulPortal1, SoulPortal2, MaterialDoor1 + leurs points de téléportation), PlayerSoul, Human, Lever, ScreenFader.
- **Exports :** `RoomCameraPositions`, `PlayerSoul`, `SlideDistance` (12 px), `SlideDuration` (0,35 s).
- **Rôle :** reçoit tous les `TeleportRequested` (`OnTeleportRequested`) et joue la transition. Tout nouvel objet téléporteur doit y être connecté.

## ScreenFader — fondu au noir
- **Scène / script :** `Components/ScreenFader/screen_fader.tscn`, `Scripts/ScreenFader.cs` (`CanvasLayer` + `ColorRect` « Overlay »)
- **API :** `FadeOut()` / `FadeIn()` renvoient un `SignalAwaiter` (utilisables avec `await`). Un nouveau fondu annule le précédent.
- **Export :** `FadeDuration` (0,2 s).
