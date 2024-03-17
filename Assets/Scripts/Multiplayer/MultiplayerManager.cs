using System.Collections;
using Colyseus;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private PlayerChracter _player;
    [SerializeField] private EnemyController _enemy;
    private ColyseusRoom<State> _room;
    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "speed", _player.speed }
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        _room.OnStateChange += OnChange;
        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo)
    {

        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("Врага нет, а он пытался стрелять!!!");
            return;
        }
        _enemies[shootInfo.key].Shoot(shootInfo);

    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;

        state.players.ForEach((String key, Player player) =>
            {
                if (key == _room.SessionId)
                { CreatePlayer(player); }
                else { CreateEnemy(key, player); }
            });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
        Instantiate(_player, position, Quaternion.identity);
    }

    private void CreateEnemy(String key, Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(player);
        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (_enemies.ContainsKey(key) == false) return;
        var enemy = _enemies[key];
        enemy.Destroy();
        _enemies.Remove(key);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionId()
    {
        return _room.SessionId;
    }
}
