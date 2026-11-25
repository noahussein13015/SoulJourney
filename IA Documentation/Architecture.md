# Architecture

## Arborescence
```
Assets/                     sprites et tilesets (pixel art)
Components/
  Areas/SpawnDungeon/       zone de départ : salles, caméra, transitions
  Doors/                    MaterialDoor (corps) et SoulPortal (âme)
  Hosts/                    Host (base abstraite) + host_base.tscn
    Human/                  premier hôte jouable
  Interactables/            IInteractable + Lever
  PlayerSoul/               l'âme (le joueur)
  ScreenFader/              fondu au noir plein écran
main.tscn                   scène principale, instancie SpawnDungeon
```
Chaque composant = un dossier avec sa scène `.tscn` et un sous-dossier `Scripts/`.

## Principes
1. **Un objet demande, la zone exécute.** Les portes et portails émettent `TeleportRequested(body, destination, targetRoom)` ; c'est la zone (`SpawnDungeon`) qui fait le fondu, la téléportation et la caméra.
2. **Les objets décident qui peut les utiliser.** `IInteractable.CanBeUsedBy(Host)` ; ajouter un objet ne modifie jamais les hôtes.
3. **Héritage des hôtes.** `Host` (abstraite) contient possession, déplacement et action. Chaque hôte en hérite (script + scène héritée de `host_base.tscn`) et ne surcharge que ce qui le distingue (`ComputeVelocity`, `HandleAction`, `OnStateChanged`).
4. **Communication par signaux**, connectés dans les scènes. Exception : `Host` connecte ses signaux en code, car `host_base.tscn` n'a pas de script.
5. **Machines à états simples** : enum `STATE` + `SetState()` qui émet `StateChanged` seulement si l'état change.

## Flux du bouton action (`action_button`, touche E)
Lu **uniquement** par `PlayerSoul._PhysicsProcess` → `HandleAction()` :
```
Âme libre (SOUL)
  ├─ TargetHost != null  → Possess()             (priorité au corps)
  └─ sinon               → TargetPortal?.RequestTeleport()
Âme dans un corps (IN_HOST)
  └─ TargetHost.HandleAction()
       ├─ true  : le corps a utilisé son TargetInteractable
       └─ false : rien à faire → Unpossess()
```
Les cibles sont posées par les objets eux-mêmes quand un corps entre dans leur zone :
- `PlayerSoul.TargetHost` ← `Host.PossessionArea`
- `PlayerSoul.TargetPortal` ← `SoulPortal`
- `Host.TargetInteractable` ← `DetectionZone` de `MaterialDoor` / `Lever`

En sortie de zone, un objet n'efface la cible que si c'est encore lui (pour ne pas écraser un objet entré juste après).
Limite connue : si deux zones d'interactables se chevauchent, c'est la dernière entrée qui gagne.

## Transitions de salle (`SpawnDungeon.PlayTransition`)
Verrouille les inputs (`SetPhysicsProcess(false)` sur l'âme et le corps) → `FadeOut` → téléporte, vitesse à 0 → caméra placée décalée de `SlideDistance` dans l'axe du trajet → `FadeIn` en parallèle du glissement de caméra → déverrouille. `_isTransitioning` empêche deux transitions simultanées.
Les salles sont repérées par un index = position dans `RoomCameraPositions`.

## Collision layers
Dans les `.tscn`, `collision_layer`/`collision_mask` sont des **masques de bits** : valeur 1 = layer 1, 2 = layer 2, 4 = layer 3, 8 = layer 4…

| Layer | Nom | Utilisé par |
|---|---|---|
| 1 | PlayerSoul | l'âme |
| 2 | HostNormal | Host (Human) |
| 3 | World | murs du tilemap, MaterialDoor, Lever |
| 4 | HostSmall | *(prévu : Rat)* |
| 5 | HostHeavy | *(prévu : Ogre, Golem)* |
| 6 | HostFlying | *(prévu : Oiseau)* |
| 7 | Grid | *(nommé mais non utilisé : la grille a été abandonnée, le SoulPortal joue ce rôle)* |
| 8 | Duct | *(prévu : conduits)* |

- Âme : layer 1, masque World (valeur 4).
- Host : layer 2, masque World (4). `PossessionArea` : masque par défaut 1 (détecte l'âme).
- `DetectionZone` des interactables : masque 2 (HostNormal). ⚠️ Un futur hôte sur un autre layer (Small, Heavy…) ne sera pas détecté tant qu'on n'élargit pas ce masque.

## Inputs (`project.godot`)
`move_up/down/left/right` = ZQSD (positions physiques WASD), `action_button` = E.

## Conventions de code
- Commentaires et noms en **anglais** ; la doc et le README en français.
- Indentation tabulations ; propriétés `{get; set;}` sans espaces internes.
- Champs privés `_camelCase`, enums en MAJUSCULES (`STATE.SOUL`).
- Sections dans chaque script : `ACCESSORS`, `GODOT`, `LOGIC`, `SIGNAL RESPONSES` (bannières `////////////////// X ////////////////////`).
- Réponses aux signaux nommées `On<Source><Signal>` (`OnDetectionZoneBodyEntered`).
- Pixel art : zoom caméra entier uniquement, filtre texture « nearest », snap des transforms au pixel.
- Messages de commit courts, en anglais, en minuscules.
