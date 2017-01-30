using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class NumberGameAssets : IStoreAssets {

	public int GetVersion(){
		return 3;
	}

	public VirtualCurrency[] GetCurrencies(){
		return new VirtualCurrency[]{};
	}

	public VirtualGood[] GetGoods() {
		return new VirtualGood[]{PACKET1,PACKET2,PACKET3,NO_ADS};
	}

	public VirtualCurrencyPack[] GetCurrencyPacks(){
		return new VirtualCurrencyPack[]{};
	}

	public VirtualCategory[] GetCategories(){
		return new VirtualCategory[]{};
	}

	public static string PACKET1_PRODUCT_ID = "com.magiclampgames.numbergame2.packet1";
	public static string PACKET1_ITEM_ID = "packet1_item_id";
	
	public static VirtualGood PACKET1 = new SingleUseVG(
		"Online Packet 1",                                       		// name
		"400 Online Game Packet", 										// description
		PACKET1_ITEM_ID,                                       			// item id
		new PurchaseWithMarket(PACKET1_PRODUCT_ID,0.99)); 


	public static string PACKET2_PRODUCT_ID = "com.magiclampgames.numbergame2.packet2";
	public static string PACKET2_ITEM_ID = "packet2_item_id";
	
	public static VirtualGood PACKET2 = new SingleUseVG(
		"Online Packet 2",                                       		// name
		"1000 Online Game Packet", 										// description
		PACKET2_ITEM_ID,                                       			// item id
		new PurchaseWithMarket(PACKET2_PRODUCT_ID,1.99)); 


	public static string PACKET3_PRODUCT_ID = "com.magiclampgames.numbergame2.packet3";
	public static string PACKET3_ITEM_ID = "packet3_item_id";
	
	public static VirtualGood PACKET3 = new SingleUseVG(
		"Online Packet 3",                                       		// name
		"2000 Online Game Packet", 										// description
		PACKET3_ITEM_ID,                                       			// item id
		new PurchaseWithMarket(PACKET3_PRODUCT_ID,2.99)); 


	public static string NO_ADS_PRODUCT_ID = "com.magiclampgames.numbergame2.noads";
	public static string NO_ADS_ITEM_ID = "no_ads_item_id";

	public static VirtualGood NO_ADS = new LifetimeVG(
		"No Ads",
		"Remove Ads from the game",
		NO_ADS_ITEM_ID,
		new PurchaseWithMarket(NO_ADS_PRODUCT_ID,1.99)
	);



}
