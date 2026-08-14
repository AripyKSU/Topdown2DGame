using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    private const int MaxRankCount = 5;
    private const string RankKeyPrefix = "Ranking_";

    public static RankingManager I { get; private set; }

    private readonly List<int> rankings = new List<int>(MaxRankCount);

    public IReadOnlyList<int> Rankings => rankings;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(this); return; }

        I = this;
        LoadRankings();
    }

    private void OnDestroy()
    {
        if (I == this)
        {
            I = null;
        }
    }

    private void OnEnable()
    {
        if (GameFlow.I != null)
        {
            GameFlow.I.GameEnded += RecordCurrentRun;
        }
    }

    private void OnDisable()
    {
        if (GameFlow.I != null)
        {
            GameFlow.I.GameEnded -= RecordCurrentRun;
        }
    }

    public void RecordCurrentRun()
    {
        if (GameScorer.I == null) return;

        AddScore(GameScorer.I.Score);
    }

    public void AddScore(int score)
    {
        if (score <= 0f) return;

        rankings.Add(score);
        rankings.Sort((a, b) => b.CompareTo(a));

        while (rankings.Count > MaxRankCount)
        {
            rankings.RemoveAt(rankings.Count - 1);
        }

        SaveRankings();
    }

    public string GetRankingText()
    {
        LoadRankings();

        List<string> lines = new List<string>(MaxRankCount);
        for (int i = 0; i < MaxRankCount; i++)
        {
            lines.Add(GetRankLine(i));
        }

        return string.Join("\n", lines);
    }

    public string GetRankLine(int rankIndex)
    {
        LoadRankings();

        string scoreText = rankIndex < rankings.Count ? GameScorer.FormatScore(rankings[rankIndex]) : "0";
        return string.Format("Rank #{0} : {1}", rankIndex + 1, scoreText);
    }

    private void LoadRankings()
    {
        rankings.Clear();

        for (int i = 0; i < MaxRankCount; i++)
        {
            int score = PlayerPrefs.GetInt(RankKeyPrefix + i, 0);
            if (score > 0f)
            {
                rankings.Add(score);
            }
        }

        rankings.Sort((a, b) => b.CompareTo(a));
    }

    private void SaveRankings()
    {
        for (int i = 0; i < MaxRankCount; i++)
        {
            int score = i < rankings.Count ? rankings[i] : 0;
            PlayerPrefs.SetInt(RankKeyPrefix + i, score);
        }

        PlayerPrefs.Save();
    }
}
