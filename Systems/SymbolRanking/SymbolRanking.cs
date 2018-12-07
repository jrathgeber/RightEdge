#region Using statements
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO; // StreamWriter
using System.Text; // StringBuilder
using RightEdge.Common;
using RightEdge.Common.ChartObjects;
using RightEdge.Indicators;
using System.Linq;
#endregion

public class MySystem : MySystemBase
{
	StreamWriter sw = null;
	StreamWriter sw2 = null;
	
	public override void Startup()
	{
		// Perform initialization or set system wide options here
		//OutputMessage("START UP {} ");
	

	}
	
	public override void Shutdown()
	{
		
		
		string path1 = Path.Combine("C:\\dec\\RightEdge\\Systems\\SymbolRanking","output.html");
		sw = new StreamWriter(path1);
	
		string path2 = Path.Combine("C:\\dec\\RightEdge\\Systems\\SymbolRanking","output.txt");
		sw2 = new StreamWriter(path2);
				
		double realizedProfit = 0.0;
		double unRealizedProfit = 0.0;
		double totalProfit = 0.0;
		
		sw.WriteLine("<!DOCTYPE html>");
		sw.WriteLine("<html>");
		
		sw.WriteLine("<style media='screen' type='text/css'>");
		sw.WriteLine(".datagrid table { border-collapse: collapse; text-align: left; width: 100%; } .datagrid {font: normal 12px/150% Arial, Helvetica, sans-serif; background: #fff; overflow: hidden; border: 1px solid #006699; -webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; }.datagrid table td, .datagrid table th { padding: 3px 10px; }.datagrid table thead th {background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');background-color:#006699; color:#FFFFFF; font-size: 15px; font-weight: bold; border-left: 1px solid #0070A8; } .datagrid table thead th:first-child { border: none; }.datagrid table tbody td { color: #00496B; border-left: 1px solid #E1EEF4;font-size: 12px;font-weight: normal; }.datagrid table tbody .alt td { background: #E1EEF4; color: #00496B; }.datagrid table tbody td:first-child { border-left: none; }.datagrid table tbody tr:last-child td { border-bottom: none; }.datagrid table tfoot td div { border-top: 1px solid #006699;background: #E1EEF4;} .datagrid table tfoot td { padding: 0; font-size: 12px } .datagrid table tfoot td div{ padding: 2px; }.datagrid table tfoot td ul { margin: 0; padding:0; list-style: none; text-align: right; }.datagrid table tfoot  li { display: inline; }.datagrid table tfoot li a { text-decoration: none; display: inline-block;  padding: 2px 8px; margin: 1px;color: #FFFFFF;border: 1px solid #006699;-webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');background-color:#006699; }.datagrid table tfoot ul.active, .datagrid table tfoot ul a:hover { text-decoration: none;border-color: #006699; color: #FFFFFF; background: none; background-color:#00557F;}div.dhtmlx_window_active, div.dhx_modal_cover_dv { position: fixed !important; }");
		sw.WriteLine("</style>");
		
		sw.WriteLine("<body>");
		sw.WriteLine("<h1>Jason Breakout Robot</h1>");
		
		sw.WriteLine("<h2>New Trades</h2>");
						
		sw.WriteLine("<div class='datagrid'><table>");
		sw.WriteLine("<thead><tr><th align='left'>Symbol</th><th align='left'>Type</th><th align='left'>Size</th><th align='left'>Link</th></tr></thead>");
		
		sw.WriteLine("<tbody>");
		
		foreach(MySymbolScript SymbolScript in SymbolScripts)
		{
			// Buys
			foreach(Position p in SymbolScript.PendingPositions)
			{
				int i = p.Orders.Count;
			
				if(i>0) {
			
					sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td>" + p.Type + "</td><td>" + p.Orders.ElementAt(0).Size + "</td><td><a href='https://finance.yahoo.com/quote/" + p.Symbol + "'>Yahoo</a></td></tr>");
					sw2.WriteLine(p.Symbol);
										
				}	else {
				
					sw.WriteLine("<tr><td> No Buy Trades </td><td>data</td><td>-</td><td>-</td></tr>");
				}
			}
			
			// Sells
			foreach(Position p in SymbolScript.OpenPositions)
			{
				bool pc = p.PendingClose;
			
				if(pc==true) {
			
					sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td> "+ "Sell" + "</td><td>"+ p.CurrentSize + "</td><td><a href='https://finance.yahoo.com/quote/" + p.Symbol + "'>Yahoo</a></td></tr>");
					sw2.WriteLine(p.Symbol);
										
				}	else {
				
					//sw.WriteLine("<tr><td> No Sell Trades </td><td>-</td><td>-</td><td>-</td></tr>");
				}
			}			
			
			
			
		}
		sw.WriteLine("<tfoot><tr><td colspan='4'><div id='no-paging'>Totals</div></tr></tfoot>");	
		sw.WriteLine("</tbody>");
		sw.WriteLine("</table>");
		sw.WriteLine("</div>");				

		sw.WriteLine("<h2>Open Positions</h2>");

		sw.WriteLine("<div class='datagrid'><table>");
		sw.WriteLine("<thead><tr><th align='left'>Symbol</th><th align='left'>EntryPrice</th><th align='left'>OpenDate</th><th align='left'>CurrentSize</th><th align='left'>CurrentValue</th><th align='left'>UnrealizedProfit</th></tr></thead>");		
		
		foreach(MySymbolScript SymbolScript in SymbolScripts)
		{	
			foreach(Position p in SymbolScript.OpenPositions)
			{
					sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td>"+ p.EntryPrice.SymbolPrice.ToString("#.##") + "</td><td>"+ p.OpenDate.ToString("dd-MMM-yy") + "</td><td align='right'>"+ p.CurrentSize.ToString("#.##") + "</td><td align='right'>"+ p.CurrentValue.ToString("#.##") + "</td><td align='right'>"+ p.UnrealizedProfit.ToString("#.##") + "</td></tr>");	
				
					unRealizedProfit = unRealizedProfit + p.UnrealizedProfit;
			}			
		}	
		sw.WriteLine("<tfoot><tr><td colspan='6'><div id='no-paging'>Total Un-Realized Profit : " + unRealizedProfit.ToString("#.##") + "</div></tr></tfoot>");		
		sw.WriteLine("</tbody>");
		sw.WriteLine("</table>");
		sw.WriteLine("</div>");			
		
		sw.WriteLine("<h2>Closed Positions</h2>");
		sw.WriteLine("<div class='datagrid'><table>");
		sw.WriteLine("<thead><tr><th align='left'>Symbol</th><th align='left'>ExitPrice</th><th align='left'>OpenDate</th><th align='left'>ExitType</th><th align='left'>MaxSize</th><th align='left'>RealizedProfit</th></tr></thead>");

		SortedList<string, Position> sorted = new SortedList<string, Position>();
				
		foreach(MySymbolScript SymbolScript in SymbolScripts)
		{
		
			foreach(Position raw in SymbolScript.ClosedPositions)
			{
				sorted.Add(raw.OpenDate.ToString("dd-MMM-yy:hh:mm:ss")+"_"+raw.Symbol+"_"+raw.EntryPrice, raw);

				realizedProfit = realizedProfit + raw.RealizedProfit;
			
			}
			
		}
		
	    var descSorted = sorted.Reverse();
		
		foreach (var pair in descSorted)
		{
			
			KeyValuePair<string, Position> kvp = (KeyValuePair<string, Position>)pair;
			
			Position p = kvp.Value;
			
			sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td>"+ p.ExitPrice.SymbolPrice.ToString("#.##") + "</td><td>"+ p.OpenDate.ToString("dd-MMM-yy") + "</td><td>"+ p.ExitTransactionType+ "</td><td align='right'>"+ p.MaxSize.ToString ("#.##") + "</td><td align='right'>"+ p.RealizedProfit.ToString ("#.##") + "</td></tr>");
			
			
		}

		sw.WriteLine("<tfoot><tr><td colspan='6'><div id='no-paging'>Total Realized Profit : " + realizedProfit.ToString("#.##") + " </div></tr></tfoot>");	
		sw.WriteLine("</tbody>");
		sw.WriteLine("</table>");
		sw.WriteLine("</div>");
		
		totalProfit = realizedProfit + unRealizedProfit;
		
		sw.WriteLine("<h2>Total Profit:" + totalProfit.ToString("#.##") + "</h2>");
		
		sw.WriteLine("</body>");
		sw.WriteLine("</html>");
		
		if(sw != null) sw.Close();	
		if(sw2 != null) sw2.Close();
		
		
		
		
	}
	
	
	public override void NewBar()
	{
		//	Call NewBar in the SymbolScripts so they can calculate their RankValue
		base.NewBar();
		
		//	Order the symbol scripts so the ones with a higher RankValue come first
		List<MySymbolScript> orderedSymbolScripts =	SymbolScripts.OrderByDescending(ss => ss.RankValue).ToList();
		
        //	Update rankings
        for (int i = 0; i < orderedSymbolScripts.Count; i++)
        {

            if (orderedSymbolScripts[i].RankValue > 0)
            {

                //	 Add one to i so the first symbol has a rank of 1, not 0
                orderedSymbolScripts[i].Rank = i + 1;

            } else
            {

                orderedSymbolScripts[i].Rank = 0;

            }
        }
		
		int position_count = 0;
				
		//	Call Trade method
		foreach (MySymbolScript ss in orderedSymbolScripts)
		{
			position_count += ss.OpenPositions.Count;
		}
		
		
		//	Call Trade method
		foreach (MySymbolScript ss in orderedSymbolScripts)
		{
			// If no positions open its trade time
			if (position_count<=1) {
			
				ss.Trade();
			}	
			
			// just one trade at a time
			break;
		}
	}
}

