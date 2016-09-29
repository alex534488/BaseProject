using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Manager
{
    public class SoundManager : BaseManager
    {
        [System.Serializable]
        public class VolumeSave
        {
            //Value '0' is the default setting. 
            public float master = 0;
            public float voice = 0;
            public float sfx = 0;
            public float music = 0;
        }
        public AudioSource stdSource;
        public AudioSource musicSource;
        public AudioMixer mixer;
        public VolumeSave save;

        public override void Init()
        {
            base.Init();
            Load();
            CompleteInit();
        }

        /// <summary>
        /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
        /// </summary>
        public void Play(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
        {
            if (delay > 0)
            {
                StartCoroutine(PlayIn(clip, delay, volume, source));
                return;
            }
            AudioSource theSource = source;
            if (theSource == null) theSource = stdSource;

            stdSource.PlayOneShot(clip, delay);
        }

        public void PlayMusic(AudioClip clip, bool looping = true, float volume = 1, bool faded = false)
        {
            if (faded)
            {
                StopMusic(true, delegate ()
                {
                    PlayMusic(clip, looping, volume);
                });
            }
            else
            {
                musicSource.volume = volume;
                StopMusic();
                musicSource.clip = clip;
                musicSource.loop = looping;
                musicSource.Play();
            }
        }

        public void StopMusic(bool faded = false, TweenCallback onComplete = null)
        {
            if (faded)
            {
                Tweener tween = DOTween.To(() => musicSource.volume, x => musicSource.volume = x, 0, 0.5f).OnComplete(delegate ()
                {
                    StopMusic(false, onComplete);
                });
            }
            else
            {
                musicSource.Stop();
                if (onComplete != null) onComplete.Invoke();
            }
        }

        IEnumerator PlayIn(AudioClip clip, float delay, float volume = 1, AudioSource source = null)
        {
            yield return new WaitForSecondsRealtime(delay);
            Play(clip, 0, volume, source);
        }

        #region Volume Set

        public void SetMaster(float value)
        {
            save.master = value;
            mixer.SetFloat("master", value);
        }
        public void SetVoice(float value)
        {
            save.voice = value;
            mixer.SetFloat("voice", value);
        }
        public void SetMusic(float value)
        {
            save.music = value;
            mixer.SetFloat("music", value);
        }
        public void SetSfx(float value)
        {
            save.sfx = value;
            mixer.SetFloat("sfx", value);
        }

        private void ApplyAll()
        {
            mixer.SetFloat("master", save.master);
            mixer.SetFloat("sfx", save.sfx);
            mixer.SetFloat("voice", save.voice);
            mixer.SetFloat("music", save.music);
        }

        #endregion

        #region Load/Save

        public void Load()
        {
            string savePath = Application.persistentDataPath + "/Sound.dat";
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                VolumeSave saveCopy = (VolumeSave)bf.Deserialize(file);
                save.master = saveCopy.master;
                save.voice = saveCopy.voice;
                save.sfx = saveCopy.sfx;
                save.music = saveCopy.music;
                file.Close();
            }
            else
            {
                save = new VolumeSave();
                Save();
            }

            ApplyAll();
        }

        public void Save()
        {
            string savePath = Application.persistentDataPath + "/Sound.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
            bf.Serialize(file, save);
            file.Close();
        }

        public void Clear()
        {
            save = new VolumeSave();
            Save();
        }

        #endregion


#if UNITY_EDITOR
        [CustomEditor(typeof(SoundManager))]
        public class SoundManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                SoundManager manager = target as SoundManager;

                if (GUILayout.Button("Clear"))
                {
                    manager.Clear();
                }
                if (GUILayout.Button("Save"))
                {
                    manager.Save();
                }
                if (GUILayout.Button("Load"))
                {
                    manager.Load();
                }
            }
        }
#endif
    }
}
