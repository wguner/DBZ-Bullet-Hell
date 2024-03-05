using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG
{
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class KeyboardControls
    {
        public string UP { get; set; }
        public string DOWN { get; set; }
        public string LEFT { get; set; }
        public string RIGHT { get; set; }
        public string FIRE { get; set; }
        public string SLOW { get; set; }
        public string INVINCIBLE { get; set; }
    }

    public class StartPosition
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Target
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class MovementSetting
    {
        public string style { get; set; }
        public int minCompletionTime { get; set; }
        public double speed { get; set; }
    }

    public class PlayerSettings
    {
        public StartPosition startPosition { get; set; }
        public string texture { get; set; }
        public int health { get; set; }
        public MovementSetting movement { get; set; }
        public double attackSpeed { get; set; }
        public string attackStyle { get; set; }
        public int lives { get; set; }
    }

    [Serializable]
    public class Wave
    {
        [JsonProperty("duration")]
        public int duration { get; set; }
        [JsonProperty("startPosition")]
        public StartPosition startPosition { get; set; }
        [JsonProperty("target")]
        public Target target { get; set; }
        [JsonProperty("enemyType")]
        public string enemyType { get; set; }
        [JsonProperty("texture")]
        public string texture { get; set; }
        [JsonProperty("enemyAmount")]
        public int enemyAmount { get; set; }
        [JsonProperty("interval")]
        public int interval { get; set; }
        [JsonProperty("hp")]
        public int hp { get; set; }
        [JsonProperty("atkSpeed")]
        public double atkSpeed { get; set; }
        [JsonProperty("attackStyle")]
        public string attackStyle { get; set; }
        [JsonProperty("movement")]
        public MovementSetting movement { get; set; }
    }

    [Serializable]
    public class GameSettings
    {
        [JsonProperty("keyboard-controls")]
        public KeyboardControls KeyboardControls { get; set; }
        [JsonProperty("player")]
        public PlayerSettings player { get; set; }
        [JsonProperty("waves")]
        public List<Wave> waves { get; set; }
    }

    [Serializable]
    public class Root
    {
        public GameSettings game { get; set; }
    }


}
