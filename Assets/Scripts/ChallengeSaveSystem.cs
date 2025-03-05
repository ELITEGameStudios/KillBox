using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class ChallengeSaveSystem
{

    public static void SaveChallenges (){

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/challenges.kbxc";
        FileStream stream = new FileStream(path, FileMode.Create);

        ChallengeData data = new ChallengeData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ChallengeData LoadChallenges(){
        string path = Application.persistentDataPath + "/challenges.kbxc";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ChallengeData loadedData = formatter.Deserialize(stream) as ChallengeData;
            stream.Close();

            return loadedData;
        }
        else
        {
            Debug.LogError("Challenge file not found ONONOON" + path);
            return null;
        }
    }

    public static void DeletePlayer(){
        string path = Application.persistentDataPath + "/challenges.kbxc";
        if (File.Exists(path)) {
            try{
                File.Delete(path);
            }
            catch(IOException e){
                return;
            }
        }
        else
        {
            Debug.LogError("Challenge file not found ONONOON" + path);
        }
    }
}
