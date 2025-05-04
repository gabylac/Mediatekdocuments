# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL.<br>
Voici le lien vers le Readme du dépôt d’origine présentant l’application d’origine:<br>
https://github.com/CNED-SLAM/MediaTekDocuments.git
# Fonctionnalités ajoutées
Ajout de 3 onglets permettant la gestion des commandes de livres, de DVD et de revues.<br>
## Onglet commande livres :
Cet onglet permet l’affichage des détails d’un livre et de la liste de ses commandes suite à la saisie de son numéro de document.<br>
![Capture d'écran 2025-05-04 231259](https://github.com/user-attachments/assets/91122134-a054-4661-83de-a0b8fb2eb4d2)
## Recherche
Par numéro : en cliquant sur le bouton « Rechercher » après la saisie d’un numéro de livre, les détails (code ISBN, titre, auteur(e), collection, genre, public, rayon, chemin de l'image et l'image) concernant le livre en question ainsi que ses commandes apparaissent.<br>
## Tri
Le fait de cliquer sur le titre d'une des colonnes de la liste des livres, permet de trier la liste par rapport à la colonne choisie.<br>
## Suppression
Il est possible de supprimer une commande d’un livre uniquement si sont statut n’est pas à « livrée ».<br>
## Modification
Il est possible de modifier le statut d’une commande en sélectionnant dans le combo le statut désiré. Une commande « livrée » ne peut pas revenir à un statut « en cours » ou « relancé », une commande ne peut également pas passer au statut « réglée » si elle n’a pas été auparavant « livrée ».<br>
## Création d’une commande
La partie basse de l’onglet permet la saisie d’une nouvelle commande pour le livre dont on a saisi le numéro en haut de la page. La date par défaut est celle du jour, la saisie d’un nombre d’exemplaires et d’un montant sont nécessaires pour valider une commande. Le clic sur le bouton « Enregistrer » permet d’ajouter la commande à la liste des commandes, le statut par défaut est « en cours ».<br>
## Onglet commande DVD
Cet onglet permet l’affichage des détails d’un DVD et de la liste de ses commandes suite à la saisie de son numéro de document.<br>
Les fonctionnalités sont identiques à l’onglet commande livre.<br>
## Onglet abonnement revue
Cet onglet permet l’affichage des détails d’une revue et de la liste de ses commandes suite à la saisie de son numéro de document.<br>
![Capture d'écran 2025-05-04 231707](https://github.com/user-attachments/assets/d2fc003a-29fa-4b83-ae0e-19c5a8236e5e)
Les fonctionnalités sont sensiblement identiques à l’onglet commande livre. Les différences résident :<br>
-	au niveau de l’affichage des commandes, ces dernières correspondent aux abonnements d’une revue, la date de fin d’abonnement s’affiche à la place du nombre d’exemplaires et du suivi<br>
-	il n’y a pas de modification du suivi, la réception d’un exemplaire d’une revue se fait au niveau de l’onglet parution<br>
-	au niveau de la commande : la date de fin d’abonnement a remplacé le nombre d’exemplaires<br>
## Affichage de la fenêtre d’alerte
Une fenêtre alertant des abonnements arrivant à échéance dans moins d’un mois s’affiche après l’authentification, avec une liste contenant le titre de la revue concernée et la date de fin d’abonnement. Le clic sur le bouton « ok » permet d’accéder au premier onglet livre de l’application.<br>
![Capture d'écran 2025-05-04 231936](https://github.com/user-attachments/assets/9802f413-35e7-4d4a-a87c-2a342808fd42)
## Authentification
Au démarrage de l’application une fenêtre d’authentification apparaît et permet à un utilisateur de saisir ses login et mot de passe. S’il n’est pas connu en base de données ou qu’il y a une erreur de saisie, l’authentification aura échoué et l’application affichera un message d’erreur indiquant que l’authentification a échouée à cause d’identifiants incorrects. Si l’utilisateur, connu en base de données, tente de s’authentifier alors qu’il n’a pas les droits (cas des personnes du service Culture), alors l’application affiche un message indiquant que l’utilisateur n’a pas les droits et se ferme immédiatement. Enfin si l’authentification d’un utilisateur, connu de la base de données, réussie, ce dernier pourra accéder aux fonctionnalités de l’application auxquelles il a les droits.<br>
![Capture d'écran 2025-05-04 232025](https://github.com/user-attachments/assets/19d54f5d-94e5-466a-8043-547f0d3089a1)
# Documentation technique
La documentation technique en ligne est disponible en cliquant ici
# Installeur
Un installeur a été créé afin de pouvoir installer l’application sur n’importe quel bureau sans passer par Visual Studio.<br> 
Pour installer l’application il suffit de double cliquer sur le ficher MediatekDocumentsSetup.msi et de suivre les instructions. Un raccourci sera alors installé sur le bureau une fois l’installation réussie. Le double clic sur le raccourci permet de lancer l’application.<br>













