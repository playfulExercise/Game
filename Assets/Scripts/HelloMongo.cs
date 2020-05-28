/**
 * Documentation of C# driver 1.10:
 * http://mongodb.github.io/mongo-csharp-driver/1.11/getting_started/
 * c# Driver for MongoDBprovided by http://answers.unity3d.com/questions/618708/unity-and-mongodb-saas.html
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Lists

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

public class HelloMongo : MonoBehaviour {

	string connectionString = "mongodb://localhost:27017";
	public BsonDocument infos;
	public BsonArray questions;
	public string eleve_prenom;
	public InputField codeText;

	public bool connected = false;

	public string error;


	private ConnectionManager connectionManager;

	private void Awake(){
		connectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<ConnectionManager>();
	}

	// LE START C'EST UNE DOCUMENTATION UTILE DE TOUT CE QU'ON PEUT FAIRE EN C# sur une Mongo
	void Start () {

		/*
		 * 1. establish connection
		 */

		/*var client = new MongoClient (connectionString);
		var server = client.GetServer ();
		var database = server.GetDatabase ("playful");
		var professeurcollection = database.GetCollection<BsonDocument> ("professeurs");
		Debug.Log ("1. ESTABLISHED CONNECTION");*/

		/*
		 * 2. INSERT new dataset: INSERT INTO players (email, name, scores, level) VALUES("..","..","..","..");
		 */

		/*professeurcollection.Insert(new BsonDocument{
			{ "level", 7 },
			{ "name", "Fabi" },
			{ "scores", 4711 },
			{ "email", "ff023@hdm-s.de" }
		});
		Debug.Log ("2. INSERTED A DOC");

		*/

		/*
		 * 3. INSERT multiple datasets
		 */

		/*BsonDocument [] batch={
			new BsonDocument{
				{ "level", 3 },
				{ "name", "Kaki" },
				{ "scores", 27017 },
				{ "email", new BsonDocument{
					{ "private", "k@k.i" },
					{ "business", new BsonArray{
						{"mr@ka.ki"},{"dr@ka.ki"}
					}}
				}}
			}, 
			new BsonDocument{
				{ "level", 5 },
				{ "name", "Gabi" },
				{ "scores", 6454 },
				{ "email", "g@b.i" }
			},
			new BsonDocument{
				{ "level", 5 },
				{ "name", "Vati" },
				{ "scores", 1936 },
				{ "email", "g@b.i" }
			} 
		};
		professeurcollection.InsertBatch (batch);
		Debug.Log ("3. INSERTED MULTIPLE DOCS");*/

		/*
		 * 4. SELECT * FROM professeurs
		 */

		/*foreach (var document in professeurcollection.FindAll ()) {
			Debug.Log ("4. SELECT ALL DOCS: \n" + document);
		}*/

		// logs
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad29"), "level" : 7, "name" : "Fabi", "scores" : 4711, "email" : "ff023@hdm-s.de" }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2a"), "level" : 3, "name" : "Kaki", "scores" : 27017, "email" : { "private" : "k@k.i", "business" : ["mr@ka.ki", "dr@ka.ki"] } }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2b"), "level" : 5, "name" : "Gabi", "scores" : 6454, "email" : "g@b.i" }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2c"), "level" : 5, "name" : "Vati", "scores" : 1936, "email" : "g@b.i" }

		/*
		 * 5. SELECT the first doc
		 */

		// Debug.Log ("5. SELECT FIRST DOC: \n" + professeurcollection.FindOne ().ToString ());

		// logs { "_id" : ObjectId("56d203d42a1e03167dc1ad29"), "level" : 7, "name" : "Fabi", "scores" : 4711, "email" : "ff023@hdm-s.de" }

		/*
		 * 6. SELECT * FROM players WHERE name = 'Fabi'
		 */

		// foreach (var document in professeurcollection.Find (new QueryDocument ("name", "Fabi"))) {
		// 	Debug.Log ("6. SELECT DOC WHERE: \n" + document);
		// }

		// logs { "_id" : ObjectId("56d203d42a1e03167dc1ad2a"), "level" : 3, "name" : "Kaki", "scores" : 27017, "email" : { "private" : "k@k.i", "business" : ["mr@ka.ki", "dr@ka.ki"] } }

		/*
		 * 7. macht dasselbe 
		 */

		// var query7 = new QueryDocument ("name", "Gabi");
		// //var query7 = Query.EQ("name", "Gabi"); // same as line above
		// foreach (var document in professeurcollection.Find (query7)) {
		// 	Debug.Log ("7. SELECT DOC WHERE: \n" + document);
		// }

		// logs { "_id" : ObjectId("56d203d42a1e03167dc1ad2b"), "level" : 5, "name" : "Gabi", "scores" : 6454, "email" : "g@b.i" }

		/*
		 * 8. SELECT scores FROM players WHERE name = 'Fabi'
		 */

		// foreach (var document in professeurcollection.Find (new QueryDocument ("name", "Kaki"))) {
		// 	Debug.Log ("8. SELECT DOC PART WHERE: \n" + document["scores"]);
		// }

		// logs 27017

		/*
		 * 9. SELECT scores FROM players WHERE {[{nested}]}
		 */

		// foreach (var document in professeurcollection.Find (new QueryDocument ("prenom_prof", "test"))) {
		// 	// Debug.Log ("9. SELECT NESTED DOC PART WHERE:  \n" + document["email"]["business"][0]);
		// 	Debug.Log ("9. SELECT NESTED DOC PART WHERE:  \n" + document["eleves"][0]["infos"]["matieres"]);
		// 	//synctestnommat = (document["eleves"][0]["infos"]["matieres"][2]["nom_matiere"]).ToString();
		// 	//Debug.Log((document["eleves"][0]["infos"]["matieres"][2]["nom_matiere"]).ToString());
		// 	Debug.Log ((document["_id"]).ToString ());

		// }

		// logs mr@ka.ki

		/*
		 * 10. SELECT * FROM players -> Limit 2 entries
		 */

		// foreach (var document in professeurcollection.FindAll ().SetLimit (2)) {
		// 	Debug.Log ("10. SELECT DOC WHERE LIMIT: \n" + document);
		// }

		// logs 
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad29"), "level" : 7, "name" : "Fabi", "scores" : 4711, "email" : "ff023@hdm-s.de" }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2a"), "level" : 3, "name" : "Kaki", "scores" : 27017, "email" : { "private" : "k@k.i", "business" : ["mr@ka.ki", "dr@ka.ki"] } }

		/*
		 * 11. DELETE FROM players WHERE name = "Vati"
		 */

		// professeurcollection.Remove (Query.EQ ("name", "Vati"));
		// Debug.Log ("11. DELETE DOC WHERE (vati)");

		/*
		 * 12. SELECT * FROM players -> sort
		 */

		// foreach (var document in professeurcollection.FindAll ()
		// 		.SetSortOrder (SortBy.Descending ("scores"))) {
		// 	Debug.Log ("12. SELECT DOC SORT: \n" + document);
		// }

		// logs 
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2a"), "level" : 3, "name" : "Kaki", "scores" : 27017, "email" : { "private" : "k@k.i", "business" : ["mr@ka.ki", "dr@ka.ki"] } }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2b"), "level" : 5, "name" : "Gabi", "scores" : 6454, "email" : "g@b.i" }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad29"), "level" : 7, "name" : "Fabi", "scores" : 4711, "email" : "ff023@hdm-s.de" }

		/*
		 * 13. SELECT * FROM players WHERE scores > 4712
		 */

		// var query13 = Query.GT ("scores", 4712);
		// foreach (var document in professeurcollection.Find (query13)) {
		// 	Debug.Log ("13. SELECT DOC WHERE GREATER THAN: \n" + document);
		// }

		// logs 
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2a"), "level" : 3, "name" : "Kaki", "scores" : 27017, "email" : { "private" : "k@k.i", "business" : ["mr@ka.ki", "dr@ka.ki"] } }
		// { "_id" : ObjectId("56d203d42a1e03167dc1ad2b"), "level" : 5, "name" : "Gabi", "scores" : 6454, "email" : "g@b.i" }

		/*
		 * 14. UPDATE players SET level = 11 WHERE name = "Fabi AND email = "ff023"
		 */

		/*var where14 = new QueryDocument{
			{"name", "Gabi"},
			{"email", "g@b.i"}
		};
		var set14 = new UpdateDocument {
			{ "$set", new BsonDocument ("level", 11) }
		};
		professeurcollection.Update (where14, set14);
		Debug.Log ("14. UPDATE DOC SET WHERE");*/

		/*
		 * 15. If value exists: UPDATE, if not: INSERT
		 */

		/*var whereClause15 = Query.And(
			Query.EQ("name", "Fabi")
		);
		BsonDocument query15 = professeurcollection.FindOne(whereClause15);
		if (query15 != null) {
			query15["level"] = 11;
			professeurcollection.Save(query15);
		}
		Debug.Log ("15. UPDATE DOC SET WHERE / IF NOT EXISTS INSERT");*/

		/*
		 * 16. COUNT docs
		 */

		// Debug.Log ("16. COUNT DOCS: \n" + professeurcollection.Count ());

		// logs 3

		/*
		 * 17. INSERT new dataset: INSERT INTO players (email, name, scores, level) VALUES("..","..","..","..");
		 */

		/*professeurcollection.Insert(new BsonDocument{
			{ "level", 2 },
			{ "name", "Kati" },
			{ "scores", 8828 },
			{ "email", "k@t.i" },
			{ "fix_date", new DateTime(2015,2,23, 0,0,0, DateTimeKind.Utc) },
			{ "current_date", DateTime.Now },
			{ "date_utcNow", DateTime.UtcNow }

		});
		Debug.Log ("17. INSERTED A DOC with some date formats");*/

		/*
		 * 18. SELECT scores FROM players WHERE {[{nested}]}
		 */

		/*DateTime fix_date = new DateTime();
		DateTime date_utcNow = new DateTime();
		DateTime nodejs_now = new DateTime();

		foreach (var document in professeurcollection.Find(new QueryDocument("name", "Kati"))){
			Debug.Log ("18. SELECT DOC PART WHERE DATE: fix_date: \n" + document["fix_date"]);
			Debug.Log ("current_date: \n" + document["current_date"]);
			Debug.Log ("date_utcNow: \n" + document["date_utcNow"]);

			fix_date = document["fix_date"].ToUniversalTime();
			date_utcNow = document["current_date"].ToUniversalTime();

			//Debug.Log ("node: \n" + document);
		}*/

		// logs
		// fix_date:     2015-02-23T00:00:00Z       In MongoDB saved as ISODate("2015-02-23T00:00:00Z"), not just as string
		// current_date: 2016-02-28T10:38:22.719Z   In MongoDB saved as ISODate("2016-02-28T10:38:22.719Z"), not just as string
		// date_utcNow:  2016-02-28T10:38:22.719Z   In MongoDB saved as ISODate("2016-02-28T10:38:22.719Z"), not just as string

		// Note: Ich accessing date from Node.js, the date object must be created directly server-side (in app.js) and not client-side and then over socket.io
		// data.timestamp = new Date(Date.now());
		// In order to test, create a dataset with name="Test" in Node.js, store it in MongoDB and then call the following function
		/*
		foreach (var document in professeurcollection.Find(new QueryDocument("name", "Test"))){
			Debug.Log ("node: \n" + document["timestamp"]);
			nodejs_now = document["timestamp"].ToUniversalTime();
		}
		*/

		//if (date_utcNow > fix_date) Debug.Log("date_utcNow ist aktueller als fix_date");
		// if (nodejs_now > fix_date) Debug.Log("nodejs_now ist aktueller als fix_date");
	}
	public void connectEleveAndReturnData () {
		var client = new MongoClient (connectionString);
		var server = client.GetServer ();
		var database = server.GetDatabase ("playful");
		var professeurcollection = database.GetCollection<BsonDocument> ("professeurs");
		Debug.Log ("1. ESTABLISHED CONNECTION");
		foreach (var document in professeurcollection.FindAll ()) {
			try {
				var nb_eleves = document["eleves"].AsBsonArray.Count;
				if (nb_eleves >= 1) {
					for (int i = 0; i < nb_eleves; ++i) {
						if (document["eleves"][i]["code_eleve"].ToString () == codeText.text.ToLower ()) {
							infos = document["eleves"][i]["infos"].AsBsonDocument;
							eleve_prenom = document["eleves"][i]["prenom_eleve"].ToString ();
							questions = document["questions"].AsBsonArray;
							connected = true;
							break;
						} else {
							connected = false;

						}
					}
				}

			} catch (KeyNotFoundException e) {
				Debug.Log (e);
			}

		}
		if (connected) {
			Debug.Log ("joueur connecte - mdp ok");
			error = "La clé de connexion est correcte.";
			connectionManager.SetInfos(infos);
			connectionManager.SetQuestions(questions);
			connectionManager.SetElevePrenom(eleve_prenom);
			connectionManager.Connect();
		} else {
			Debug.Log ("connection err - mdp incorrect");
			error = "La clé de connexion est incorrecte.";
		}
	}
}