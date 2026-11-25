# Journal des fonctionnalités

Le plus récent en haut. Format : date — titre, puis ce qui a été fait, les fichiers et ce qu'il reste à faire.

---

## 2026-11-25 — Levier : sprite, collision et placement
- Animations remplies avec `LeverSpritesheet.png` : `off` = région (0, 0), `on` = région (0, 16), 16×16 ; collision du corps réduite à 12×4, décalée de 6 px vers le bas.
- `CanBeUsedBy` simplifié en `return host is Human;`.
- Un levier est placé dans `SpawnDungeon` en (213, 206).
- **Fichiers :** `Components/Interactables/lever.tscn`, `Components/Interactables/Scripts/Lever.cs`, `Components/Areas/SpawnDungeon/spawn_dungeon.tscn`.
- **Reste à faire :** créer l'objet contrôlé (porte verrouillée / herse) et y connecter `Toggled`.

## 2026-11-23 — Création de la documentation IA
- Création du dossier `IA Documentation/` (README, Architecture, Composants, Journal) et d'un `CLAUDE.md` à la racine qui impose de la tenir à jour.

## 2026-11-23 — Levier (`Lever`) — commit `28d9a93`
- Nouvel interactable `Lever : StaticBody2D, IInteractable`, utilisable seulement par un `Human`.
- `Interact` bascule on/off, joue l'animation `on`/`off` et émet `Toggled(bool isOn)`.
- Aucune modification de `Host`, `Human` ou `PlayerSoul` : valide l'architecture `IInteractable`.
- L'export `StartsOn` a été retiré à la main après coup : le levier commence toujours sur off.
- **Fichiers :** `Components/Interactables/Scripts/Lever.cs`, `Components/Interactables/lever.tscn`, `Assets/LeverSpritesheet.png`.
- **Reste à faire :** remplir les animations avec `LeverSpritesheet.png`, ajuster les collisions, placer un levier dans une salle et créer l'objet qu'il contrôle (porte verrouillée / herse).

## 2026-11-23 — Décision de design : pas de grille
- La grille (obstacle que seule l'âme traverse) est abandonnée ; le **SoulPortal** remplit ce rôle. Le layer 7 `Grid` reste nommé mais inutilisé. Le README principal mentionne encore la grille.

## Avant la documentation (historique git)
- `9e03168` architecture d'héritage des hôtes : `Host` abstraite, scènes héritées, `IInteractable`.
- `685c0bd` README remplacé par la banque d'idées (hôtes, objets, progression).
- `0852627` ajout de `SoulPortal2` pour revenir à la salle de départ.
- `911a471` corrections de bugs, transitions de caméra améliorées.
- `d3bc538` style de code mis à jour.
