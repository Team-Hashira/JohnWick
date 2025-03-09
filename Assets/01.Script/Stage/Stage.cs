using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage
{
    [Serializable]
    public class Wave
    {
        public float delay = 1;
        public Enemy[] enemies;
        public UnityEvent ClearEvent;

        private int _enemyCount = 0;
        private Stage _owner;

        public void Init(Stage stage)
        {
            _enemyCount = enemies.Length;
            _owner = stage;

            foreach (Enemy enemy in enemies)
            {
                enemy.GetEntityComponent<EntityHealth>().OnDieEvent += HandleEnemyCounting;
            }
        }

        private void HandleEnemyCounting(Entity entity)
        {
            --_enemyCount;
            _owner.enemyList.Remove(entity as Enemy);
            if (_enemyCount > 0) return;

            ClearEvent?.Invoke();
            _owner.AddWaveCount();
        }

        public void SetActiveAllEnemies(bool active)
        {
            for (int i = 0; i < enemies.Length; i++)
                enemies[i].gameObject.SetActive(active);
        }
    }

    public class Stage : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        public Wave[] waves;

        protected StageGenerator _stageGenerator;
        public int CurrentWaveCount { get; private set; } = 0;
        public UnityEvent OnAllClearEvent;
        
        [HideInInspector] public List<Enemy> enemyList;

        public Wave GetCurrentWave() => waves[CurrentWaveCount];

        public Enemy[] GetEnabledRandomEnemies(int count)
        {
            List<Enemy> curEnemyList = GetEnabledEnemies().ToList();

            int finalCount = Mathf.Clamp(count, 0, curEnemyList.Count);
            Enemy[] finalEnemies = new Enemy[finalCount];

            for (int i = 0; i < finalCount; i++)
            {
                int random = UnityEngine.Random.Range(0, curEnemyList.Count);

                finalEnemies[i] = curEnemyList[random];
                curEnemyList.RemoveAt(random);
            }

            return finalEnemies;
        }

        public Enemy[] GetEnabledEnemies()
        {
            return enemyList.Where(x => x.gameObject.activeSelf.Equals(true)).ToArray();
        }

        private void Start()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].Init(this);
                enemyList.AddRange(waves[i].enemies);
            }

            for (int i = 0; i < waves.Length; i++)
                waves[i].SetActiveAllEnemies(false);

            SetPlayerPosToSpawnPoint();

            AddWaveCount(0);
        }

        public void AddWaveCount(int value = 1)
        {
            StartCoroutine(CoroutineAddWaveCount(value));
        }

        private IEnumerator CoroutineAddWaveCount(int value)
        {
            CurrentWaveCount += value;
            if (CurrentWaveCount >= waves.Length)
            {
                OnAllClearEvent?.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(waves[CurrentWaveCount].delay);
                waves[CurrentWaveCount].SetActiveAllEnemies(true);
            }
        }

        public void SetPlayerPosToSpawnPoint()
        {
            PlayerManager.Instance.Player.transform.position = _playerSpawnPoint.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                for (int j = 0; j < waves[i].enemies.Length; j++)
                {
                    if (waves[i].enemies[j] == null) return;
                    var pos = waves[i].enemies[j].gameObject.transform.position + new Vector3(0, 1);
                    Handles.Label(pos, $"wave {i}");
                }
            }

        }
#endif
    }
}