public class MySymbolScript : MySymbolScriptBase
{
	public double RankValue { get; private set; }
	public int Rank { get; set; }
	
	public double AdxValue { get; private set; }
		
	RelativeStrength RSI;
    COGOscillator COG;
	
	ADX ADX;

    EMA MAONE;
    EMA MATWO;
    
    BreakOutModel BO;

    public override void Startup()
	{
		double BO_BUY_DAYS = SystemParameters["BreakOutBuyDays"]; 
        double BO_SELL_DAYS = SystemParameters["BreakOutSellDays"]; 
		
        double BO_BUY_HEIGHT = SystemParameters["BreakOutBuyHeight"]; 
        double BO_SELL_HEIGHT = SystemParameters["BreakOutSellHeight"]; 
		
		double STOP_LOSS = SystemParameters["StopLoss"]; 
        double TAKE_PROFIT = SystemParameters["TakeProfit"]; 
		
        double ADX_PARM = SystemParameters["ADXPeriods"];
        ADX = new ADX((int)ADX_PARM);
		
		double RSI_PARM = SystemParameters["RSIPeriods"];
        RSI = new RelativeStrength((int)RSI_PARM, Close);
		
        double COG_PARM = SystemParameters["COG"];
        COG = new COGOscillator((int)COG_PARM);

		BO = new BreakOutModel(BO_BUY_DAYS, BO_SELL_DAYS, BO_BUY_HEIGHT, BO_SELL_HEIGHT, ADX_PARM);
		
		PositionManager.StopLoss = STOP_LOSS;
		PositionManager.ProfitTarget = TAKE_PROFIT;
		
		
        ConfigureMAs();
		
		
    }
	
	
	private void testOrder () {
		
	  		OutputMessage("Test {} ");
				
             PositionSettings ps = new PositionSettings();
		
             ps.Description = "Buy";
             ps.OrderType = OrderType.Market;
             ps.PositionType = PositionType.Long;
		
			 		
			 ps.Size = 1000;
                   
             Position pos = this.OpenPosition(ps);
		
			//OpenPosition(PositionType.Long, OrderType.Market);
			OutputMessage("Open Position Called !! " + ps.OpenOrder);
			
		
	}
	
	
	
	
	// This method returs a a bars data object of the correct size
	private BarData[] getLookBackBarData (int daysBack, RList<BarData> Bars) {
		
		BarData[] lbd = 	new BarData[daysBack +1 ]; 
			
		int[] numbersInArray = Enumerable.Range(1, daysBack).ToArray();	
					
		lbd[0] = Bars.Current;	
			
		foreach (int i in numbersInArray)
		{
			lbd[i] = Bars.LookBack(i);
		}	
		
	return lbd;
	
	}
	
