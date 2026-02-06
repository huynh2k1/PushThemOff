using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using H_Utils;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    //[SerializeField] private Transform _playerTransform;
    [SerializeField] Transform _gameplayParent;
    [SerializeField] TextAsset _data;
    [SerializeField] private Transform _mapRoot; // Gán MapTransform trong Inspector
                                                 // 
    [SerializeField] private AreaContainer _areaContainerPrefab;

    [Header("Enemy Prefabs")]
    [SerializeField] BaseEnemy _enemy1Prefab;
    [SerializeField] BaseEnemy _enemy2Prefab;
    [SerializeField] BaseEnemy _enemy3Prefab;
    [SerializeField] BaseEnemy _enemy4Prefab;
    [SerializeField] BaseEnemy _enemy5Prefab;

    LevelData _currentLevelData;
    int _currentAreaIndex;
    AreaContainer _currentArea;

    public static Action<Transform> OnPlayerInitAction;
    public static Action<int> OnAreaClearedGlobal;
    public event Action OnLevelCompletedEvent;

    #region LIFECYCLE   
    public void OnStartGame(int idLevel)
    {
        OrderId = idLevel;
        ClearLevel();
        LoadLevelDataOnLy();
        StartFirstArea();
    }

    public void OnGameWin()
    {
        if(GameDatas.CurrentLevel < GetMaxLevelOrder() - 1)
        {
            GameDatas.CurrentLevel++;
        }
        else
        {
            GameDatas.CurrentLevel = 0; 
        }
    }

    public void OnReplayGame(bool isWin)
    {
        if (!isWin)
            return;
        if (GameDatas.CurrentLevel > 0)
            GameDatas.CurrentLevel--;
    }

    public void ClearLevel()
    {
        StopAllCoroutines();

        // 🔹 Clear Area hiện tại (nếu còn)
        if (_currentArea != null)
        {
            _currentArea.OnAreaCleared -= HandleAreaCleared;
            Destroy(_currentArea.gameObject);
            _currentArea = null;
        }

        // 🔹 Xóa toàn bộ AreaContainer còn sót
        AreaContainer[] areas = _gameplayParent.GetComponentsInChildren<AreaContainer>();
        foreach (var area in areas)
        {
            Destroy(area.gameObject);
        }

        // 🔹 Xóa Player đã spawn
        PlayerCtrl player = _gameplayParent.GetComponentInChildren<PlayerCtrl>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        // 🔹 Reset map (tắt toàn bộ)
        foreach (Transform map in _mapRoot)
        {
            map.gameObject.SetActive(false);
        }

        // 🔹 Reset data runtime
        _currentLevelData = null;
        _currentAreaIndex = 0;

        Debug.Log("Cleaned Level → Back Home");
    }

    public void LoadLevelDataOnLy()
    {
        string json = _data.text;

        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(json);

        _currentLevelData =
        dataContainer.levels.FirstOrDefault(l => l.OrderId == OrderId);

        if (_currentLevelData == null)
        {
            Debug.LogError("Level not found " + OrderId);
            return;
        }


        if (_currentLevelData.PlayerData != null)
        {
            GameObject player = Instantiate(_playerPrefab.gameObject, _gameplayParent);
            _currentLevelData.PlayerData.Transform.ApplyTo(player.transform);
            OnPlayerInitAction?.Invoke(player.transform);   
            player.GetComponent<PlayerCtrl>().OnInit();
        }
        ApplyMap(_currentLevelData.MapId);
    }

    void StartFirstArea()
    {
        _currentAreaIndex = 0;
        SpawnCurrentArea();
    }

    void SpawnCurrentArea()
    {
        if (_currentLevelData == null ||
            _currentLevelData.listArea == null ||
            _currentAreaIndex >= _currentLevelData.listArea.Count)
        {
            OnLevelCompletedEvent?.Invoke();
            return;
        }

        var areaData = _currentLevelData.listArea
            .OrderBy(a => a.AreaId)
            .ToList()[_currentAreaIndex];

        AreaContainer areaContainer =
            Instantiate(_areaContainerPrefab, _gameplayParent);

        areaContainer.AreaId = areaData.AreaId;
        areaContainer.name = $"Area_{areaData.AreaId}";

        foreach (EnemyData enemyData in areaData.listEnemy)
        {
            BaseEnemy prefab = GetEnemyPrefab(enemyData.Type);
            if (prefab == null) continue;

            BaseEnemy enemy =
                Instantiate(prefab, areaContainer.transform);

            Debug.Log("Spawn Enemy: " + enemyData.Transform.Position);
            enemyData.Transform.ApplyTo(enemy.transform);
            enemy.OnInit();
        }

        _currentArea = areaContainer;

        _currentArea.Init();
        _currentArea.OnAreaCleared += HandleAreaCleared;
    }

    void HandleAreaCleared(AreaContainer area)
    {
        area.OnAreaCleared -= HandleAreaCleared;

        OnAreaClearedGlobal?.Invoke(_currentAreaIndex);

        Destroy(area.gameObject);

        _currentAreaIndex++;

        SpawnCurrentArea();
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

        Transform playerTranform = _gameplayParent.GetComponentInChildren<PlayerCtrl>().transform;
        LevelData newLevel = new LevelData
        {
            OrderId = this.OrderId,
            MapId = GetCurrentMapId(),
            PlayerData = new PlayerData
            {
                Transform = TransformObj.FromTransform(playerTranform)
            },
            listArea = new List<AreaData>()
        };

        AreaContainer[] areas = _gameplayParent.GetComponentsInChildren<AreaContainer>();

        for(int i = 0; i < areas.Length; i++)
        {
            AreaData areaData = new AreaData();
            areaData.AreaId = i;
            areaData.listEnemy = new List<EnemyData>(); 

            BaseEnemy[] enemies = areas[i].transform.GetComponentsInChildren<BaseEnemy>();

            foreach (BaseEnemy e in enemies)
            {
                EnemyData enemyData = new EnemyData
                {
                    Transform = TransformObj.FromTransform(e.transform),
                    Type = e.enemyType
                };

                areaData.listEnemy.Add(enemyData);
            }
            newLevel.listArea.Add(areaData);    
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
            GameObject player = Instantiate(_playerPrefab.gameObject, _gameplayParent);
            levelData.PlayerData.Transform.ApplyTo(player.transform);
        }
        else
        {
            Debug.LogWarning("PlayerData hoặc Transform bị null, bỏ qua load player.");
        }

        // ================= CLEAR ENEMY =================
        AreaContainer[] oldAreas = _gameplayParent.GetComponentsInChildren<AreaContainer>();

        foreach (var a in oldAreas)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(a.gameObject);
            else
                Destroy(a.gameObject);
#else
    Destroy(a.gameObject);
#endif
        }

        // ================= SPAWN ALL AREA (spawn cả AreaContainer) =================

        if (levelData.listArea == null || levelData.listArea.Count == 0)
        {
            Debug.LogWarning("Level không có area nào để load.");
            return;
        }

        foreach (AreaData area in levelData.listArea)
        {
            AreaContainer areaContainer =
                Instantiate(_areaContainerPrefab, _gameplayParent);

            areaContainer.AreaId = area.AreaId;
            areaContainer.name = $"Area_{area.AreaId}";

            if (area.listEnemy == null || area.listEnemy.Count == 0)
                continue;

            foreach (EnemyData enemyData in area.listEnemy)
            {
                BaseEnemy prefab = GetEnemyPrefab(enemyData.Type);

                if (prefab == null)
                    continue;

                BaseEnemy newEnemy =
                    Instantiate(prefab, areaContainer.transform);

                enemyData.Transform.ApplyTo(newEnemy.transform);
            }
        }
    }

    BaseEnemy GetEnemyPrefab(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.E1: return _enemy1Prefab;
            case EnemyType.E2: return _enemy2Prefab;
            case EnemyType.E3: return _enemy3Prefab;
            case EnemyType.E4: return _enemy4Prefab;
            case EnemyType.E5: return _enemy5Prefab;
            default:
                Debug.LogError("Unknown EnemyType: " + type);
                return null;
        }
    }
}
