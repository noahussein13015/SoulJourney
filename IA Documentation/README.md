# IA Documentation — Soul Journey

Documentation technique tenue par l'IA (Claude) pour se retrouver dans le projet d'une session à l'autre.
La banque d'idées de game design reste dans le [README principal](../README.md).

## Le projet en bref
Jeu de puzzle 2D vue de dessus, **Godot 4.7 / C#**, résolution 640×360 en pixel art (zoom entier).
Le joueur est une **âme** (`PlayerSoul`) qui **possède des corps** (`Host`) pour utiliser leurs capacités.
Les objets du monde (`IInteractable`) décident eux-mêmes quels corps peuvent les utiliser.

## Sommaire
| Fichier | Contenu |
|---|---|
| [Architecture.md](Architecture.md) | Principes, flux du bouton action, signaux, collision layers, inputs, conventions de code |
| [Composants.md](Composants.md) | Fiche de chaque composant : rôle, scène, script, propriétés, signaux |
| [Journal.md](Journal.md) | Historique des fonctionnalités ajoutées, le plus récent en haut |

## Règle de mise à jour
À chaque fonctionnalité ajoutée ou modifiée :
1. ajouter une entrée en haut de [Journal.md](Journal.md) (date, ce qui a été fait, fichiers, points à faire dans l'éditeur) ;
2. créer ou mettre à jour la fiche du composant dans [Composants.md](Composants.md) ;
3. mettre à jour [Architecture.md](Architecture.md) si un principe, un layer, un input ou une convention change.
