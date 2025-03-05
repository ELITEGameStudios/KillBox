using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class KeybindsSave
{

    public static void SavePlayer (CustomKeybinds _keys){

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/keybinds.kbxk";
        FileStream stream = new FileStream(path, FileMode.Create);

        KeybindData data = new KeybindData(_keys);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static KeybindData LoadPlayer(){
        string path = Application.persistentDataPath + "/keybinds.kbxk";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            KeybindData loadedData = formatter.Deserialize(stream) as KeybindData;
            stream.Close();

            return loadedData;
        }
        else
        {
            Debug.LogError("Keybind file not found ONONOON" + path);
            return null;
        }
    }

    public static void DeletePlayer(){
        string path = Application.persistentDataPath + "/keybinds.kbxk";
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
            Debug.LogError("Keybinds file not found ONONOON" + path);
        }
    }

}
