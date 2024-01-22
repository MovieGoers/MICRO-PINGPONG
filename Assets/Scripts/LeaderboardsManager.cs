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

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    private static LeaderboardsManager instance;
    public class Score
    {
        public string playerId;
        public string playerName;
        public int rank;
        public double score;
    };

    List<Score> scores = new List<Score>(); // ���� �÷��̾� Score ����Ʈ
    Score playerScore = new Score(); // ���� �÷��̾� Score.
    
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

        await SignInAnonymously();
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

    public async void AddScore(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);

        scores.Clear();

        for(int i=0;i< scoresResponse.Total; i++)
        {
            Score newScore = new Score();

            newScore.playerId = scoresResponse.Results[i].PlayerId;
            newScore.playerName = scoresResponse.Results[i].PlayerName;
            newScore.rank = scoresResponse.Results[i].Rank;
            newScore.score = scoresResponse.Results[i].Score;

            scores.Add(newScore);
        }
    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);

        playerScore.playerId = scoreResponse.PlayerId;
        playerScore.playerName = scoreResponse.PlayerName;
        playerScore.rank = scoreResponse.Rank;
        playerScore.score = scoreResponse.Score;
    }

    public async void GetPlayerRange()
    {
        // Returns a total of 11 entries (the given player plus 5 on either side)
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
            LeaderboardId,
            new GetPlayerRangeOptions { RangeLimit = rangeLimit }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}