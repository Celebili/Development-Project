using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "宝可梦道具/创建新恢复道具")]
public class RecoveryItem : ItemBase
{
    [Header("回血")]
    [Tooltip("HP或PP恢复值")]
    [SerializeField] int restoreValue;
    [Tooltip("HP PP 恢复满")]
    [SerializeField] bool restoreMax;
    [Tooltip("全恢复 或 四技能PP恢复")]
    [SerializeField] bool restoreAll;

    public override bool Use(Pokemon pokemon)
    {
        if(pokemon.isHPFull || pokemon.isFainted)
        {
            return false;
        }
        return true;
    }

    public override string UseForPokemon(Pokemon pokemon)
    {
        if(restoreAll)//恢复血量及状态
        {
            AudioManager.Instance.HealPokemon();
            pokemon.AllCureItem();
            return pokemon.NickName + "'s HP and condition recovered!";
        }
        else if(restoreMax)//恢复满血
        {
            pokemon.UpdateHP(pokemon.MaxHP, UpdateHpVoiceType.Cure);
            return pokemon.NickName + "'s HP recover!";
        }
        else//恢复值
        {
            int lack = pokemon.HP;
            pokemon.UpdateHP(restoreValue, UpdateHpVoiceType.Cure);
            return $"{pokemon.NickName} recover {(pokemon.HP - lack).ToString()} HP!";
        }
    }

    public string AddPP(Pokemon pokemon, int n)
    {
        List<Skill> skills = pokemon.Skill;
        if(restoreMax)
        {
            if(restoreAll)
            {
                foreach(Skill skill in skills)
                {
                    skill.ResetPP();
                }
                return pokemon.NickName + "recover all PP!";
            }
            else
            {
                pokemon.Skill[n].ResetPP();
                return $"{pokemon.NickName}'s {pokemon.Skill[n].Base.SkillName} PP recover!";
            }
        }
        else
        {
            if(restoreAll)
            {
                foreach(Skill skill in skills)
                {
                    skill.PPValueChange(restoreValue);
                }
                return $"{pokemon.NickName}'s all skill PP recover {restoreValue.ToString()}!";
            }
            else
            {
                pokemon.Skill[n].PPValueChange(restoreValue);
                return $"{pokemon.NickName}'s {pokemon.Skill[n].Base.SkillName} PP recover {restoreValue.ToString()}!";
            }
        }
    }
}