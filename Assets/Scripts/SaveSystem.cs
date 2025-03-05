using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{

    public static void SavePlayer (GameManager gameData){

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.bruh";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(){
        string path = Application.persistentDataPath + "/progress.bruh";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData loadedData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return loadedData;
        }
        else
        {
            Debug.LogError("Save file not found ONONOON" + path);
            return null;
        }
    }

    public static void DeletePlayer(){
        string path = Application.persistentDataPath + "/progress.bruh";
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
            Debug.LogError("Save file not found ONONOON" + path);
        }
    }

}
