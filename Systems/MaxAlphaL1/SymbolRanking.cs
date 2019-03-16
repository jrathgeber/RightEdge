#region Using statements
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO; // StreamWriter
using System.Text; // StringBuilder
using RightEdge.Common;
using RightEdge.Common.ChartObjects;
using RightEdge.Indicators;
using DoQ_Indicators;
using System.Linq;

#endregion

public class MySystem : MySystemBase
{
	StreamWriter sw = null;
	StreamWriter sw2 = null;
	
	// Until we have 25k
	public bool systemTradedToday { get; set; }
	public int dayTradesLeft { get; set; }
		
	public override void Startup()
	{
		systemTradedToday = false;
		dayTradesLeft = (int) SystemParameters["DayTradesLeft"];
		
	}
		
	
	public override void Shutdown()
	{
	//		saveOutput();
	}
		
	
	public void sendMail(String subjecttext, String messagetext) {
			
		System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
					
		message.To.Add("jrathgeber@yahoo.com");
		message.Subject = subjecttext;
		message.From = new System.Net.Mail.MailAddress("jrathgeber@yahoo.com");
		message.Body = "MA is Starting " + messagetext;
		System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.yahoo.com", 587);
		
		smtp.EnableSsl =true;
		smtp.Credentials = new System.Net.NetworkCredential("jrathgeber", "");
		
		
		smtp.Send(message);

	}	
	
	
	public void saveOutput() {
		
		string path1 = Path.Combine("C:\\dec\\RightEdge\\Systems\\MaxAlphaLive","output.html");
		sw = new StreamWriter(path1);
	
		string path2 = Path.Combine("C:\\dec\\RightEdge\\Systems\\MaxAlphaLive","output.txt");
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
		sw.WriteLine("<h1>MaxShort Breakout Robot</h1>");
		
		sw.WriteLine("<h3>Parameters</h3>");
		
		sw.WriteLine("BreakOutBuyBars 	: " + SystemParameters["BreakOutBuyDays"] + "<BR>");
		sw.WriteLine("BreakOutBuyBars 	: " + SystemParameters["BreakOutSellDays"] + "<BR>");
		
		sw.WriteLine("<BR>");
		
		sw.WriteLine("StopLoss 			: " + SystemParameters["StopLoss"] + "<BR>");
		sw.WriteLine("TakeProfit 		: " + SystemParameters["TakeProfit"] + "<BR>");
		
		sw.WriteLine("<BR>");
		
	
		sw.WriteLine("<h2>Open Positions</h2>");

		sw.WriteLine("<div class='datagrid'><table>");
		sw.WriteLine("<thead><tr><th align='left'>Symbol</th><th align='left'>Type</th><th align='left'>EntryPrice</th><th align='left'>OpenDate</th><th align='left'>CurrentSize</th><th align='left'>CurrentValue</th><th align='left'>UnrealizedProfit</th></tr></thead>");		
		
		foreach(MySymbolScript SymbolScript in SymbolScripts)
		{	
			foreach(Position p in SymbolScript.OpenPositions)
			{
					sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td>"+ p.Type + "</td><td>"+ p.EntryPrice.SymbolPrice.ToString("#.##") + "</td><td>"+ p.OpenDate.ToString("dd-MMM-yy") + "</td><td align='right'>"+ p.CurrentSize.ToString("#.##") + "</td><td align='right'>"+ p.CurrentValue.ToString("#.##") + "</td><td align='right'>"+ p.UnrealizedProfit.ToString("#.##") + "</td></tr>");	
				
					unRealizedProfit = unRealizedProfit + p.UnrealizedProfit;
			}			
		}	
		sw.WriteLine("<tfoot><tr><td colspan='7'><div id='no-paging'>Total Un-Realized Profit : " + unRealizedProfit.ToString("#.##") + "</div></tr></tfoot>");		
		sw.WriteLine("</tbody>");
		sw.WriteLine("</table>");
		sw.WriteLine("</div>");			
		
		sw.WriteLine("<h2>Closed Positions</h2>");
		sw.WriteLine("<div class='datagrid'><table>");
		sw.WriteLine("<thead><tr><th align='left'>Symbol</th><th align='left'>Type</th><th align='left'>EntryPrice</th><th align='left'>ExitPrice</th><th align='left'>OpenDate</th><th align='left'>ExitType</th><th align='left'>MaxSize</th><th align='left'>RealizedProfit</th></tr></thead>");

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
			
			sw.WriteLine("<tr><td>"+ p.Symbol + "</td><td>"+ p.Type+ "</td><td>" + p.GrossEntryPrice.SymbolPrice.ToString("#.##") + "</td><td>"+ p.ExitPrice.SymbolPrice.ToString("#.##") + "</td><td>"+ p.OpenDate.ToString("dd-MMM-yy") + "</td><td>"+ p.ExitTransactionType+ "</td><td align='right'>"+ p.MaxSize.ToString ("#.##") + "</td><td align='right'>"+ p.RealizedProfit.ToString ("#.##") + "</td></tr>");
			
			
		}
		

