using UnityEngine;
using System.Collections;

public static class TempestUtil
{
	public static string FormatSeconds(int seconds)
	{
		string formattedTime = "";
		int timePassedHr = 0;
		int timePassedMin = 0;
		int timePassedSec = 0;

		if(seconds>=60)
		{
			timePassedMin=seconds/60;
			//if it's been more than an hour...
			if(timePassedMin>=60)
			{
				timePassedHr=seconds/3600;
				timePassedMin=timePassedMin%60;
			}
		}
		timePassedSec=seconds%60;

		formattedTime="";			
		if(timePassedHr<=9)
			formattedTime="0";				
		formattedTime+=timePassedHr+":";			
		if(timePassedMin<=9)
			formattedTime+="0";				
		formattedTime+=timePassedMin+":";			
		if(timePassedSec<=9)
			formattedTime+="0";				
		formattedTime+=timePassedSec;
		return formattedTime;
	}
}
