using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using UnityEngine.Events;
using CCC.Manager;

namespace CCC.Utility
{
    public class ThreadSave
    {
        static public void Save(string path, object graph, UnityAction onComplete = null)
        {
            Thread t = new Thread(new ThreadStart(() => SaveMethod(path, graph, onComplete)));
            t.Start();
        }

        static void SaveMethod(string path, object graph, UnityAction onComplete = null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            bf.Serialize(file, graph);
            file.Close();

            lock (MainThread.instance)
            {
                MainThread.AddAction(onComplete);
            }
        }

        static public void Load(string path, UnityAction<object> onComplete)
        {
            Thread t = new Thread(new ThreadStart(delegate () { LoadMethod(path, onComplete); }));
            t.Start();
        }

        static void LoadMethod(string path, UnityAction<object> onComplete)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            object obj = bf.Deserialize(file);
            file.Close();

            lock (MainThread.instance)
            {
                if (onComplete != null)
                    MainThread.AddAction(delegate ()
                    {
                        onComplete(obj);
                    });
            }
        }

        static public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