	//	In the NewBar method, calculate and set the RankValue property
	public override void NewBar()
	{
		RankValue = 0;
		
		int lookBackBuy = (int)BO.bars_to_use;
		int lookBackSell = (int)BO.bars_to_sell;
		
		// Need 4 days to decide
		//if (Bars.Count <= lookBackBuy || Bars.Count <= lookBackSell)
		if (Bars.Count <= lookBackBuy)
		{
			return;
		}		
		
		// Get Objects that hold the Bars to lookback
		BarData[] LookBackDataBuy = getLookBackBarData(lookBackBuy, Bars);
		BarData[] LookBackDataSell = getLookBackBarData(lookBackSell, Bars);
		
		// Calc the body
		double body = (Bars.Current.Close - Bars.Current.Open) / Bars.Current.Open ;
		
		// Calc the ADX
		AdxValue = Math.Abs(ADX.Current);
								
		// Decide what to do
		if (OpenPositions.Count == 0)
		{
			OutputMessage("New bar close: [" + LookBackDataBuy[0].Close + "] open: [" + LookBackDataBuy[0].Open + "] : "  + Bars.Current.BarStartTime.ToString());
								
			// Std
			if ( BO.calcBuy(LookBackDataBuy, body, AdxValue) == true )
			{
				OutputMessage("CALC BUY HAS Been True: " + Bars.Current.BarStartTime.ToString());
								
				// Check the RSI
				if (double.IsNaN(RSI.Current))
				{
					RankValue = 0;
				}
				else
                {
                    RankValue = Math.Abs(RSI.Current);
                    OutputMessage("Rank Value [" + RankValue + "]");
                }
			}
			
		} else {

			if ( BO.calcSell(LookBackDataSell, body) == true  )
			{
				//	Loop through any open positions for this symbol and close them
				foreach(Position pos in OpenPositions)
				{
					pos.CloseAtMarket();
				}
			}
		}
	}

	


