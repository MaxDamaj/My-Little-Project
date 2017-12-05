using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Skill : MonoBehaviour {

    public enum SkillType {
        Passive = 0,        //Passive skill
        Dash = 1,           //Increase pony speed and decrease recieved damage. Pony can jump
        AirDash = 2,        //Increase pony speed and decrease recieved damage. Perform only in midair
        Projectile = 3,     //Pony shoot various projectiles
        ChangeStat = 4,     //Changing pony stat value
        RestoreStat = 5,    //HP/MP restoration speed
        ItemsGiving = 6,    //Gave items
        Fly = 7,            //Wings or jetpack flying. Perform only in midair
        SlowMotion = 8,
        MPProtection = 9    //Pony lose MP not HP while hitting obstacles
    };

    public enum PassiveType {
        ItemMultiplier = 0, //Increase picking up items quantity
        ItemPickup = 1,     //Get item for collided object
        StatMultiplier = 2  //Set HP/MP multiplier (1 by default)
    };

    public enum StatType {
        Health = 0,
        Mana = 1,
        Stamina = 2,
        Speed = 3,
        Damage = 4,     //Taken damage. 1 by default
        Luck = 5
    };

    public enum Condition {
        Always = 0,
        HPLow = 1,      //HP < 50%
        HPHigh = 2,     //HP >= 50%
        MPLow = 3,      //MP < 50%
        MPHigh = 4      //MP >= 50%
    };

    public enum FXTarget {
        None = 0,       //FX not spawned
        Body = 1,       //FX attached to chacter root
        Point = 2       //FX attached to object "fx_point"
    };

    public bool IsCooldown = false;
    //-------------------------
    public string title;            //Skill title
    public Sprite icon;             //Skill icon
    public string description;      //Skill description

    public SkillType skillType;     //enum skill type
    public PassiveType passiveType; //enum passive skill type
    public StatType statType;       //enum stat type. Set multiplier to affected
    public Condition condition;     //enum special condition for passive skills
    //Common variables
    public float duration;          //Skill effect duration. Projectiles autofire delay
    public float MP_cost;           //Skill mana cost
    public float cooldown;          //Skill cooldown in second
    public float chance = 1;        //chance to succeed execute ability
    public float multiplier;        //custom multiplier
    public float SPDmlp = 1;        //SPD multiplier
    public float DMGmlp = 1;        //DMG multiplier
    public string item;             //item/obstacle what skill are affected
    public string[] items;          //items array what skill are affected
    public GameObject obj;          //skill attached object
    public string fx;               //skill spawned fx by pool manager
    public string sound;            //sound played by using this skill
    public FXTarget fxTarget;       //FX spawn position
    public Vector3 projPosition;    //Projectile spawn point

    public void ItemMultiplier(string i, float q) {
        if (Random.Range(0.0f, 1.0f) <= chance) {
            Database.Instance.IncreaseItemQuantity(i, q);
        }
    }
}
