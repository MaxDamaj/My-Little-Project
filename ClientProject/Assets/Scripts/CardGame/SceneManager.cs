using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SceneManager : MonoBehaviour
{
	public PopupManager PM;
	public DBChallenges DBC;

	[Header("Controllers")]
	public EnemyLineController enemy1;
	public EnemyLineController enemy2;
	public PlayerLineController player1;
	public PlayerLineController player2;

    [Header("UI")]
	public Button EndTurn;
	public Color DisbandColor;
	public Color ExtraColor;

    [Header("Decks")]
	public RectTransform[] tRowPile;
	public RectTransform[] tRowDeck;
	public RectTransform[] tRowSource;
	public RectTransform dPile;

    [Header("FX")]
    public RectTransform fx = null;
    public GameObject[] fx_tRow;
	public GameObject[] bl_tRow;

    [Header("Common")]
    public int PlayerBIT = 0;
	public int PlayerATK = 0;
	public int discardCount = 0;
	public int onTopOfDesk = 0;
	public OnTopType onTopType = OnTopType.None;

	private int IsEarthTribe = 0;
	private int IsHurricaneSquad = 0;
	private int IsMagicalOrder = 0;
    private int IsTechnocracy = 0;
    private int IsCrystalEmpire = 0;
	public PlayerLineController nowPlayer;
	public PlayerLineController notNowPlayer;
	public EnemyLineController nowEnemy;
	public EnemyLineController notNowEnemy;

	public GameObject U04;
	private CardsSpawn tradeRowStyle = CardsSpawn.DivideRow;
	private bool endlessOption = false;

	void Start() {
		ItemsController IC = GameObject.Find("UI_Items").GetComponent<ItemsController>();
		//Get variables
		if (IC != null) {
			CardChallenge challenge = DBC.GetCardChallenge(GlobalData.Instance.nowChallenge);
			player1.pImage.sprite = Database.Instance.GetCharCardIcon(Database.Instance.SelectedPony);
			enemy2.pImage.sprite = Database.Instance.GetCharCardIcon(Database.Instance.SelectedPony);
			player2.pImage.sprite = challenge.enemyIcon;
			enemy1.pImage.sprite = challenge.enemyIcon;

			player1.playerText.text = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony).CharName;
			enemy2.enemyText.text = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony).CharName;
			player2.playerText.text = challenge.enemyName;
			enemy1.enemyText.text = challenge.enemyName;

			player1.PlayerLUV = challenge.playerLUV;
			enemy2.LUV = challenge.playerLUV;
			player2.PlayerLUV = challenge.enemyLUV;
			enemy1.LUV = challenge.enemyLUV;
			//Fill Desk
			CardsReshuffle(IC.playerDeck, player1.pDeck);
			CardsReshuffle(IC.enemyDeck, player2.pDeck);
			//--------
			tradeRowStyle = challenge.tradeRowType;
			endlessOption = challenge.endlessOption;
			IC.itemWindow.anchoredPosition = new Vector2(0, 200);
		}
		nowPlayer = player1;
		nowEnemy = enemy1;
		notNowPlayer = player2;
		notNowEnemy = enemy2;
		Card.onRefresh += RefreshUI;
		EndTurn.onClick.AddListener(EndTurnClick);
		PutCardsOnPile(3, nowPlayer.pDeck, nowPlayer.pHand, nowPlayer.pPile, true);
		//Take cards in TradeRow
		if (tradeRowStyle == CardsSpawn.DivideRow) {
			CardsReshuffle(tRowSource[0], tRowDeck[0]);
			CardsReshuffle(tRowSource[1], tRowDeck[1]);
		}
		if (tradeRowStyle == CardsSpawn.TypesRow) {
			CardsReshuffle(tRowSource[0], tRowSource[1]);
			CardsReshuffle(tRowSource[1], tRowSource[0]);
			CardsReshuffle(tRowSource[0], tRowDeck[0], tRowDeck[1], tradeRowStyle);
		}
		if (tradeRowStyle == CardsSpawn.StandardRow) {
			CardsReshuffle(tRowSource[0], tRowSource[1]);
			CardsReshuffle(tRowSource[1], tRowDeck[0]);
		}
		RefreshUI();
	}
	
	// Refreshing window
	public void RefreshUI()	{
		PM.Refresh();
		//Recalculate card states
		RecalculateTribeState(nowPlayer);
		RecalculateCardsActions(nowPlayer.pTurn);
		RecalculateCardsActions(nowPlayer.pWarehouses);

		for (int i = 0; i < fx_tRow.GetLength(0); i++) {
			fx_tRow[i].SetActive(false);
			bl_tRow[i].SetActive(true);
		}	
		nowPlayer.pDeckInfo.text = "" + nowPlayer.pDeck.childCount;

		nowPlayer.textBIT.text = "" + PlayerBIT;
		nowPlayer.textATK.text = "" + PlayerATK;

		if (PlayerATK > 0 && nowEnemy.ReturnCastlesCount() == 0) {
			nowEnemy.luvFrame.SetActive(true);
		} else {
			nowEnemy.luvFrame.SetActive(false);
		}
		nowEnemy.Refresh();
		nowPlayer.Refresh();

		//Prospector check
		if (PlayerBIT >= U04.GetComponent<Card>().price) {
			fx_tRow[0].SetActive(true);
			bl_tRow[0].SetActive(false);
		}
		//1-4 TradeRow check
		for (int i = 1; i < 4; i++) {
			if (tRowPile[i].childCount != 0) {
				if (PlayerBIT >= tRowPile[i].GetChild(0).GetComponent<Card>().price) {
					fx_tRow[i].SetActive(true);
					bl_tRow[i].SetActive(false);
				}
			} else {
				AddCardInTradeRow(Decks.tLow, tRowPile[i]);
			}
		}
		//5-9 TradeRow check
		for (int i = 4; i < 6; i++) {
			if (tRowPile[i].childCount != 0) {
				if (PlayerBIT >= tRowPile[i].GetChild(0).GetComponent<Card>().price) {
					fx_tRow[i].SetActive(true);
					bl_tRow[i].SetActive(false);
				}
			} else {
				if (tradeRowStyle != CardsSpawn.StandardRow) {
					AddCardInTradeRow(Decks.tMiddle, tRowPile[i]);
				} else {
					AddCardInTradeRow(Decks.tLow, tRowPile[i]);
				}
			}
		}
	}

	void OnDestroy() {
		Card.onRefresh -= RefreshUI;
	}


	//----------------------------------------------------------
	public void SetFX(DeckDrop deck, Card card) {
		switch (deck.targetDeck) {
		case Decks.tProsp:
		case Decks.tLow:
		case Decks.tMiddle:
			fx.rotation = Quaternion.identity;
			fx.gameObject.SetActive(true);
			if (onTopOfDesk > 0 && card.cardType == CardType.Hero) {
				fx.anchoredPosition = nowPlayer.pDeck.anchoredPosition;
				fx.sizeDelta = nowPlayer.pDeck.sizeDelta;
			} else {
				fx.anchoredPosition = nowPlayer.pPile.anchoredPosition;
				fx.sizeDelta = nowPlayer.pPile.sizeDelta;

			}
			break;
		case Decks.pHand:
			fx.gameObject.SetActive(true);
			fx.rotation = Quaternion.identity;
			if (card.cardType == CardType.Hero) {
				fx.anchoredPosition = nowPlayer.pTurn.parent.parent.GetComponent<RectTransform>().anchoredPosition;
				fx.sizeDelta = nowPlayer.pTurn.parent.parent.GetComponent<RectTransform>().sizeDelta;
			} else {
				fx.anchoredPosition = nowPlayer.pWarehouses.parent.parent.GetComponent<RectTransform>().anchoredPosition;
				fx.sizeDelta = nowPlayer.pWarehouses.parent.parent.GetComponent<RectTransform>().sizeDelta;
				fx.Rotate(Vector3.forward, -90);
			}
			break;
		}
	}

	public void PutCardsOnPile(int num, Transform begin, Transform target, Transform extraPile, bool isActivate) {
		if (begin.childCount >= num) {
			for (int i = 0; i < num; i++) {
				begin.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
				begin.GetChild(0).GetComponent<Draggable>().enabled = isActivate;
				begin.GetChild(0).SetParent(target);
			}
		} else {
			int numExtra = num - begin.childCount; //Additional cards number
			int numBasic = begin.childCount; //Basic card number
			for (int i = 0; i < numBasic; i++) {
				begin.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
				begin.GetChild(0).GetComponent<Draggable>().enabled = isActivate;
				begin.GetChild(0).GetComponent<Card>().SetActionExecution(ActCondition.Normal, !isActivate);
				begin.GetChild(0).GetComponent<Card>().SetActionExecution(ActCondition.Tribe, !isActivate);
				begin.GetChild(0).GetComponent<Card>().SetActionExecution(ActCondition.Disband, !isActivate);
				begin.GetChild(0).SetParent(target);
			}
			CardsReshuffle(extraPile, begin); //Make Reshuffle
			for (int i = 0; i < numExtra; i++) {
				begin.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
				begin.GetChild(0).GetComponent<Draggable>().enabled = isActivate;
				begin.GetChild(0).GetComponent<CardInPile>().enabled = false;
				begin.GetChild(0).SetParent(target);
			}
		}
	}

	public void EndTurnClick() {
		if (nowPlayer.pHand.childCount > 0) {	return; }
		//Clear Table
		SetActionForCardsInDeck(nowPlayer.pTurn, false);
		PutCardsOnPile(nowPlayer.pTurn.transform.childCount, nowPlayer.pTurn, nowPlayer.pPile, nowPlayer.pHand, true);
		PlayerBIT = 0;
		PlayerATK = 0;
		IsEarthTribe = 0; IsMagicalOrder = 0;
		IsHurricaneSquad = 0; IsTechnocracy = 0;
		IsCrystalEmpire = 0;
		onTopOfDesk = 0;
		nowEnemy.uifxHit.SetActive(false);
		//Put Warehouses on enemy line
		PutCardsOnPile(nowPlayer.pWarehouses.transform.childCount, nowPlayer.pWarehouses, notNowEnemy.eWarehouses, notNowEnemy.eWarehouses, false);
		PutCardsOnPile(nowEnemy.eWarehouses.transform.childCount, nowEnemy.eWarehouses, notNowPlayer.pWarehouses, notNowPlayer.pWarehouses, true);
		//Change player
		if (nowPlayer == player1) {
			player2.PlayerLUV = enemy1.LUV; //LUV
			enemy2.LUV = player1.PlayerLUV; //LUV
			player1.gameObject.SetActive(false);
			player2.gameObject.SetActive(true);
			enemy1.gameObject.SetActive(false);
			enemy2.gameObject.SetActive(true);
			nowPlayer = player2;
			notNowPlayer = player1;
			nowEnemy = enemy2;
			notNowEnemy = enemy1;
		} else {
			player1.PlayerLUV = enemy2.LUV; //LUV
			enemy1.LUV = player2.PlayerLUV; //LUV
			player2.gameObject.SetActive(false);
			player1.gameObject.SetActive(true);
			enemy2.gameObject.SetActive(false);
			enemy1.gameObject.SetActive(true);
			nowPlayer = player1;
			notNowPlayer = player2;
			nowEnemy = enemy1;
			notNowEnemy = enemy2;
		}
		SetActionForCardsInDeck(nowPlayer.pWarehouses, false);
		PutCardsOnPile(5, nowPlayer.pDeck, nowPlayer.pHand, nowPlayer.pPile, true);
		for (int i = 0; i < nowPlayer.pHand.childCount; i++) {
			Card card = nowPlayer.pHand.GetChild(i).GetComponent<Card>();
			card.SetActionExecution(ActCondition.Normal, false);
			card.SetActionExecution(ActCondition.Tribe, false);
			card.SetActionExecution(ActCondition.Disband, false);
		}
		//Show Discard popup if needed
		if (discardCount > 0) {
			if (discardCount > 5) discardCount = 5;
			PM.popupDiscardWindow.ShowDiscardPopup();
		}

		RefreshUI();
	}

	void CardsReshuffle(Transform begin, Transform target) {
		while (begin.childCount > 0) {
			Transform tmp = begin.GetChild(Mathf.RoundToInt(UnityEngine.Random.Range(0, begin.childCount - 1)));
			tmp.SetParent(target);
			tmp.localScale = Vector3.one;
		}
	}
	void CardsReshuffle(Transform begin, Transform target1, Transform target2, CardsSpawn cardSpawn) {
		//Typed
		if (cardSpawn == CardsSpawn.TypesRow) {
			while (begin.childCount > 0) {
				int rnd = Mathf.RoundToInt(UnityEngine.Random.Range(0, begin.childCount - 1));
				if (begin.GetChild(rnd).GetComponent<Card>().cardType == CardType.Hero) {
					begin.GetChild(rnd).SetParent(target1);
				} else {
					begin.GetChild(rnd).SetParent(target2);
				}
			}
		}
	}

	public Draggable ReturnUnusedExtraWarehouse() {
		for (int i = 0; i < nowPlayer.pWarehouses.childCount; i++) {
			if (nowPlayer.pWarehouses.GetChild(i).GetComponent<Image>().color == ExtraColor) {
				return nowPlayer.pWarehouses.GetChild(i).GetComponent<Draggable>();
			}
		}
		return null;
	}
	public Draggable ReturnUnusedExtraHero() {
		for (int i = 0; i < nowPlayer.pTurn.childCount; i++) {
			if (nowPlayer.pTurn.GetChild(i).GetComponent<Image>().color == ExtraColor) {
				return nowPlayer.pTurn.GetChild(i).GetComponent<Draggable>();
			}
		}
		return null;
	}

	void AddCardInTradeRow(Decks deck, Transform target) {
		if (endlessOption) {
			if (deck == Decks.tLow) {
				Transform obj = Instantiate(tRowDeck[0].GetChild(UnityEngine.Random.Range(0, tRowDeck[0].childCount - 1)));
				obj.SetParent(target);
				obj.localScale = new Vector3(1, 1, 1);
			}
			if (deck == Decks.tMiddle) {
				Transform obj = Instantiate(tRowDeck[1].GetChild(UnityEngine.Random.Range(0, tRowDeck[1].childCount - 1)));
				obj.SetParent(target);
				obj.localScale = new Vector3(1, 1, 1);
			}
		} else {
			if (deck == Decks.tLow) {
				if (tRowDeck[0].childCount == 0) return;
				PutCardsOnPile(1, tRowDeck[0], target, tRowDeck[1], true);
			}
			if (deck == Decks.tMiddle) {
				if (tRowDeck[1].childCount == 0) return;
				PutCardsOnPile(1, tRowDeck[1], target, tRowDeck[0], true);
			}
		}

	}

	void SetActionForCardsInDeck(Transform deck, bool value) {
		for (int i = 0; i < deck.childCount; i++) {
			Card card = deck.GetChild(i).GetComponent<Card>();
			card.SetActionExecution(ActCondition.Normal, value);
			card.SetActionExecution(ActCondition.Tribe, value);
			card.SetActionExecution(ActCondition.Disband, value);
		}
	}

	public bool CheckTribe(Tribe tribe) {
		if (IsEarthTribe > 1 && tribe == Tribe.EarthTribe) { return true; }
		if (IsHurricaneSquad > 1 && tribe == Tribe.HurricaneSquad) { return true; }
		if (IsMagicalOrder > 1 && tribe == Tribe.MagicalOrder) { return true; }
		if (IsTechnocracy > 1 && tribe == Tribe.Technocracy) { return true; }
		if (IsCrystalEmpire > 1 && tribe == Tribe.CrystalEmpire) { return true; }
		return false;
	}

	void SetTribe(Tribe tribe) {
		switch (tribe) {
		case Tribe.EarthTribe:
			IsEarthTribe++;
			break;
		case Tribe.HurricaneSquad:
			IsHurricaneSquad++;
			break;
		case Tribe.MagicalOrder:
			IsMagicalOrder++;
			break;
		case Tribe.Technocracy:
			IsTechnocracy++;
			break;
		case Tribe.CrystalEmpire:
			IsCrystalEmpire++;
			break;
		}
	}

	public void RecalculateTribeState(PlayerLineController player) {
		IsEarthTribe = 0; IsMagicalOrder = 0;
		IsHurricaneSquad = 0; IsTechnocracy = 0;
		IsCrystalEmpire = 0;
		//--------
		for (int i = 0; i < player.pTurn.childCount; i++) {
			Card card = player.pTurn.GetChild(i).GetComponent<Card>();
			SetTribe(card.tribe);
		}
		for (int i = 0; i < player.pWarehouses.childCount; i++) {
			Card card = player.pWarehouses.GetChild(i).GetComponent<Card>();
			SetTribe(card.tribe);
		}
	}

	public void RecalculateCardsActions(Transform deck)	{
		if (deck.childCount == 0) { return; }
		//Execute Actions
		for (int i = 0; i < deck.childCount; i++) {
			Card card = deck.GetChild(i).GetComponent<Card>();
			card.GetComponent<Image>().color = new Color(1, 1, 1, 1);
			//Disband
			if (!card.GetActionExecution(ActCondition.Disband)) {
				card.GetComponent<Image>().color = DisbandColor;
			}
			//Normal
			if (!card.GetActionExecution(ActCondition.Normal)) {
                if (card.GetActionVariation(ActCondition.Normal) == Variation.AND) {
                    PlayerBIT += card.ReturnBIT(ActCondition.Normal);
                    PlayerATK += card.ReturnATK(ActCondition.Normal);
                    nowPlayer.PlayerLUV += card.ReturnLUV(ActCondition.Normal);
                    //Actions
                    if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.DrawACard) {
                        PutCardsOnPile(card.ReturnModifier(ActCondition.Normal), nowPlayer.pDeck, nowPlayer.pHand, nowPlayer.pPile, true);
                    }
                    if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.Opp_DiscardCard) {
                        discardCount += card.ReturnModifier(ActCondition.Normal);
                    }
                    if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.DrawForTwoWarehouses) {
                        if (nowPlayer.pWarehouses.childCount >= 2) {
                            PutCardsOnPile(card.ReturnModifier(ActCondition.Normal), nowPlayer.pDeck, nowPlayer.pHand, nowPlayer.pPile, true);
                        }
                    }
					if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.HeroOnTopOfDeck) {
						onTopOfDesk += card.ReturnModifier(ActCondition.Normal);
						onTopType = OnTopType.Hero;
					}
                    card.SetActionExecution(ActCondition.Normal, true);
                } else {
                    card.GetComponent<Image>().color = ExtraColor;
                }
				
			}
			//Tribe
			if (CheckTribe(card.tribe)) {
				if (!card.GetActionExecution(ActCondition.Tribe)) {
					if (card.GetActionVariation(ActCondition.Tribe) == Variation.AND) {
						PlayerBIT += card.ReturnBIT(ActCondition.Tribe);
						PlayerATK += card.ReturnATK(ActCondition.Tribe);
						nowPlayer.PlayerLUV += card.ReturnLUV(ActCondition.Tribe);
						card.SetActionExecution(ActCondition.Tribe, true);
						//Actions
						if (card.ReturnInAddition(ActCondition.Tribe) == SpecAction.DrawACard) {
							PutCardsOnPile(card.ReturnModifier(ActCondition.Tribe), nowPlayer.pDeck, nowPlayer.pHand, nowPlayer.pPile, true);
						}
						if (card.ReturnInAddition(ActCondition.Tribe) == SpecAction.Opp_DiscardCard) {
							discardCount += card.ReturnModifier(ActCondition.Tribe);
						}
						if (card.ReturnInAddition(ActCondition.Tribe) == SpecAction.HeroOnTopOfDeck) {
							onTopOfDesk += card.ReturnModifier(ActCondition.Tribe);
							onTopType = OnTopType.Hero;
						}
					}
					if (card.GetActionVariation(ActCondition.Tribe) != Variation.AND) {
						card.GetComponent<Image>().color = ExtraColor;
					}
				}
			}

		}
	}

}
