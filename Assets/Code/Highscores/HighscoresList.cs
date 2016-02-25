using System;
using ProtoBuf;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighscoresList {
	private List<Highscore> data = new List<Highscore>();
	// TODO: Can reside in a tool class or something
	public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	public static long ConvertToTimestamp(DateTime value) {
		TimeSpan elapsedTime = value - Epoch;
		return (long) elapsedTime.TotalSeconds;
	}

	public static DateTime ConvertToDate(long value) {
		return Epoch.AddSeconds(value);
	}

	public void AddNewScore(int score, string level, string player) {
		if (score < 0) {
			// No negative scores, if any. There probably won't be any though
			return;
		}
		if (data.Count >= 10) {
			// First sort
			data.Sort((a, b) => {
				return b.Score.CompareTo(a.Score);
			});
			// Remove the last one
			// If not we're not good enough for the last score, no entry
			if (data[data.Count - 1].Score < score) {
				data.RemoveAt(data.Count - 1);
				data.Add(new Highscore(score, level, player, ConvertToTimestamp(DateTime.UtcNow)));
			}

		}
		else {
			data.Add(new Highscore(score, level, player, ConvertToTimestamp(DateTime.UtcNow)));
		}
	}

	public List<Highscore> GetTopTen() {
		if (data.Count == 0) {
			return new List<Highscore>();
		}
		// Sort this list ...
		data.Sort((a, b) => {
			return b.Score.CompareTo(a.Score);
		});
		return data.GetRange(0, Mathf.Min (10, data.Count));
	}

	public List<Highscore> GetTopThree() {
		if (data.Count == 0) {
			return new List<Highscore>();
		}
		// Sort this list ...
		data.Sort((a, b) => {
			return b.Score.CompareTo(a.Score);
		});
		return data.GetRange(0, Mathf.Min (3, data.Count));
	}

	public void Load() {
		if (File.Exists(Application.dataPath + "/highscores.dat")) {
			using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(Application.dataPath + "/highscores.dat"))) {
				ms.Position = 0;
				this.data = ProtoBuf.Serializer.Deserialize<List<Highscore>>(ms);
			}
		}
	}

	public void Save() {
		using (var file = File.Open(Application.dataPath + "/highscores.dat", FileMode.OpenOrCreate)) {
			using (MemoryStream ms = new MemoryStream()) {
				ProtoBuf.Serializer.Serialize<List<Highscore>>(ms, this.data);
				ms.Position = 0;
				var arr = ms.ToArray();
				file.Write(arr, 0, (int)ms.Length);
			}
		}
	}
}