	//	In the Trade method, make trading decisions based on the Rank property
	//	(and whatever other criteria you need)
	public void Trade()
	{	
		if (Rank >= 1)
		{
			if (OpenPositions.Count == 0)
			{
				
				
				OpenPosition(PositionType.Long, OrderType.Market);
				OutputMessage("Open Position Called !! ");
			}
		}
		else
		{
			PositionManager.CloseAllPositions(Symbol);
		}

	}

	public override void OrderFilled(Position position, Trade trade)
	{
		// This method is called when an order is filled
		OutputMessage("Order Filled !! " + trade.Description + trade.FilledTime);

	}

	public override void OrderCancelled(Position position, Order order, string information)
	{
		// This method is called when an order is cancelled or rejected

	}
	
    public void ConfigureMAs()
    {

        double MA1 = SystemParameters["MA1"];
        double MA2 = SystemParameters["MA2"];

        MAONE = new EMA(Convert.ToInt32(MA1));
        MATWO = new EMA(Convert.ToInt32(MA2));


        // Note: SMA's are chainable indicators and can use
        // bar elements as well as other indicators as input.  In this
        // case, we'll simply set the input as the bar's closing price
        MAONE.SetInputs(Close);

        // Set the line color to green.
        MAONE.ChartSettings.Color = Color.Maroon;

        // Let's make our signal line dashed.
        //SMA50.ChartSettings.LineType = SeriesLineType.Dashed;
        MATWO.ChartSettings.LineSize = 3;

        // Now set up the "slow" 200 period moving average.
        MATWO.SetInputs(Close);

        // Set the line color to Crimson.
        MATWO.ChartSettings.Color = Color.Blue;

        // Let's make our slow line thicker
        MATWO.ChartSettings.LineSize = 3;
    }

	
}
