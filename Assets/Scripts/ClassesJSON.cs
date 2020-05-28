using System;


[Serializable]
public class JSONData
{
    public string prenom;
    public Infos infos;
    public Question[] questions;
}

[Serializable]
public class Infos
{
    public int progression_monde;
    public int nb_pieces;
    public Matiere[] matieres;
}

[Serializable]
public class Matiere
{
    public string nom_matiere;
    public int nb_donjons;
    public int nb_donjons_finis;
}

[Serializable]
public class Question
{
    public int id;
    public string matiere;
    public string intitule;
    public string type;
    public bool strict_libre;
    public string[] reponses_libre;
    public string[] propositions_qcm;
    public int reponse_qcm;
    public bool deja_pose;
}
