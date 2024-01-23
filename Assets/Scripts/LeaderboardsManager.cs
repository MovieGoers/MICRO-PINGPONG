using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public class LeaderboardsManager : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "micro-pong-leaderboard";

    private static LeaderboardsManager instance;
    public class Score // �������� element
    {
        public string playerId;
        public string playerName;
        public int rank;
        public double score;
    };

    public List<Score> scores = new List<Score>(); // ���� �÷��̾� Score ����Ʈ
    public Score playerScore = new Score(); // ���� �÷��̾� Score.
    
    public static LeaderboardsManager Instance
    {
        get
        {
            return instance;
        }
    }

    async void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        await UnityServices.InitializeAsync();

        await SignInAnonymously(); // �͸����� ���񽺿� �÷��̾� ���
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    // �÷��̾� �ְ� ���� ��� -> �������� �� �÷��̾� ���� ��������.
    public async void HandleLeaderboard(int highscore)
    {
        var addScoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, highscore); // �÷��̾� �ְ� ���� �Է�

        var leaderboardResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId); // �������� ��������.

        scores.Clear();

        foreach(var response in leaderboardResponse.Results)
        {
            Score newScore = new Score();

            newScore.playerId = response.PlayerId;
            newScore.playerName = response.PlayerName;
            newScore.rank = response.Rank;
            newScore.score = response.Score;

            scores.Add(newScore);
        }

        UIManager.Instance.AddLeaderboardScoreText(); // �÷��̾��� �������� ���� ��������.

        var playerScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);

        playerScore.playerId = playerScoreResponse.PlayerId;
        playerScore.playerName = playerScoreResponse.PlayerName;
        playerScore.rank = playerScoreResponse.Rank;
        playerScore.score = playerScoreResponse.Score;

        UIManager.Instance.SetPlayerLeaderboardText();
    }
}