		sw.WriteLine("<tfoot><tr><td colspan='8'><div id='no-paging'>Total Realized Profit : " + realizedProfit.ToString("#.##") + " </div></tr></tfoot>");	
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
	
	
	
	public void doRanking() {

		
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
			if (position_count<=5) {
			
			}	
			
			// just one trade at a time
			break;
		}	

	}	
	
	
	public override void NewBar()
	{
		
		//	Call NewBar in the SymbolScripts so they can calculate their RankValue
		base.NewBar();
	
		//	Order the symbol scripts so the ones with a higher RankValue come first
		doRanking();
		
		// Save Output @ 3:30
		TimeSpan start = new TimeSpan(15, 30, 0); //10 o'clock
		TimeSpan end = new TimeSpan(16, 0, 0); //12 o'clock
		TimeSpan now = DateTime.Now.TimeOfDay;
			
		if ((now > start) && (now < end))
		{
		//	saveOutput();
		}	
				
	}
}

public class MySymbolScript : MySymbolScriptBase
{
	public double RankValue { get; private set; }
	public int Rank { get; set; }
	
	public double AdxValue { get; private set; }
	public double VwapValue { get; private set; }
	
	RelativeStrength RSI;
    COGOscillator COG;
	
	ADX ADX;

    EMA MAONE;
    EMA MATWO;
    
	BreakOutModel BO;
	
	VWAP VWAP;

	private bool tradedTodayShort = false;
	private bool tradedTodayLong = false;
	
	private double trailLongPrice = 0.0;
	private double trailShortPrice = 100.0;

