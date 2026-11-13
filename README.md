Étape 0 : Initialisation & Configuration Godot (Mobile Paysage & Résolution)
Objectif : Poser les fondations techniques pour un affichage net et des contrôles tactiles impeccables.

Création du projet : Ouvre Godot, crée un projet 2D vide.

Paramètres de la fenêtre (Project Settings > Display > Window) :

Orientation : Landscape (Paysage forcé).

Taille de base (Viewport Width / Height) : 640 x 360 (ratio 16:9 parfait pour le mobile).

Étirement (Stretch) : Mode canvas_items et Aspect keep.

Le rendu net : Va dans Rendering > Textures et règle le Default Texture Filter sur Nearest (évite tout effet flou).

Arborescence des dossiers : Crée des dossiers propres : scenes/, scripts/, assets/ (vides pour l'instant).

Étape 1 : Le Mouvement Fluide de l'Âme (Boîte Grise)
Objectif : Faire bouger une simple forme géométrique grise avec une inertie vaporeuse et aérienne.

Scène PlayerSoul.tscn :

Ajoute un nœud CharacterBody2D.

Ajoute un CollisionShape2D (forme de cercle, rayon de 6 pixels).

Ajoute un Sprite2D affichant une simple texture de carré ou de cercle gris clair (#cccccc).

Ajoute une Camera2D (zoom réglé à 2.0 ou 3.0).

Le script de déplacement (PlayerSoul.gd) :

Coder la récupération de la position du doigt ou de la souris à l'écran (get_global_mouse_position()).

Appliquer un mouvement fluide avec une interpolation (lerp) pour donner l'effet de flottement propre à une âme.

Vérification : Lance le jeu sur ton téléphone ou l'émulateur et assure-toi que la forme grise glisse de façon fluide sous ton doigt.

Étape 2 : Le Décor et les Collisions (La Boîte Grise Initiale)
Objectif : Construire une mini-salle fermée avec des murs pour tester les déplacements.

Scène principale (Main.tscn) :

Ajoute un nœud StaticBody2D pour dessiner les murs de la pièce à l'aide de rectangles gris sombres (#444444).

Associe des CollisionShape2D rectangulaires à chaque mur.

Vérification : Déplace ton âme grise : elle doit longer les murs sans pouvoir les traverser.

Étape 3 : Le Système de Possession (Hôtes en Boîte Grise)
Objectif : Pouvoir entrer dans un corps (carré d'une autre couleur), en prendre le contrôle et en ressortir.

Scène Host.tscn :

Ajoute un CharacterBody2D nommé Host.

Ajoute un CollisionShape2D et un Sprite2D affichant un carré de couleur différente (ex: bleu clair #3b82f6 pour l'humain).

Ajoute une Area2D (zone de détection) légèrement plus large autour du carré bleu.

La logique de possession :

Code une variable d'état is_possessed = false.

Lorsque l'âme entre dans l'aire de l'hôte et qu'un signal/bouton d'action est déclenché :

is_possessed = true.

Désactive le script de mouvement et masque le PlayerSoul (ou centre-le sur l'hôte).

Active le script de déplacement de l'hôte (déplacement à pied, plus lourd et plus lent).

Change la couleur du carré hôte (ex: passe du bleu au vert) pour indiquer qu'il est habité.

Sortir du corps :

Si le joueur appuie sur le bouton "Libérer", l'hôte s'immobilise, is_possessed redevient false, et l'âme grise réapparaît juste à côté de lui.

Étape 4 : Les Obstacles et la Logique "Clé / Serrure"
Objectif : Mettre en place la règle d'or des énigmes (l'âme passe partout, l'humain pousse/ouvre).

Différencier les déplacements :

L'âme (cercle gris) peut traverser une zone de "grille" (un simple rectangle semi-transparent).

L'humain (carré bleu) est bloqué par la grille, mais peut franchir ou actionner un déclencheur.

Créer une porte interactive (Door.tscn) :

Une zone rectangulaire qui vérifie quel corps entre en contact avec elle. Si c'est l'hôte humain, la porte (rectangle gris) s'efface ou s'ouvre.

Étape 5 : L'Interface Tactile (UI Mobile minimale)
Objectif : Poser les boutons indispensables pour jouer sur écran tactile en mode paysage.

Ajouter un CanvasLayer :

Crée un bouton contextuel "Posséder" qui s'affiche uniquement lorsque l'âme est à portée d'un hôte.

Crée un bouton "Quitter le corps" qui apparaît en haut de l'écran lorsque le joueur contrôle un hôte.

Étape 6 : Assemblage et Test du Niveau 1 (Prototype Valide)
Objectif : Concaténer toutes les briques grises dans une mini-map pour valider que le jeu est intuitif et amusant.

Mise en scène de la boîte grise :

Zone A (Départ) : L'âme grise apparaît seule derrière une grille infranchissable pour un humain.

Zone B (L'Hôte) : De l'autre côté de la grille, un carré bleu (l'hôte) est bloqué près d'une porte fermée.

Zone C (Résolution) : L'âme traverse la grille, possède le carré bleu, actionne l'ouverture de la porte, se libère, et sort de la zone.

Phase de test : Joue l'intégralité de cette boucle en boîte grise sur ton téléphone jusqu'à ce que la sensation de jeu soit fluide et sans accroc. (C'est seulement une fois cette étape validée à 100 % que tu pourras remplacer les carrés gris par tes futurs sprites 16x16 et tes lumières).