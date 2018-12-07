using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using RightEdge.Common;
using RightEdge.Indicators;

public class BreakOutModel
{
	
	public int bars_to_use { get; set; }
	public int bars_to_sell { get; set; }

	public double bo_buy_height_param { get; set; }
	public double bo_sell_height_param { get; set; }
	
	public double adx_param { get; set; }
	
	
	public BreakOutModel(double buy_bars, double sell_bars, double buy_height, double sell_height, double adx_value)
	{
        bars_to_use = (int)buy_bars;
		bars_to_sell = (int)sell_bars;

		bo_buy_height_param = buy_height;
		bo_sell_height_param = sell_height;
		
		adx_param = adx_value;
		
    }
	
		
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcBuy (BarData[] LookBackData , double body, double AdxValue) {
		
		// Default to Buying
		bool result = true;

				
		// Check Level of ADX
		if (AdxValue > adx_param) {
			result = false;
		}
				
		// Check Height of Bar
		if (body < bo_buy_height_param) {
			result = false;
		}

		// Must be an up day
		if (LookBackData[0].Close < LookBackData[0].Open) {
            result = false;
		}
		
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if (  LookBackData[0].Close < bd.Close ) {
               result = false;
			}
		}
						
        return result;
	}
	
	
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcSell (BarData[] LookBackData , double body ) {
		
		// Default to Selling
		bool result = true;
		
			
		// Check Height of Bar
		if (body > bo_sell_height_param) {
			result = false;
		}
				
		if (LookBackData[0].Close > LookBackData[0].Open) {
            result = false;
		}
		
		
		foreach (BarData bd in LookBackData)
        {
			if (  LookBackData[0].Close > bd.Close ) {
               result = false;
			}
		}
		
		
		return result;
	}	

	
	
}
