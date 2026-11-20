# Soul Journey — Banque d'idées

Jeu de puzzle 2D en vue de dessus (Godot / C#, contrôles PC).
Le joueur est une **âme** qui traverse ce qu'un corps ne peut pas franchir, et qui **possède des hôtes** pour utiliser leurs capacités.
Chaque énigme repose sur le choix du bon corps, au bon endroit, dans le bon ordre.

---

## 1. L'âme (état de base)

| Capacité | Détail |
|---|---|
| Traverse les grilles | Les barreaux et grilles bloquent tous les corps, pas l'âme |
| Emprunte les SoulPortal | Téléportation réservée à l'âme |
| Possède / libère | Entre dans un hôte proche, en ressort à côté de lui |
| Fragile | Bloquée par la **lumière sacrée** et les **flammes bleues** (voir objets) |
| Temps limité *(option)* | Hors d'un corps, l'âme s'affaiblit : il faut trouver un hôte avant la fin du chrono |

---

## 2. Les hôtes

### Humain *(existant)*
- **Fonction :** il interagit avec le monde.
- Il ouvre les portes, actionne les leviers, ramasse et porte une clé, appuie sur des boutons, tourne les miroirs.
- **Limite :** c'est le corps le plus « moyen ». Il ne peut ni casser, ni voler, ni se faufiler.

### Ogre
- **Fonction :** la force brute.
- Il **casse les murs fissurés**, **pousse les blocs lourds** et écrase les caisses.
- Il pèse assez lourd pour activer les **plaques de pression lourdes**.
- **Limites :** il est lent et ne passe pas dans les couloirs étroits. Il effondre les **planchers fragiles** (ce qui peut aussi servir).

### Sorcier
- **Fonction :** la télékinésie.
- Il **déplace des objets à distance**, y compris en hauteur, par-dessus les fosses ou derrière les grilles.
- Il **active des runes** murales.
- **Limites :** il ne porte qu'un objet à la fois et doit garder une ligne de vue. Il est lent pour lancer un sort.

### Rat
- **Fonction :** se faufiler.
- Il passe dans les **trous de souris** et les **conduits**.
- Il est trop léger pour déclencher les plaques et les pièges à pression.
- Il ramasse les **petits objets** (petite clé, engrenage).
- **Limite :** il ne peut actionner ni porte ni levier.

### Oiseau
- **Fonction :** voler.
- Il **survole les fosses, l'eau et les pics**.
- Il **accroche des objets légers** pour les déposer ailleurs.
- **Limite :** le vol est limité par une jauge, ou bien il est bloqué par les courants d'air.

### Golem de pierre
- **Fonction :** servir de poids et de bouclier.
- Il **reste en place quand l'âme le quitte** et sert alors de poids permanent sur une plaque.
- Il **bloque les projectiles** et résiste au feu et à la lave.
- **Limite :** il ne bouge que s'il est possédé et il est très lent.

### Slime
- **Fonction :** se diviser et coller.
- Il **se divise en deux** pour tenir deux plaques à la fois, puis se reforme.
- Il passe **sous les portes**.
- Il **éteint les flammes**.
- **Limite :** il est très lent et se dissout dans l'eau.

---

## 3. Les objets et éléments de décor

### Mécanismes
| Objet | Qui l'utilise | Effet |
|---|---|---|
| **Levier** | Humain | Change d'état (on/off) : porte, pont, plateforme |
| **Bouton mural** | Humain | Effet temporaire (minuterie) |
| **Plaque de pression** | Tout corps sauf le Rat (pas l'âme) | Active tant qu'un poids est dessus |
| **Plaque lourde** | Ogre, Golem, bloc lourd | Ne réagit qu'à un gros poids |
| **Rune magique** | Sorcier | S'allume à distance |
| **Engrenage manquant** | Rat (le ramasse), Humain (le pose) | Répare une machine |

### Obstacles
| Obstacle | Bloque | Franchi par |
|---|---|---|
| **Grille** | Tous les corps | L'âme |
| **Mur fissuré** | Tout le monde | L'Ogre (le détruit) |
| **Trou de souris / conduit** | Les gros corps | Le Rat, le Slime |
| **Fosse / vide** | Les marcheurs | L'Oiseau, ou un pont abaissé par levier |
| **Eau profonde** | Les marcheurs | L'Oiseau (au-dessus) ; le Slime s'y dissout |
| **Lave** | Tout le monde | Le Golem |
| **Plancher fragile** | Rien (il cède sous l'Ogre) | Crée un passage vers l'étage du dessous |
| **Lumière sacrée** | L'âme | Un corps possédé, ou un miroir pour dévier le rayon |
| **Flammes bleues** | L'âme | Le Slime (les éteint) |

### Objets déplaçables
| Objet | Comment le déplacer |
|---|---|
| **Caisse en bois** | L'Humain la pousse, l'Ogre la casse |
| **Bloc de pierre** | L'Ogre le pousse, le Sorcier le soulève |
| **Clé** | L'Humain la porte, le Rat porte une petite clé, l'Oiseau la transporte, le Sorcier la fait léviter |
| **Miroir orientable** | L'Humain le tourne ; il dévie les rayons de lumière |
| **Orbe d'énergie** | Le Sorcier la place sur un socle pour alimenter une porte |
| **Tonneau explosif** | Allumé par une flamme ; il détruit les murs fissurés à distance |

### Dangers
| Élément | Effet |
|---|---|
| **Tourelle à projectiles** | Bloquée par le Golem |
| **Courant d'air** | Repousse l'Oiseau et l'âme |
| **Cristal d'expulsion** | Éjecte l'âme de son hôte quand il passe à côté |
| **Autel de purification** | Rend un hôte non possédable pendant X secondes |

### Spécifiques à l'âme
| Élément | Effet |
|---|---|
| **SoulPortal** *(existant)* | Téléportation réservée à l'âme |
| **Lanterne d'âme** | L'âme s'y loge pour éclairer une zone sombre ou révéler des passages cachés |
| **Réceptacle vide** | Un corps-statue que l'âme anime pour actionner un mécanisme fixe |

---

## 4. Combos d'énigmes (exemples)

1. **Le relais :** l'âme traverse la grille, possède l'Humain, tire le levier, se libère, puis passe le pont.
2. **Le poids oublié :** le Golem reste sur une plaque pendant que l'âme part posséder un autre hôte.
3. **La livraison :** le Rat passe par le conduit et récupère la clé, l'âme le quitte, possède l'Humain de l'autre côté, et le Rat lâche la clé devant la porte.
4. **Le pont magique :** le Sorcier fait léviter un bloc au-dessus d'une fosse, puis l'Ogre le pousse dedans pour combler le trou.
5. **Le rayon :** l'Humain tourne le miroir pour dévier la lumière sacrée qui bloquait l'âme.
6. **La chute :** l'Ogre marche sur un plancher fragile, tombe à l'étage du dessous et y casse un mur.
7. **Le double :** le Slime se divise pour tenir deux plaques à la fois, pendant que l'âme passe la porte.
8. **Le coursier :** l'Oiseau survole l'eau avec un engrenage et le dépose près d'une machine que l'Humain répare.
9. **Le chronomètre :** l'Humain appuie sur un bouton temporisé, se libère, et l'âme doit atteindre le SoulPortal avant la fin.
10. **La chaîne (fin de jeu) :** le Golem tient une plaque lourde, le Sorcier dépose l'orbe au-dessus de la lave, puis le Slime éteint les flammes bleues pour laisser passer l'âme.

---

## 5. Progression de la difficulté

| Monde | Hôtes débloqués | Nouveaux éléments | Type d'énigmes |
|---|---|---|---|
| **1. Le Donjon** | Humain | Portes, leviers, grilles, SoulPortal | Une seule capacité, un seul hôte |
| **2. Les Égouts** | + Rat | Conduits, petits objets, engrenages | Alterner entre 2 hôtes |
| **3. La Forteresse** | + Ogre | Murs fissurés, plaques lourdes, caisses | Ordre des actions |
| **4. La Tour du Mage** | + Sorcier | Runes, orbes, objets en hauteur | Verticalité, objets à distance |
| **5. Les Cavernes** | + Golem, Slime | Lave, planchers fragiles, flammes bleues | Laisser des hôtes en place, gestion du poids |
| **6. Les Falaises** | + Oiseau | Fosses, eau, courants d'air | Transport aérien, chemins alternatifs |
| **7. Le Sanctuaire** | — | Lumière sacrée, miroirs, cristaux d'expulsion, autels | L'âme elle-même est menacée ; combinaisons de 3 hôtes ou plus |

**Principes de design :**
- Chaque monde **introduit** une mécanique seule, puis la **combine** avec les précédentes, puis la **détourne** (par exemple, l'Ogre qui casse un plancher par erreur).
- Le joueur doit toujours pouvoir **voir** ce qui bloque, et comprendre quel hôte peut le franchir (couleur ou icône par type d'obstacle).
- Une **salle bonus** optionnelle par monde, avec une énigme difficile et une récompense cosmétique pour l'âme.

---

## 6. Pistes techniques (architecture actuelle)

- **Classe `Host` de base** et une sous-classe par type (`Ogre`, `Wizard`…), avec une méthode virtuelle `UseAbility()`.
- **Interface `IInteractable`** (leviers, portes, runes) avec un `CanBeUsedBy(Host host)`, pour que chaque objet décide quels hôtes il accepte.
- **Collision layers** par type de corps : âme, petit, normal, lourd, volant. Les grilles et conduits filtrent par layer.
- **Signaux** comme actuellement : un objet *demande* et la zone ou le gestionnaire *exécute*.
