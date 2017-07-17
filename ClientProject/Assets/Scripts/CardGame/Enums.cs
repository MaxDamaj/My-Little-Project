using UnityEngine;
using System.Collections;

public enum Tribe {
	Unaligned, HurricaneSquad, MagicalOrder, EarthTribe, Technocracy, CrystalEmpire
};
public enum ActCondition {
	Normal, Tribe, Disband
};
public enum SpecAction {
	None, DrawACard, Opp_DiscardCard, Disband_Hand, Disband_HandAndDiscard, DisbandRow, DrawForTwoWarehouses, HeroOnTopOfDeck
};
public enum Variation {
	AND, OR, ACT
};
public enum CardType {
	Hero, Warehouse, Castle, Village, Artifact, Event
};
public enum OnTopType {
	None, Hero, Warehouses, All
};

public enum DLC {
	Core, FirstExpand, Mane6, Royalty, Artifacts, Spells, Creatures, Warehouses, Transportation, FiM
};


public enum Decks {
	Nothing, tProsp, tLow, tMiddle, tHigh, pHand, pTurn, pPile, pDeck, tDisband, mDeck, pWarehouses
};
public enum PopupStatement {
	None, BIT, ATK, LUV, GEM, Action
};
public enum CardsSpawn {
	None, DivideRow, StandardRow, TypesRow
};
public enum PopupState {
	Normal, AND, OR, ShowCard, DisbandPile, DisbandRow, Discard
};
