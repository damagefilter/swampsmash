using System;
using ProtoBuf;

[ProtoContract]
public class Highscore {

	[ProtoMember(1)]
	private int score;
	public int Score {
		get {
			return score;
		}
	}

	[ProtoMember(2)]
	private string levelName;
	public string LevelName {
		get {
			return levelName;
		}
	}

	[ProtoMember(3)]
	private string playerName;
	public string PlayerName {
		get {
			return playerName;
		}
	}

	[ProtoMember(4)]
	private long timestamp;
	public long Timestamp {
		get {
			return timestamp;
		}
	}

	public string FormattedTimestamp {
		get {
			// International time format... kinda sorta
			return HighscoresList.ConvertToDate(Timestamp).ToString("yyyy.m.d HH:mm");
		}
	}

	public Highscore(int score, string level, string player, long timestamp) {
		this.score = score;
		this.levelName = level;
		this.playerName = player;
		this.timestamp = timestamp;
	}
	// protobuf ctor
	public Highscore() {}
}

