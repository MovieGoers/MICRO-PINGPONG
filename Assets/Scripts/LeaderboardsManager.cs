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
    public class Score // 리더보드 element
    {
        public string playerId;
        public string playerName;
        public int rank;
        public double score;
    };

    public List<Score> scores = new List<Score>(); // 상위 플레이어 Score 리스트
    public Score playerScore = new Score(); // 현재 플레이어 Score.
    
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

        await SignInAnonymously(); // 익명으로 서비스에 플레이어 등록
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


    // 플레이어 최고 점수 등록 -> 리더보드 및 플레이어 점수 가져오기.
    public async void HandleLeaderboard(int highscore)
    {
        var addScoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, highscore); // 플레이어 최고 점수 입력

        var leaderboardResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId); // 리더보드 가져오기.

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

        UIManager.Instance.AddLeaderboardScoreText(); // 플레이어의 리더보드 정보 가져오기.

        var playerScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);

        playerScore.playerId = playerScoreResponse.PlayerId;
        playerScore.playerName = playerScoreResponse.PlayerName;
        playerScore.rank = playerScoreResponse.Rank;
        playerScore.score = playerScoreResponse.Score;

        UIManager.Instance.SetPlayerLeaderboardText();
    }
}