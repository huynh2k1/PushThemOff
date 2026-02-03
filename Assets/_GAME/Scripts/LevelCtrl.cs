using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] Transform _gameplayParent;
    [SerializeField] PlayerCtrl _player;
    [SerializeField] TextAsset _data;
    [SerializeField] private Transform _mapRoot; // Gán MapTransform trong Inspector    

    #region LIFECYCLE   
    public void OnStartGame()
    {
        //OrderId = idLevel;
        InitPlayer();
        LoadData();
    }

    public void InitPlayer()
    {
        _player.OnInitGame();
    }
    #endregion

    public int OrderId;
    const string pathSave = "";
    const string fileName = "AllLevel.json";

    public int GetMaxLevelOrder()
    {
        string json = _data.text;
        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
            return 0;

        return dataContainer.levels.Count;  
    }

    string GetFilePath()
    {
        string directory = Path.Combine(Application.dataPath, pathSave);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log("Created directory: " + directory);
        }

        return Path.Combine(directory, fileName);
    }

    void ApplyMap(int mapID)
    {
        foreach (Transform map in _mapRoot)
        {
            map.gameObject.SetActive(false);
        }

        foreach (Transform map in _mapRoot)
        {
            MapIdentifier id = map.GetComponent<MapIdentifier>();
            if (id != null && id.MapId == mapID)
            {
                map.gameObject.SetActive(true);
                return;
            }
        }

        Debug.LogError("Không tìm thấy map có MapId = " + mapID);
    }

    private int GetCurrentMapId()
    {
        foreach (Transform map in _mapRoot)
        {
            if (map.gameObject.activeSelf)
            {
                MapIdentifier id = map.GetComponent<MapIdentifier>();
                if (id != null)
                    return id.MapId;

                Debug.LogError("Map chưa có MapIdentifier: " + map.name);
            }
        }

        Debug.LogWarning("Không có map nào active, mặc định MapId = 0");
        return 0;
    }



    [Button("Save Data To Json")]
    public void Save()
    {
        string path = GetFilePath();

        DataContainer dataContainer = new DataContainer();

        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            dataContainer = JsonUtility.FromJson<DataContainer>(existingJson);
        }

        if (dataContainer.levels == null)
        {
            dataContainer.levels = new List<LevelData>();
        }

        LevelData newLevel = new LevelData
        {
            OrderId = this.OrderId,
            MapId = GetCurrentMapId(),
            PlayerData = new PlayerData
            {
                Transform = TransformObj.FromTransform(_playerTransform)
            },
            listEnemy = new List<EnemyData>()
        };

        List<BaseCharacter> enemies = transform.GetComponentsInChildren<BaseCharacter>().ToList();

        foreach (BaseCharacter e in enemies)
        {
            EnemyData enemyData = new EnemyData
            {
                Transform = TransformObj.FromTransform(e.transform),
            };

            newLevel.listEnemy.Add(enemyData);
        }

        int existingIndex = dataContainer.levels.FindIndex(l => l.OrderId == OrderId);
        if (existingIndex >= 0)
        {
            dataContainer.levels[existingIndex] = newLevel;
            Debug.Log($"Level {OrderId} đã được ghi đè");
        }
        else
        {
            dataContainer.levels.Add(newLevel);
            Debug.Log($"Level {OrderId} đã được thêm mới");
        }

        string json = JsonUtility.ToJson(dataContainer, true);
        File.WriteAllText(path, json);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("Level saved to: " + path);
    }

    [Button("Load Data")]
    public void LoadData()
    {
        string json = _data.text;

        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
        {
            Debug.LogError("File JSON không hợp lệ hoặc không có dữ liệu level!");
            return;
        }

        LevelData levelData = dataContainer.levels.FirstOrDefault(l => l.OrderId == OrderId);
        if (levelData == null)
        {
            Debug.LogError($"Không tìm thấy Level có OrderId = {OrderId}");
            return;
        }

        // ================= MAP =================
        ApplyMap(levelData.MapId);

        // ================= PLAYER =================
        if (levelData.PlayerData != null)
        {
            levelData.PlayerData.Transform.ApplyTo(_playerTransform);
        }
        else
        {
            Debug.LogWarning("PlayerData hoặc Transform bị null, bỏ qua load player.");
        }

        // ================= CLEAR ENEMY =================
//        List<M1_Enemy> existingEnemies = transform.GetComponentsInChildren<M1_Enemy>().ToList();
//        foreach (M1_Enemy enemy in existingEnemies)
//        {
//#if UNITY_EDITOR
//            if (!Application.isPlaying)
//                DestroyImmediate(enemy.gameObject);
//            else
//                Destroy(enemy.gameObject);
//#else
//        Destroy(enemy.gameObject);
//#endif
//        }

        // ================= SPAWN ENEMY =================
        if (levelData.listEnemy == null || levelData.listEnemy.Count == 0)
        {
            Debug.LogWarning("Level không có enemy nào để load.");
            return;
        }

        foreach (EnemyData enemyData in levelData.listEnemy)
        {
            //if (enemyData == null || enemyData.Transform == null)
            //{
            //    Debug.LogWarning("EnemyData hoặc Transform bị null, bỏ qua enemy này.");
            //    continue;
            //}

            // Tạo enemy mới (không set transform ở Instantiate để tránh set 2 lần)
            //M1_Enemy newEnemy = Instantiate(_enemyPrefab, this.transform);
            //enemyData.Transform.ApplyTo(newEnemy.transform);

        }

        Debug.Log($"Load Level {OrderId} thành công. Enemy count: {levelData.listEnemy.Count}");
    }
}
