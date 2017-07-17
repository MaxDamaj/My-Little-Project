using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Action {
	public ActCondition condition;
	public SpecAction inAddition;
	public Variation varity;
	public int BIT;
	public int ATK;
	public int LUV;
	public int GEM;
	public int Modifier;
    public bool IsExecuted = false;
}

public class Card : MonoBehaviour {
	public string Name;
	public DLC pack;
	public int price;
    public CardType cardType;
	public Tribe tribe;
	public int Protection;

	[SerializeField]
	List<Action> actions = null;

	public delegate void RefreshArgs();
	public static event RefreshArgs onRefresh;

	public int ReturnBIT(ActCondition cond) {
		int bit = 0;
		foreach (Action act in actions) {
            if (act.condition == cond) {
                bit += act.BIT;
            }
		}
		return bit;
	}
	public int ReturnATK(ActCondition cond) {
		int atk = 0;
		foreach (Action act in actions) {
            if (act.condition == cond) {
                atk += act.ATK;
            }
		}
		return atk;
	}
	public int ReturnLUV(ActCondition cond) {
		int luv = 0;
		foreach (Action act in actions) {
            if (act.condition == cond) {
                luv += act.LUV;
            }
		}
		return luv;
	}
	public int ReturnModifier(ActCondition cond) {
		foreach (Action act in actions) {
			if (act.condition == cond) {
				return act.Modifier;
			}
		}
		return 0;
	}

    public SpecAction ReturnInAddition(ActCondition cond) {
        foreach (Action act in actions) {
            if (act.condition == cond) {
                return act.inAddition;
            }
        }
        return SpecAction.None;
    }

	public Variation GetActionVariation(ActCondition cond) {
		foreach (Action act in actions) {
			if (act.condition == cond) {
				return act.varity;
			}
		}
		return Variation.AND;
	}

    public void SetActionExecution(ActCondition cond, bool exec) {
        foreach (Action act in actions) {
            if (act.condition == cond) {
                act.IsExecuted = exec;
            }
        }
    }

    public bool GetActionExecution(ActCondition cond) {
        foreach (Action act in actions) {
            if (act.condition == cond) {
                return act.IsExecuted;
            }
        }
        return true;
    }

    public void Refresh() {
		if (onRefresh != null) {onRefresh();}
	}

}
