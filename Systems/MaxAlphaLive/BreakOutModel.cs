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

	
	public bool calcBuy (BarData[] LookBackData , double body, double EmaValue, double Vwap) {
	
		bool result = true;

		if (LookBackData[0].BarStartTime.Hour < 11  || LookBackData[0].BarStartTime.Hour >= 15 ) {
            result = false;
		}

		if (LookBackData[0].Close < Vwap) {
            result = false;
		}
		
		if (EmaValue < Vwap) {
            result = false;
		}
		
		// Only shorting on down bar
		if (LookBackData[0].Close < LookBackData[0].Open) {
            result = false;
		}
		
		// Check Height of Bar
		if (body < bo_buy_height_param  ) {
			//result = false;
		}
		
		// Check Volume
		if (LookBackData[0].Volume <= 10000 || LookBackData[1].Volume <= 10000 || LookBackData[2].Volume <= 10000 ) {
			result = false;
		}
		
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
		
				if (  LookBackData[0].High < bd.High ) {
					result = false;
				}
			}
		}
		
		
		
		return result;
	}
	
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcShortInitial (BarData[] LookBackData , double body, double AdxValue) {
		
		// Default to Shorting
		//bool lookback = true;
		bool lookback = false;
		
		bool downbar = false;
		bool firstbar = false;
		bool secondbar = false;
		bool shortAll = false;
		
		bool price = true;
		
		// Check Height of Bar
		if (body < bo_buy_height_param  ) {
		//	lookback = false;
		}
		
		
		// Only short > 3 buck
		if (LookBackData[0].High <= 5.0) {
            price = false;
		}
				
		// Wait for first down bar
		if (LookBackData[0].Close < LookBackData[0].Open /* && LookBackData[1].Close < LookBackData[1].Open */) {
            downbar = true;
		}

		
		// Short Everything !!
		if ((LookBackData[0].BarStartTime.Hour < 10 && LookBackData[0].BarStartTime.Minute < 34) ) {
            shortAll = true;
		}
		
		
		// Only Trade in 9s or 10s
		if ( LookBackData[0].BarStartTime.Hour > 11) {
            lookback = false;
		}
	
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
		
				if (  LookBackData[0].High < bd.High /*|| bd.Volume == 0*/ ) {
					lookback = false;
				}
			}
		}
		
		
		
				
		// Check the BarData for Break Down
		foreach (BarData bd in LookBackData)
        {
			// Ignore current bar
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
			
				// previous bar was a breakout
				if (  LookBackData[1].High < bd.High || bd.Volume == 0) {
             		lookback = false;
				}	
			
			}
			
		}
				
		
	    //return (firstbar && secondbar) || (lookback && downbar);	
        //return (firstbar) || (lookback && downbar);	
		return (shortAll && downbar && price);	
	}
	
		
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcShort (BarData[] LookBackData , double body, double AdxValue) {
		
		// Default to Shorting
		bool result = true;
		
		bool down = true;
		
		// Check Level of ADX
		if (AdxValue > adx_param) {
			result = false;
		}
		
		// Only short > 3 buck
		if (LookBackData[0].Close <= 3.0) {
            down = false;
		}
				
		// Check Height of Bar
		if (body*-1 < bo_sell_height_param  ) {
			result = false;
		}
		
		// Only shorting on down bar
		if (LookBackData[0].Close > LookBackData[0].Open) {
            down = false;
		}
		
		// Needs to be past 9:35
		if ((LookBackData[0].BarStartTime.Hour < 10 && LookBackData[0].BarStartTime.Minute < 35) ) {
            result = false;
		}
		
	
		// Only Trade in 9s & 10s
		if (LookBackData[0].BarStartTime.Hour > 10 /* || LookBackData[0].BarStartTime.Hour > 15 */) {
            result = false;
		}
				
		
		// Check the BarData for BreakOut
		foreach (BarData bd in LookBackData)
        {
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
		
				if (  LookBackData[0].High <= bd.High /*|| bd.Volume == 0*/ ) {
					result = false;
				}
			}
		}
		
        return result && down;
	}
	
	
	
	// Determins a BreakOut Buy Signal for an Instrument
	public bool calcSell (BarData[] LookBackData , double body, double trailPrice, double Vwap, double Ema, SymbolScriptBase sb ) {
		
		// Default to Selling
		bool result = false;
		bool vwap = false;
		
		// Check Height of Bar
		if (body < bo_sell_height_param) {
		//	result = false;
		}
				
		if (LookBackData[0].Close > LookBackData[0].Open) {
            //result = false;
		}
	
		if (LookBackData[0].Close < Vwap) {
            //vwap = true;
		}
		
		foreach (BarData bd in LookBackData)
        {
			
			if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
			
				if (  LookBackData[0].Close > bd.Close ) {
				
		//			result = false;
					
				}
			}
		}
		
		// If past trail then sell
		if (LookBackData[0].Close <= trailPrice ) {
		//	result = true;
		}

		
		//If past trail then sell
		if (Ema <= Vwap ) {
			result = true;
		}
		
		
		return result || vwap;
	}	

	
		// Determins a BreakOut Buy Signal for an Instrument
	public bool calcCover (BarData[] LookBackData , double body, double trailPrice,  double Vwap, double Ema, SymbolScriptBase sb ) {
		
		// Default to Selling
		bool upbar = false;
		bool lookback = true;
		bool trail = false;
		bool crossVwap = false;
		
			
		// Check Height of Bar
		if (body < bo_sell_height_param) {
		//	 upbar = false;
		}
			
		
		// is current bar up ? 
		if ( LookBackData[0].Close > LookBackData[0].Open) {
			
			upbar = true;
		}
		
		
		// is it a break out ? 
		foreach (BarData bd in LookBackData)
        {
        	if( LookBackData[0].BarStartTime !=  bd.BarStartTime) {
			
				if ( LookBackData[0].Close < bd.High) {
						
					lookback = false;
				}
			}
		}
		
		// Trail it
		if (LookBackData[0].Close >= trailPrice ) {
			trail = true;
		}

		
		// Trail it
		if (LookBackData[0].BarStartTime.Hour >= 11 && LookBackData[0].Close >= Vwap ) {
		//	crossVwap = true;
		}
		
		//If past trail then sell
		if (LookBackData[0].BarStartTime.Hour >= 10 && LookBackData[0].BarStartTime.Minute >= 30 && Ema < Vwap ) {
		//	crossVwap = true;
		}
		
		
		//return trail || (upbar && lookback);
		return crossVwap;
	}	

	
	
	public bool calcTimeup (BarData[] LookBackData , double body, SymbolScriptBase sb ) {
		
		bool timeup = false;
		
		DateTime uptime = new DateTime(LookBackData[0].BarStartTime.Year, LookBackData[0].BarStartTime.Month, LookBackData[0].BarStartTime.Day, 15, 30, 0 );
		
		// time is up
		if (LookBackData[0].BarStartTime.Hour >= 15 && LookBackData[0].BarStartTime.Minute >= 55 ) {
        //    timeup = true;
		}
	
		return timeup;
		
	}
	
}
