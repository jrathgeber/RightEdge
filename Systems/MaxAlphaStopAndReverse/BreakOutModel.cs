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
		//	result = false;
		}
				
		// Check Height of Bar
		if (body < bo_buy_height_param  ) {
			result = false;
		}

		// Must be an up day
		if (LookBackData[0].Close < LookBackData[0].Open) {
            result = false;
		}
	
		// Only Trade in 9s
		if (LookBackData[0].BarStartTime.Hour < 10 || LookBackData[0].BarStartTime.Hour > 15) {
            result = false;
		}
		
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
		
				if (  LookBackData[0].High + body < bd.High /*|| bd.Volume == 0*/ ) {
					result = false;
				}
			}
		}
						
        return result;
	}
	
		// Determins a BreakOut Buy Signal for an Instrument
	public bool calcFlip (BarData[] LookBackData , double body, double AdxValue) {
		
		// Default to Buying
		bool result = true;

				
		// Check Level of ADX
		if (AdxValue > adx_param) {
		//	result = false;
		}
				
		// Check Height of Bar
		if (body < bo_buy_height_param  ) {
		//	result = false;
		}

		// Must be an up day
		//if (LookBackData[0].Close < LookBackData[0].Open) {
        //    result = false;
		//}
	
		// Only Trade in 9s
		//if (LookBackData[0].BarStartTime.Hour < 10 || LookBackData[0].BarStartTime.Hour > 15) {
        //    result = false;
		//}
		
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
				if (  LookBackData[0].High < bd.High /*|| bd.Volume == 0*/ ) {
				result = false;
				}
			}
		}
						
        return result;
	}
	
	public bool calcTimeup (BarData[] LookBackData , double body, SymbolScriptBase sb ) {
		
		bool timeup = false;
		
		DateTime uptime = new DateTime(LookBackData[0].BarStartTime.Year, LookBackData[0].BarStartTime.Month, LookBackData[0].BarStartTime.Day, 15, 30, 0 );
		
		// time is up
		if (LookBackData[0].BarStartTime.Hour >= 15 && LookBackData[0].BarStartTime.Minute >= 55 ) {
            timeup = true;
		}
	
		return timeup;
		
	}
	
	
	
	
	
	
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcSell (BarData[] LookBackData , double body, double trailPrice, SymbolScriptBase sb ) {
		
		// Default to Selling
		bool result = true;
		
			
		// Check Height of Bar
		if (body < bo_sell_height_param) {
		//	result = false;
		}
				
		if (LookBackData[0].Close > LookBackData[0].Open) {
            result = false;
		}
		
		
		foreach (BarData bd in LookBackData)
        {
			
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
			
				if (  LookBackData[0].Close > bd.Close ) {
				
					result = false;
					
				}
			}
		}


		
		// If past trail then sell
		if (LookBackData[0].Close <= trailPrice ) {
		
			result = true;
			
		}
		
		
		
		return result;
	}	

	
	
}