	private double entryLongPrice = 0.0;
	private double entryShortPrice = 100.0;
	
	
    public override void Startup()
	{
		double BO_BUY_DAYS = SystemParameters["BreakOutBuyDays"]; 
        double BO_SELL_DAYS = SystemParameters["BreakOutSellDays"]; 
		
        double BO_BUY_HEIGHT = SystemParameters["BreakOutBuyHeight"]; 
        double BO_SELL_HEIGHT = SystemParameters["BreakOutSellHeight"]; 

		double STOP_LOSS = SystemParameters["StopLoss"]; 
        double TAKE_PROFIT = SystemParameters["TakeProfit"]; 
		
		trailLongPrice = SystemParameters["TrailLong"];
		trailShortPrice = SystemParameters["TrailShort"];
		
        double ADX_PARM = SystemParameters["ADXPeriods"];
        //ADX = new ADX((int)ADX_PARM);
		ADX = new ADX(14);
		
		double RSI_PARM = SystemParameters["RSIPeriods"];
        RSI = new RelativeStrength((int)RSI_PARM, Close);
		
        double COG_PARM = SystemParameters["COG"];
        COG = new COGOscillator((int)COG_PARM);

		VWAP = new VWAP(RightEdge.Common.BarElement.Close,1);
		
		BO = new BreakOutModel(BO_BUY_DAYS, BO_SELL_DAYS, BO_BUY_HEIGHT, BO_SELL_HEIGHT, ADX_PARM);
				
		PositionManager.StopLoss = STOP_LOSS;
		PositionManager.ProfitTarget = TAKE_PROFIT;
		
        ConfigureMAs();
		
		OutputMessage("START UP {} ");
		
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
	
	private BarData[] getLookBackBarDataBuy (int daysBack, RList<BarData> Bars) {
		
			BarData[] lbd = 	new BarData[daysBack +1 ]; 
				
			int[] numbersInArray = Enumerable.Range(1, daysBack).ToArray();	
						
			lbd[0] = Bars.Current;	
				
			foreach (int i in numbersInArray)
			{
				lbd[i] = Bars.LookBack(i);
			}	
		
			return lbd;
	
	}	


	
	private BarData[] getLookBackBarDataSell (int daysBack, RList<BarData> Bars) {
		
			BarData[] lbd = 	new BarData[daysBack +1 ]; 
			
			int[] numbersInArray = Enumerable.Range(1, daysBack).ToArray();	
						
			lbd[0] = Bars.Current;	
				
			foreach (int i in numbersInArray)
			{
				lbd[i] = Bars.LookBack(i);
			}	
		
			return lbd;
	}

	
	public void calcRsi() {
		
		// Check the RSI
		if (double.IsNaN(RSI.Current))
		{
			RankValue = 0;
		}
		else
        {
        	RankValue = Math.Abs(RSI.Current);
        }
		
		
	}	
	

	
	//	In the NewBar method, calculate and set the RankValue property
	public override void NewBar()
	{
		if (Bars.Current.BarStartTime.Hour < 10 && Bars.Current.BarStartTime.Minute <= 30) {

			tradedTodayShort = false;
			tradedTodayLong = false;
		
		}

			
		// Check that Symbol folder = current bar date
		String cd = this.Symbol.SymbolInformation.CustomHistoricalData;
		String bcs = Bars.Current.BarStartTime.ToString("yyyyMMdd");

		if (cd == null) {
    		cd = "";
		}
				
		if(!cd.Equals(bcs)) {
		
			return;
		}
		
		// Check the RSI
		if (double.IsNaN(RSI.Current))
		{
			RankValue = 0;
		}
		else
        {
        	RankValue = Math.Abs(RSI.Current);
        }
		
		int lookBackBuy = (int)BO.bars_to_use;
		int lookBackSell = (int)BO.bars_to_sell;
		
		// Need 4 days to decide
		if (Bars.Count <= lookBackBuy)
		{
			return;
		}	

		// Need 4 days to decide
		if (Bars.Count <= lookBackSell)
		{
			return;
		}	
		
		
		
		// Get Objects that hold the Bars to lookback
		BarData[] LookBackDataBuy = getLookBackBarDataBuy(lookBackBuy, Bars);
		BarData[] LookBackDataSell = getLookBackBarDataSell(lookBackSell, Bars);
		
		// Calc the body
		// double body = (Bars.Current.Close - Bars.Current.Open) / Bars.Current.Open ;
		// double body = (Bars.Current.Close - Bars.Current.Open) ;
		double body = (LookBackDataBuy[0].Close - LookBackDataBuy[0].Open) ;
		
		
		// Set Trail Long
		if (Bars.Current.Close > entryLongPrice + 0.25) {
			//trailLongPrice = Bars.Current.Low - 0.25;
			trailLongPrice = Math.Max(trailLongPrice, Bars.Current.High - 0.25);
			//trailLongPrice = entryLongPrice;
		}	
		
		// Set Trail Short
		if (Bars.Current.Close < trailShortPrice - 0.25) {
			trailShortPrice = Math.Min(trailShortPrice, Bars.Current.High + 0.25);
			trailShortPrice = entryShortPrice;
		}	
		
		
		
		// Calc the ADX
		AdxValue = Math.Abs(ADX.Current);
								
		// Calc the VWAP
		VwapValue = VWAP.Current;
		
		// Decide what to do
		if (OpenPositions.Count == 0)
		{
			OutputMessage("Open Positions [" + OpenPositions.Count + "] [" + RankValue + "] [" + Rank + "]");

			// Std
			if ( BO.calcShortInitial(LookBackDataBuy, body, AdxValue) == true && Rank ==1)
			{
				OutputMessage("CALC Short Initial is True: " + Bars.Current.BarStartTime.ToString() + " By [" + body + "]");
						
				if (OpenPositions.Count == 0 && tradedTodayShort == false && TradingSystem.systemTradedToday == false)
				{
					OpenPosition(PositionType.Short, OrderType.Market);
					
					tradedTodayShort = true;
				}
			}

			
			// Check if a buy
			if ( BO.calcBuy(LookBackDataBuy, body, AdxValue, VwapValue) == true )
			{
				OutputMessage("CALC Buy is True: " + Bars.Current.BarStartTime.ToString());
						
				if (OpenPositions.Count == 0 && tradedTodayLong == false && TradingSystem.systemTradedToday == false)
				{
					
						PositionSettings settings = new PositionSettings(); 
						settings.PositionType = PositionType.Long; 
						settings.OrderType = OrderType.Market; 
						settings.Size = 100; 
						settings.BarsValid = 5;              						
						settings.ProfitTarget = 1.00;        						 
						settings.ProfitTargetType = TargetPriceType.RelativePrice;  
						settings.StopLoss = .25;        						 
						settings.StopLossType = TargetPriceType.RelativePrice;  
																		
						OutputMessage("Going Long");
						OpenPosition(settings);
						tradedTodayLong = true;
										
						//OpenPosition(PositionType.Long, OrderType.Market);
						//tradedTodayLong = true;
					
				}
			}
			
			// Short Topside Down
			if ( BO.calcShort(LookBackDataSell, body, AdxValue) == true  )
			{
	
			
				// Check if a sell
				if ( OpenPositions.Count == 0 && tradedTodayShort == false && TradingSystem.systemTradedToday == false )
				{
					OutputMessage("CALC Short is True: [" + body + "]");
						
					// Now go long !!!
					if (OpenPositions.Count <=2	  /* & OpenPositions.SingleOrDefault().Type == PositionType.Short */ )
					{
						
						PositionSettings settings = new PositionSettings(); 
						settings.PositionType = PositionType.Short; 
						settings.OrderType = OrderType.Market; 
						settings.Size = 100; 
						settings.BarsValid = 5;              						
						settings.ProfitTarget = .30;        						 
						settings.ProfitTargetType = TargetPriceType.RelativePrice;  
						settings.StopLoss = .40;        						 
						settings.StopLossType = TargetPriceType.RelativePrice;  
																		
						OutputMessage("Shorting More");
						//OpenPosition(PositionType.Short, OrderType.Market);
						OpenPosition(settings);
						tradedTodayShort = true;
							
					}
				}		
			
			}
			
		} else {
			
			bool cover = BO.calcCover(LookBackDataSell, body, trailShortPrice, this);
			bool timeupShort = BO.calcTimeup(LookBackDataBuy, body, this);
			bool timeupLong = BO.calcTimeup(LookBackDataBuy, body, this);
			bool sellLong = BO.calcSell(LookBackDataSell, body, trailLongPrice, this, VwapValue);
			bool shortMore = BO.calcBuy(LookBackDataSell, body, AdxValue, VwapValue);
			
			OutputMessage("Close [" + LookBackDataBuy[0].Close + "] Open [" + LookBackDataBuy[0].Open + "] Trail ["+trailLongPrice+"]");
			
			// Short More
			if ( BO.calcShort(LookBackDataBuy, body, AdxValue) == true )
			{
				OutputMessage("Shorting More! Body [" + body + "]");
					
				// Now go long !!!
				if (OpenPositions.Count <=2	  /* & OpenPositions.SingleOrDefault().Type == PositionType.Short */ )
				{
					OutputMessage("Shorting More");
					OpenPosition(PositionType.Short, OrderType.Market);
					tradedTodayShort = true;
						
				}
			}			
			

			// Cover
			if ( cover == true  )
			{
				OutputMessage("Cover[" + cover + "] trail ["+trailShortPrice+"]");

				//	Loop through any open positions for this symbol and close them
				foreach(Position pos in OpenPositions)
				{
					if(pos.Type == PositionType.Short) {
					
						OutputMessage("Covering");
						
						pos.CloseAtMarket();

					}
				}
				
			}
			

			// Sell a long
			if ( sellLong == true )
			{
				OutputMessage("Sell [" + sellLong + "] trail ["+trailLongPrice+"]");
							
				//	Loop through any open positions for this symbol and close them
				foreach(Position pos in OpenPositions)
				{
					if(pos.Type == PositionType.Long) {
					
						OutputMessage("Selling");
						
						pos.CloseAtMarket();

					}
				}	
			}
						
			
			
			// EOD
			if ( timeupShort == true )
			{
				//	Loop through any open positions for this symbol and close them
				foreach(Position pos in OpenPositions)
				{
					if(pos.Type == PositionType.Short) {
					
						OutputMessage("Covering");
			
						pos.CloseAtMarket();

					}
				}
			}
			
		
			
			// EOD
			if ( timeupLong == true )
			{
				//	Loop through any open positions for this symbol and close them
				foreach(Position pos in OpenPositions)
				{
					if(pos.Type == PositionType.Long) {
					
						OutputMessage("Selling");
			
						pos.CloseAtMarket();

					}
				}
			}			
		}
	}


	//	In the Trade method, make trading decisions based on the Rank property
	//	(and whatever other criteria you need)
	public void Trade()
	{	

		String ff = DateTime.Today.ToString();
		
		if (Rank >= 1)
		{
			if (OpenPositions.Count == 0)
			{
				OpenPosition(PositionType.Short, OrderType.Market);

				OutputMessage("Open Position Called !! " + ff);
				
				if(this.Bars.Current.BarStartTime >= DateTime.Today)
				{
					//sendMail("Ma : Buy " + this.Symbol + " at " + this.Close.Current , "Hope u happy");
				}
			}
		}
		else
		{
				//PositionManager.CloseAllPositions(Symbol);
		}

	}

	public override void OrderFilled(Position position, Trade trade)
	{
		
		//OutputMessage("Order Filled !! " + trade.Description + trade.FilledTime);
		if(position.Type == PositionType.Long) {
					
			tradedTodayLong = true;
			trailLongPrice = position.EntryPrice.SymbolPrice - 0.75 ;
			entryLongPrice = position.EntryPrice.SymbolPrice ;
			
		}

		
		if(position.Type == PositionType.Short) {
			
			tradedTodayShort = true;
			trailShortPrice = position.EntryPrice.SymbolPrice + 0.75 ;
			entryShortPrice = position.EntryPrice.SymbolPrice ;
			
		}
		
		
		// Finally updated system status
		TradingSystem.systemTradedToday = false;
		TradingSystem.dayTradesLeft = TradingSystem.dayTradesLeft - 1;
		
	}

	public override void OrderCancelled(Position position, Order order, string information)
	{
		// This method is called when an order is cancelled or rejected

	}
	
	
	
	public void sendMail(String subjecttext, String messagetext) {
			
		System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
					
		message.To.Add("jrathgeber@yahoo.com");
		message.Subject = subjecttext;
		message.From = new System.Net.Mail.MailAddress("jrathgeber@yahoo.com");
		message.Body = "You have" + messagetext;
		System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.yahoo.com", 587);
		
		smtp.EnableSsl =true;
		smtp.Credentials = new System.Net.NetworkCredential("jrathgeber", "");
		
		
		smtp.Send(message);

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
